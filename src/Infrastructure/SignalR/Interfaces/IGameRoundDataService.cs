using CleanArchitecture.Application.Common.Models;
using CleanArchitecture.Domain.SignalRModels;
using Microsoft.AspNetCore.Http;

namespace CleanArchitecture.Infrastructure.SignalR.Interfaces;

public interface IGameRoundDataService
{
    Result InitializeGame(int code, string connectionId);
    int RemoveGameRoundData(string moderatorConnectionId);

    GameRoundData GetGameData(int code);

    int ChangeCurrentQuestionIndex(int code);
    Task<(QuestionDataModel, bool)> GetCurrentQuestion(int code);
     (PlayerDataModel , QuestionDataModel) PlayerAnswer(string answer, int code, string cId , int questionId ,int responseTime);
    Result CheckGameRunning(int code); 
    Result JoinPlayersGame(string form , int code , string cId);
    ( string, int) RemovePlayer( string cId);
    string ExtractNameFromForm(string form);

    List<string> GetPlayersNames(int code);

    void IsLastQuestion(int code);
    bool IsListContainGameCode(int code);
    Task<int> SaveRoundData(int code);
    Task<List<PlayerDataModel>> GetTopFivePlayers(int code);

    bool IsVREnabled(int code);  


}