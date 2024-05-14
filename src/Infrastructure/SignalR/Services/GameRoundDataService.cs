using System.Net;
using CleanArchitecture.Application.Common.Exceptions;
using CleanArchitecture.Application.Common.Models;
using CleanArchitecture.Application.Requests.GameSubmissions.Commands;
using CleanArchitecture.Domain.SignalRModels;
using CleanArchitecture.Infrastructure.SignalR.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace CleanArchitecture.Infrastructure.SignalR.Services;

public class GameRoundDataService : IGameRoundDataService
{
    private readonly IGameDataService _gameDataService;

    private readonly IPlayerDataService _playerDataService;

    private readonly IQuestionDataService _questionDataService;
    private readonly ISender _sender;

    private static object _lockObj = new Object();

     private static List<GameRoundData> _list = new List<GameRoundData>();

    public GameRoundDataService(IGameDataService gameDataService 
        , IPlayerDataService playerDataService, 
          IQuestionDataService questionDataService, 
        ISender sender)
    {
        _gameDataService = gameDataService;
        _playerDataService = playerDataService;
        _questionDataService = questionDataService;
        _sender = sender;
    }

    #region Game

    public Result InitializeGame(int code ,string connectionId)
    {
        lock (_lockObj)
        {
            var isExist = IsListContainGameCode(code);
            if (isExist) return Result.Failure(new[] { "Game Is Already running in other tab" });

            var gameData = _gameDataService.GetGameDataByCode(code, connectionId).Result;
            if (gameData == null) return Result.Failure(new[] { "Error While initialize the game" });

            _list.Add(new GameRoundData() { GameDataModel = gameData });

        }

        return Result.Success();
    }


    public int RemoveGameRoundData(string moderatorConnectionId)
    {
        var code = 0; 
        lock (_lockObj)
        {
            var removedList = _list.FirstOrDefault(x => x.GameDataModel.ModeratorConnectionId == moderatorConnectionId);

            if (removedList == null) return code; 

            code=removedList.GameDataModel.Code;
            _list.Remove(removedList);
        }

        return code; 
    }

    public GameRoundData GetGameData(int code)
    {
        return _list.FirstOrDefault(x => x.GameDataModel.Code == code);
    }

    public int ChangeCurrentQuestionIndex(int code)
    {
        var game = GetGameData(code); 
        if (game == null) throw  new  NotFoundException("game not found");
        game.CurrentQuestionIndex += 1;
        IsLastQuestion(code);

        return game.CurrentQuestionIndex;
    }

    #endregion





        #region Player

    public Result CheckGameRunning(int code)
    {
        var isExist = IsListContainGameCode(code);  
        if(!isExist) return Result.Failure(new[] { "Game Not Started Yet , Please Try again later ..." });

        var isStarted = IsGameStarted(code);
        if (isStarted) return Result.Failure(new[] { "Game Is Already Started ..." });

        return  Result.Success();
    }

    public Result JoinPlayersGame(string form, int code , string cId)
    {
        lock (_lockObj)
        {

            var name = ExtractNameFromForm(form); 

            var isNameIsExistInList = IsNameIsExistInList(name,code);
            var isGameStarted = IsGameStarted(code);

            if(isGameStarted) return Result.Failure(new[] { "Game Is Already Started" });


            if(isNameIsExistInList) return Result.Failure(new[] { "Name is taken, please choose a new unique one" });

            var playerData = _playerDataService.JoinPlayer(name ,form , cId).Result; 
            if(playerData==null) return Result.Failure(new[] { "Error while adding new player..!" });


            var round = GetRoundByCode(code); 
            if(round==null) return Result.Failure(new[] { "Game not found..!" });


            round.PlayerDataModels.Add(playerData);
            return  Result.Success() ;
        }
    }

