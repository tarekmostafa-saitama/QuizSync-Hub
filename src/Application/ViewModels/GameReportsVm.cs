using CleanArchitecture.Application.Requests.GameSubmissions.Models;

namespace CleanArchitecture.Application.ViewModels;

public class GameReportsVm
{
    public List<GameRoundDataVm> GameRoundDataVm { get; set; }
    public List<GameSubmissionVm>GameSubmissionVms { get; set; }
}