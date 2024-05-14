namespace CleanArchitecture.Domain.SignalRModels;

public class PlayerDataModel
{
    public PlayerDataModel()
    {
        SubmittedQuestionAndAnswersList = new List<SubmittedQuestionAndAnswers>();
    }
    public string Name { get; set; }
    public string SubmittedForm { get; set; }
    public int CurrentTotalScore { get; set; }
    public bool IsCurrentQuestionCorrect { get; set; }
    public string ConnectionId { get; set; }
    public List<SubmittedQuestionAndAnswers> SubmittedQuestionAndAnswersList { get; set; }
}