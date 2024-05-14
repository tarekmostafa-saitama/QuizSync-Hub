namespace CleanArchitecture.Domain.SignalRModels;

public class QuestionDataModel
{
    public QuestionDataModel()
    {
        ChoiceDataModels = new List<ChoiceDataModel>();
    }
    public int Id { get; set; }
    public string Title { get; set; }
    public Score Score { get; set; }
    public int Duration { get; set; }
    public string PhotoUrl { get; set; }

    public List<ChoiceDataModel> ChoiceDataModels { get; set; }
}