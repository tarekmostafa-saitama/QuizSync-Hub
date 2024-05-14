using AutoMapper;
using CleanArchitecture.Application.Common.Interfaces;
using CleanArchitecture.Application.Requests.GameSubmissions.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CleanArchitecture.Application.Requests.GameSubmissions.Query;

public class GetRoundsPlayersReportQuery :IRequest<List<GameSubmissionVm>>
{
    public int GameId { get; set; }

    public GetRoundsPlayersReportQuery(int gameId)
    {
        GameId = gameId; 
    }
}

public class GetRoundsPlayersReportQueryHandler : IRequestHandler<GetRoundsPlayersReportQuery, List<GameSubmissionVm>>
{
    private readonly IApplicationDbContext _dbContext;
    private readonly IMapper _mapper;


    public GetRoundsPlayersReportQueryHandler(IApplicationDbContext dbContext, IMapper mapper)
    {
        _dbContext = dbContext;
        _mapper = mapper;
    }
    public async Task<List<GameSubmissionVm>> Handle(GetRoundsPlayersReportQuery request, CancellationToken cancellationToken)
    {
        var query = await _dbContext.GameSubmissions.Where(x => x.GameId== request.GameId).ToListAsync(cancellationToken);
        return _mapper.Map<List<GameSubmissionVm>>(query);
    }
}