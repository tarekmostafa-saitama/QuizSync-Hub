using AutoMapper;
using CleanArchitecture.Application.Common.Helpers;
using CleanArchitecture.Application.Common.Interfaces;
using CleanArchitecture.Domain.SignalRModels;
using MediatR;

namespace CleanArchitecture.Application.Requests.GameSubmissions.Commands;

public class CreateBulkPlayersSubmissionsCommand :IRequest<int>
{
    public GameRoundData GameRoundData { get; set; }
    public CreateBulkPlayersSubmissionsCommand(GameRoundData gameRoundData)
    {
        GameRoundData = gameRoundData;
    }
}


public class CreateBulkPlayersSubmissionsCommandHandler : IRequestHandler<CreateBulkPlayersSubmissionsCommand, int>
{
    private readonly IApplicationDbContext _dbContext;
    private readonly IMapper _mapper;
    private readonly ISender _sender;

    public CreateBulkPlayersSubmissionsCommandHandler(IApplicationDbContext dbContext, IMapper mapper, ISender sender)
    {
        _dbContext = dbContext;
        _mapper = mapper;
        _sender = sender;
    }
    public async Task<int> Handle(CreateBulkPlayersSubmissionsCommand request, CancellationToken cancellationToken)
    {
        List<Domain.Entities.GameSubmission> players = new List<Domain.Entities.GameSubmission>();
        var identifier = Utilities.GenerateRandomNumbers();
        var date = DateTime.UtcNow;
        foreach (var player in request.GameRoundData.PlayerDataModels)
        {
            Domain.Entities.GameSubmission playerData = new Domain.Entities.GameSubmission
            {
                GameId = request.GameRoundData.GameDataModel.Id,
                SubmissionIdentifier = identifier,
                DateTime = date,
                Name = player.Name, 
                SubmittedForm = player.SubmittedForm,
                TotalScore = player.CurrentTotalScore,
                SubmittedQuestionAndAnswersList = player.SubmittedQuestionAndAnswersList
            };
            players.Add(playerData);

        }

        _dbContext.GameSubmissions.AddRange(players);
        await _dbContext.SaveChangesAsync(cancellationToken);
        return identifier;


    }
}