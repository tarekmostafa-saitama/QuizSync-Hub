namespace CleanArchitecture.Domain.SignalRModels;
public class GameDataModel
{
    public GameDataModel()
    {
        Questions = new List<QuestionDataModel>();
    }

    public int Id { get; set; }
    public string Name { get; set; }
    public int Code { get; set; }
    public bool IsVrEnabled { get; set; }
    public string ModeratorId { get; set; }
    public string ModeratorConnectionId { get; set; }

    
    public List<QuestionDataModel> Questions { get; set; }
    public ThemeConfigurationModel ThemeConfigurationModel { get; set; }


}
