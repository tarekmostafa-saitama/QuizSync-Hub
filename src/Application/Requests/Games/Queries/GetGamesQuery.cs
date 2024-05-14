using AutoMapper;
using CleanArchitecture.Application.Common.Interfaces;
using MediatR;
using System.Linq.Expressions;
using CleanArchitecture.Application.Requests.Games.Models;
using CleanArchitecture.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace CleanArchitecture.Application.Requests.Games.Queries;

public class GetGamesQuery : IRequest<List<GameVm>>
{
    public List<Expression<Func<Game, object>>> Includes { get; set; }

    public GetGamesQuery(List<Expression<Func<Game, object>>> includes=null)
    {
        Includes = includes ?? new List<Expression<Func<Game, object>>>();
    }
}

public class GetGamesQueryHandler : IRequestHandler<GetGamesQuery, List<GameVm>>
{
    private readonly IApplicationDbContext _dbContext;
    private readonly IMapper _mapper;

    public GetGamesQueryHandler(IApplicationDbContext dbContext, IMapper mapper)
    {
        _dbContext = dbContext;
        _mapper = mapper;
    }
    public async Task<List<GameVm>> Handle(GetGamesQuery request, CancellationToken cancellationToken)
    {
        var query = _dbContext.Games.AsNoTracking();
        foreach (var include in request.Includes)
        {
            query = query.Include(include);
        }
        return _mapper.Map<List<GameVm>>(await query.ToListAsync(cancellationToken));
    }
}
