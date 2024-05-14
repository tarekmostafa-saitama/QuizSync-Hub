namespace CleanArchitecture.Domain.SignalRModels;

public class SubmittedQuestionAndAnswers
{
    public SubmittedQuestionAndAnswers()
    {
        Choices = new List<ChoiceDataModel>();
    }

    public int Id { get; set; }
    public string QuestionTitle { get; set; }
    public int QuestionScore { get; set; }
    public List<ChoiceDataModel> Choices { get; set; }

    public string SelectedChoice { get; set; }
    public string CorrectChoice { get; set; }
    public bool IsCorrect { get; set; }
}