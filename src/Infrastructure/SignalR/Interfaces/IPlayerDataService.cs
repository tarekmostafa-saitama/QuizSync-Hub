using CleanArchitecture.Application.Common.Models;
using CleanArchitecture.Domain.SignalRModels;

namespace CleanArchitecture.Infrastructure.SignalR.Interfaces;

public interface IPlayerDataService
{
    Task<PlayerDataModel> JoinPlayer(string name, string form ,string cId); 
    Result RemovePlayer( string cId);

    PlayerDataModel SubmittedUserResult(QuestionDataModel question, PlayerDataModel player , string answer, int reponseTime);  

}