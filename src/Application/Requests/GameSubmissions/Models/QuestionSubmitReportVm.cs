namespace CleanArchitecture.Application.Requests.GameSubmissions.Models;

public class QuestionSubmitReportVm
{
    public int TotalSubmits { get; set; }
    public string QuestionTitle { get; set; }
    public Dictionary<string, int> ChoicesReportData { get; set; }

    public QuestionSubmitReportVm()
    {
        ChoicesReportData = new Dictionary<string, int>();
    }
}