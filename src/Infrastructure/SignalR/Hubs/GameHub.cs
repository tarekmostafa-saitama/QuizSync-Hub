using CleanArchitecture.Domain.SignalRModels;
using CleanArchitecture.Infrastructure.SignalR.Interfaces;
using Microsoft.AspNetCore.SignalR;

namespace CleanArchitecture.Infrastructure.SignalR.Hubs;

public class GameHub:Hub<IGameHub>
{
    private readonly IGameRoundDataService _gameRoundDataService;

    public GameHub(IGameRoundDataService gameRoundDataService )
    {
        _gameRoundDataService = gameRoundDataService;
    }

    #region Moderator

    public async Task InitializeGame( int code)
    {

       var result = _gameRoundDataService.InitializeGame(code, Context.ConnectionId);
    
           await Clients.Caller.InitializeGame(result);
           await Groups.AddToGroupAsync(Context.ConnectionId, code.ToString());
           var question= await GetCurrentQuestion(code);
           await Clients.Caller.CurrentQuestion(question.Item1, 0);

    }

    public async Task ChangeCurrentQuestionIndex(int code)
    {
        var index =_gameRoundDataService.ChangeCurrentQuestionIndex(code);
        var question= await GetCurrentQuestion(code);
        if (question.Item2)
        {
            var players = await _gameRoundDataService.GetTopFivePlayers(code);
            await Clients.Group(code.ToString()).GameEnded(players);
            await _gameRoundDataService.SaveRoundData(code);
        }
        await Clients.Caller.CurrentQuestion(question.Item1, index);

    }

    #endregion


    #region Player

    public async Task CheckGameRunning(int code)
    {

        var result = _gameRoundDataService.CheckGameRunning(code);

        await Clients.Caller.CheckGameRunning(result);


    }


    public async Task JoinPlayersGame(string form , int code)
    {
        var playersNames = _gameRoundDataService.GetPlayersNames(code);

        var result = _gameRoundDataService.JoinPlayersGame(form , code , Context.ConnectionId);
        string name = null; 
        if (result.Succeeded)
        { 
             name =_gameRoundDataService.ExtractNameFromForm(form);

            await Groups.AddToGroupAsync(Context.ConnectionId, code.ToString());
            await Clients.OthersInGroup(code.ToString()).NotifyOtherInGroupWhenNewJoining(name);          

        }
       
        await Clients.Caller.JoinPlayer(result, name, playersNames );


       


    }

    public async Task SendAnswer(string answer , int code , int questionId , int responseTime)
    {
      
        var result = _gameRoundDataService.PlayerAnswer(answer , code, Context.ConnectionId , questionId , responseTime);


        if (result.Item1 != null)
        {

            if (answer == null)
            {

                await Clients.Caller.WaitingForNextQuestion(result.Item1, result.Item2 , true);


            }
            else
            {
                await Clients.Caller.WaitingForNextQuestion(result.Item1, result.Item2 , false);

            }
            await Clients.Group(code.ToString()).UpdatePlayerScore(result.Item1.CurrentTotalScore, result.Item1.Name);

        }

    }

    #endregion


    #region Shared

    public override Task OnConnectedAsync()
    {

        return base.OnConnectedAsync();
    }

    public override async Task OnDisconnectedAsync(Exception exception)
    {
        var cId = Context.ConnectionId;

        var gameCode = _gameRoundDataService.RemoveGameRoundData(cId);

        if (gameCode != 0)
        {
            await Clients.Group(gameCode.ToString()).DisconnectedGame();
        }
        else
        {
           var data=  _gameRoundDataService.RemovePlayer(cId);
           if (data.Item1 != null && data.Item2 != 0)
           {
               await Groups.RemoveFromGroupAsync(cId, data.Item2.ToString());

               await Clients.OthersInGroup(data.Item2.ToString()).DisconnectedPlayer(data.Item1);

           }
          


        }
    }



    public async Task<(QuestionDataModel, bool)> GetCurrentQuestion(int code)
    {
       var question =await _gameRoundDataService.GetCurrentQuestion(code);
       return question;
    }


    public async Task SendQuestion(int code)
    {
        var game = _gameRoundDataService.GetGameData(code);

        if (game.CurrentQuestionIndex== 0)
        {
            game.IsGameStarted = true;
            var haveVR = _gameRoundDataService.IsVREnabled(code);
            await Clients.Group(code.ToString()).GameStarted(haveVR);
        }

        var question= await GetCurrentQuestion(code);
        if (!question.Item2)
        {
            await Clients.OthersInGroup(code.ToString()).CurrentQuestion(question.Item1 , game.CurrentQuestionIndex);
            await Clients.Caller.StartTimer(question.Item1);
        }
      


    }
    #endregion



}