    public (string, int) RemovePlayer(string cId)
    {
        var name = "";
        var code =0; 
        foreach (var game in _list)
        {
            foreach (var playerData in game.PlayerDataModels.Where(playerData => playerData.ConnectionId == cId))
            {
                game.PlayerDataModels.Remove(playerData);
                name = playerData.Name;
                code = game.GameDataModel.Code;
                return (name , code);
            }
        }

        return (name,code);
    }

    public List<string> GetPlayersNames(int code)
    {
        var game = _list.FirstOrDefault(x => x.GameDataModel.Code == code);
        if (game == null) return null;

        var players = game.PlayerDataModels.Select(x => x.Name).ToList();
        return players;
    }

    public void IsLastQuestion(int code)
    {
        var game = GetGameData(code);

        if (game.GameDataModel.Questions.Count <  game.CurrentQuestionIndex+1)
        {
            game.IsLastQuestion = true;
          
        }
           
      
    }

    #endregion


    #region Question
    public async Task<(QuestionDataModel, bool)> GetCurrentQuestion(int  code)
    {
        var gameRoundData = _list.FirstOrDefault(x => x.GameDataModel.Code == code);
        if (gameRoundData == null)
        {
            return (null, false);
        }

        if (gameRoundData.IsLastQuestion)
        {
            return (null, true);
        }
        var question = gameRoundData.GameDataModel.Questions[gameRoundData.CurrentQuestionIndex];
     
        return (question, false);
    }


    public (PlayerDataModel, QuestionDataModel )PlayerAnswer(string answer , int code, string cId , int questionId , int responseTime)
    {
        lock (_lockObj)
        {
            var game = GetRoundByCode(code);
            var player = game.PlayerDataModels.FirstOrDefault(x => x.ConnectionId == cId);
            var question = game.GameDataModel.Questions.FirstOrDefault(x => x.Id == questionId);
            if (player != null && player.SubmittedQuestionAndAnswersList.FirstOrDefault(x => x.Id == question?.Id) != null) return (null, null) ;

            var playerResult = _playerDataService.SubmittedUserResult(question, player, answer , responseTime);



            return (playerResult , question);
        }
     
    }
    #endregion

    public bool IsListContainGameCode(int code)
    {
        var game = _list.FirstOrDefault(x => x.GameDataModel.Code == code);

        return game != null;
    }


    public async Task<int> SaveRoundData(int code)
    {
        var gameRoundData = _list.FirstOrDefault(x => x.GameDataModel.Code == code);

        var result =await _sender.Send(new CreateBulkPlayersSubmissionsCommand(gameRoundData));
        return result;
    }
    private bool IsGameStarted(int code)
    {
        var isStarted = _list.FirstOrDefault(x => x.GameDataModel.Code == code).IsGameStarted;

        return isStarted;
    }

    private bool IsNameIsExistInList(string name, int code)
    {
        var gameRound = GetRoundByCode(code);

        if(gameRound==null) return false;

        return gameRound.PlayerDataModels.Any(player => player.Name.ToLower() == name.ToLower());
    }

    private GameRoundData GetRoundByCode(int code)
    {
        var gameRound = _list.FirstOrDefault(x => x.GameDataModel.Code == code);

        return gameRound;
    }

    public string ExtractNameFromForm(string form)
    {
        var dictionary = form.Trim('{','}')
            .Split('&')
            .Select(x => x.Split('='))
            .ToDictionary(x => x[0], x => x[1]);


        var name = WebUtility.UrlDecode(dictionary["FullName"]);
        var trimName = name?.Trim();
        return trimName;
    }


    public async Task<List<PlayerDataModel>> GetTopFivePlayers(int code)
    {
        var game = GetRoundByCode(code);
        return game.PlayerDataModels.OrderByDescending(x => x.CurrentTotalScore).ToList();
    }

    public bool IsVREnabled(int code)
    {
        var game = GetRoundByCode(code);
        return game.GameDataModel.IsVrEnabled; 
    }
}