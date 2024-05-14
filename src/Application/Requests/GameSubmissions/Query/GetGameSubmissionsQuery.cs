using System.Linq.Expressions;
using AutoMapper;
using CleanArchitecture.Application.Common.Interfaces;
using CleanArchitecture.Application.Requests.Games.Models;
using CleanArchitecture.Application.Requests.GameSubmissions.Models;
using CleanArchitecture.Application.Requests.Questions.Models;
using CleanArchitecture.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CleanArchitecture.Application.Requests.GameSubmissions.Query;

public class GetGameSubmissionsQuery :IRequest<List<GameRoundDataVm>>
{
    public int GameId { get; set; }
    public List<Expression<Func<GameSubmission, object>>> Includes { get; }
    public GetGameSubmissionsQuery(int gameId , List<Expression<Func<GameSubmission, object>>> includes=null)
    {
        GameId=gameId;
        Includes = includes ??new List<Expression<Func<GameSubmission, object>>>();
    }
}

public class GetGameSubmissionsQueryHandler : IRequestHandler<GetGameSubmissionsQuery, List<GameRoundDataVm>>
{

    private readonly IApplicationDbContext _dbContext;
    private readonly IMapper _mapper;


    public GetGameSubmissionsQueryHandler(IApplicationDbContext dbContext, IMapper mapper)
    {
        _dbContext = dbContext;
        _mapper = mapper;
    }
    public async Task<List<GameRoundDataVm>> Handle(GetGameSubmissionsQuery request, CancellationToken cancellationToken)
    {
        List<GameRoundDataVm> models = new List<GameRoundDataVm>();
        var query = _dbContext.GameSubmissions.AsNoTracking();
        foreach (var include in request.Includes)
        {
            query = query.Include(include);
        }

        var submissions = await query.Where(x => x.GameId == request.GameId)
            .GroupBy(x=>x.DateTime)
            
            .ToListAsync(cancellationToken);

        var game = await _dbContext.Games.Where(x => x.Id == request.GameId).Include(x => x.Questions).FirstOrDefaultAsync(cancellationToken);


        submissions.ForEach(submit =>
        {
            models.Add(new GameRoundDataVm()
                {
                    GameId = request.GameId,
                    ParticipantsNumber = submit.Count(),
                    DateTime = submit.Key,
                    RoundIdentifier = submit.Select(x=>x.SubmissionIdentifier).First(),
                    QuestionVms = _mapper.Map<List<QuestionVm>>(game?.Questions),
                    GameSubmissionVms = _mapper.Map<List<GameSubmissionVm>>(submit.ToList())
                }
            );
        });
  
        return models; 
    }
}
