using AutoMapper;
using CleanArchitecture.Application.Common.Interfaces;
using CleanArchitecture.Application.Requests.GameSubmissions.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CleanArchitecture.Application.Requests.GameSubmissions.Query;

public class GetTopFivePlayersQuery:IRequest<List<GameSubmissionVm>>
{
    public GetTopFivePlayersQuery(int identifier)
    {
        Identifier = identifier;
    }

    public int Identifier { get; set; }
}

public class GetTopFivePlayersQueryHandler : IRequestHandler<GetTopFivePlayersQuery , List<GameSubmissionVm>>
{

    private readonly IApplicationDbContext _dbContext;
    private readonly IMapper _mapper;

    public GetTopFivePlayersQueryHandler(IApplicationDbContext dbContext, IMapper mapper)
    {
        _dbContext = dbContext;
        _mapper = mapper;
    }
    public async Task<List<GameSubmissionVm>> Handle(GetTopFivePlayersQuery request, CancellationToken cancellationToken)
    {
        var players =await  _dbContext.GameSubmissions.Where(x => x.SubmissionIdentifier == request.Identifier).OrderByDescending(x => x.TotalScore)
            .Take(5).ToListAsync(cancellationToken);

        return  _mapper.Map<List<GameSubmissionVm>>(players);
    }
}
