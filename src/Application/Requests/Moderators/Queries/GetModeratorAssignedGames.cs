using System.Linq.Expressions;
using AutoMapper;
using CleanArchitecture.Application.Common.Interfaces;
using CleanArchitecture.Application.Requests.Games.Models;
using CleanArchitecture.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CleanArchitecture.Application.Requests.Moderators.Queries;

public class GetModeratorAssignedGames:IRequest<List<GameVm>>
{
    public GetModeratorAssignedGames(string moderatorId , List<Expression<Func<Game, object>>> includes = null)
    {
        ModeratorId = moderatorId;
        Includes = includes ?? new List<Expression<Func<Game, object>>>();

    }

    public string ModeratorId { get; set; }
    public List<Expression<Func<Game, object>>> Includes { get; set; }


}

public class GetModeratorAssignedGamesHandler : IRequestHandler<GetModeratorAssignedGames, List<GameVm>>
{
    private readonly IApplicationDbContext _dbContext;
    private readonly IMapper _mapper;

    public GetModeratorAssignedGamesHandler(IApplicationDbContext dbContext, IMapper mapper)
    {
        _dbContext = dbContext;
        _mapper = mapper;
    }

    public async Task<List<GameVm>> Handle(GetModeratorAssignedGames request, CancellationToken cancellationToken)
    {

        var query = _dbContext.Games.AsNoTracking();
        foreach (var include in request.Includes)
        {
            query = query.Include(include);
        }
        return _mapper.Map<List<GameVm>>(await query.Where(x=>x.ModeratorId==request.ModeratorId).ToListAsync(cancellationToken));
    }
}