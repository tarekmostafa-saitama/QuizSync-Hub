using AutoMapper;
using CleanArchitecture.Application.Common.Interfaces;
using MediatR;
using System.Linq.Expressions;
using CleanArchitecture.Application.Requests.Games.Models;
using CleanArchitecture.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace CleanArchitecture.Application.Requests.Games.Queries;

public class GetGameByCodeQuery : IRequest<GameVm>
{
    public int GameCode { get; set; }
    public List<Expression<Func<Game, object>>> Includes { get; set; }

    public GetGameByCodeQuery(int gameCode, List<Expression<Func<Game, object>>> includes=null)
    {
        Includes = includes ?? new List<Expression<Func<Game, object>>>();
        GameCode = gameCode;
    }
}
public class GetGameByCodeQueryHandler : IRequestHandler<GetGameByCodeQuery, GameVm>
{
    private readonly IApplicationDbContext _dbContext;
    private readonly IMapper _mapper;

    public GetGameByCodeQueryHandler(IApplicationDbContext dbContext, IMapper mapper)
    {
        _dbContext = dbContext;
        _mapper = mapper;
    }
    public async Task<GameVm> Handle(GetGameByCodeQuery request, CancellationToken cancellationToken)
    {
        var model = _dbContext.Games.AsNoTracking();
        foreach (var include in request.Includes)
        {
            model = model.Include(include);
        }

        return _mapper.Map<GameVm>(await model.FirstOrDefaultAsync(x => x.Code == request.GameCode,
            cancellationToken));
    }
}