namespace CleanArchitecture.Domain.SignalRModels;

public class GameRoundData
{

    public GameRoundData()
    {
        PlayerDataModels = new List<PlayerDataModel>();
    }
    public GameDataModel GameDataModel { get; set; }
    public List<PlayerDataModel> PlayerDataModels { get; set; }

    public bool IsGameStarted { get; set; }
    public bool IsLastQuestion { get; set; }


    public int CurrentQuestionIndex { get; set; }

}