using CleanArchitecture.Domain.SignalRModels;

namespace CleanArchitecture.Infrastructure.SignalR.Interfaces;

public interface IGameDataService
{
    Task<GameDataModel> GetGameDataByCode(int code, string connectionId); 
}