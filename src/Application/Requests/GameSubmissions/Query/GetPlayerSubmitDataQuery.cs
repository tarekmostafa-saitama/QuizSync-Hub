using AutoMapper;
using CleanArchitecture.Application.Common.Interfaces;
using CleanArchitecture.Application.Requests.GameSubmissions.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CleanArchitecture.Application.Requests.GameSubmissions.Query;

public class GetPlayerSubmitDataQuery : IRequest<GameSubmissionVm>
{
    public GetPlayerSubmitDataQuery(int playerId)
    {
        PlayerId = playerId;
    }

    public int PlayerId { get; set; }


}

public class GetPlayerSubmitDataQueryHandler : IRequestHandler<GetPlayerSubmitDataQuery, GameSubmissionVm>
{
    private readonly IApplicationDbContext _dbContext;
    private readonly IMapper _mapper;


    public GetPlayerSubmitDataQueryHandler(IApplicationDbContext dbContext, IMapper mapper)
    {
        _dbContext = dbContext;
        _mapper = mapper;
    }
    public async Task<GameSubmissionVm> Handle(GetPlayerSubmitDataQuery request, CancellationToken cancellationToken)
    {
        var query = await _dbContext.GameSubmissions.FirstOrDefaultAsync(x => x.Id == request.PlayerId, cancellationToken);
        return _mapper.Map<GameSubmissionVm>(query);
    }
}
