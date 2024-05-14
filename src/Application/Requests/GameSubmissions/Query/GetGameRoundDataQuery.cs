using System.Linq.Expressions;
using AutoMapper;
using CleanArchitecture.Application.Common.Interfaces;
using CleanArchitecture.Application.Requests.GameSubmissions.Models;
using CleanArchitecture.Application.Requests.Questions.Models;
using CleanArchitecture.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CleanArchitecture.Application.Requests.GameSubmissions.Query;

public class GetGameRoundDataQuery : IRequest<GameRoundDataVm>
{
    public int GameId { get; set; }
    public int Identifier { get; set; }
    public GetGameRoundDataQuery(int gameId, int identifier)
    {
        GameId = gameId;
        Identifier = identifier;
    }
}

public class GetGameRoundDataQueryHandler : IRequestHandler<GetGameRoundDataQuery, GameRoundDataVm>
{

    private readonly IApplicationDbContext _dbContext;
    private readonly IMapper _mapper;


    public GetGameRoundDataQueryHandler(IApplicationDbContext dbContext, IMapper mapper)
    {
        _dbContext = dbContext;
        _mapper = mapper;
    }
    public async Task<GameRoundDataVm> Handle(GetGameRoundDataQuery request, CancellationToken cancellationToken)
    {
       
        var query = _dbContext.GameSubmissions.AsNoTracking();

        var game =await  _dbContext.Games.Where(x=>x.Id==request.GameId).Include(x=>x.Questions).FirstOrDefaultAsync(cancellationToken);
        var submissions =await  query.Where(x => x.GameId == request.GameId && 
                                           x.SubmissionIdentifier == request.Identifier)
           
            .ToListAsync(cancellationToken);

        return new GameRoundDataVm()
        {
            GameId = request.GameId,
            DateTime = submissions.FirstOrDefault()!.DateTime,
            ParticipantsNumber = submissions.Count,
            RoundIdentifier = request.Identifier,
            QuestionVms = _mapper.Map<List<QuestionVm>>(game?.Questions),
            GameSubmissionVms = _mapper.Map<List<GameSubmissionVm>>(submissions)
        };
    }
}
