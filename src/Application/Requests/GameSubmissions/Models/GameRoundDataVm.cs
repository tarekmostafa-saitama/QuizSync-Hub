using CleanArchitecture.Application.Requests.Questions.Models;

namespace CleanArchitecture.Application.Requests.GameSubmissions.Models;

public class GameRoundDataVm
{
    public DateTime DateTime { get; set; }
    public int GameId { get; set; }
    public int ParticipantsNumber { get; set; }
    public int RoundIdentifier { get; set; }

    public List<QuestionVm> QuestionVms { get; set; } 
    public List<GameSubmissionVm> GameSubmissionVms { get; set; }
}