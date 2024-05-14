using CleanArchitecture.Domain.SignalRModels;

namespace CleanArchitecture.Infrastructure.SignalR.Hubs;

public interface IGameHub
{
    //client func 

    Task InitializeGame(object result);

    Task CheckGameRunning(object result);
    Task JoinPlayer(object result , string name , List<string> playersNames);

    Task NotifyOtherInGroupWhenNewJoining(string name );
    Task UpdatePlayerScore(int score , string name); 
    Task CurrentQuestion(QuestionDataModel question , int questionIndex);
    Task GameStarted(bool haveVR);
    Task GameEnded(List<PlayerDataModel> playerDataModels);
    Task WaitingForNextQuestion(PlayerDataModel playerDataModel , QuestionDataModel questionDataModel , bool IsTimeOut);
    Task StartTimer(QuestionDataModel questionDataModel); 
    Task DisconnectedGame(); 
    Task DisconnectedPlayer(string name); 
}