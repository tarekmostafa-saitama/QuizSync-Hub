using CleanArchitecture.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CleanArchitecture.Application.Requests.Moderators.Queries;

public class IsModeratorHasAccessToGame : IRequest<bool>
{
    public IsModeratorHasAccessToGame(int gameCode , string userId)
    {
        GameCode = gameCode; 
        UserId = userId;

    }
    public int  GameCode { get; set; }
    public string UserId { get; set; }
}

public class IsModeratorHasAccessToGameHandler : IRequestHandler<IsModeratorHasAccessToGame, bool>
{
    private readonly IApplicationDbContext _dbContext;

    public IsModeratorHasAccessToGameHandler(IApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    public async Task<bool> Handle(IsModeratorHasAccessToGame request, CancellationToken cancellationToken)
    {
        var isAny =  _dbContext.Games.Any(x => x.Code == request.GameCode && x.ModeratorId == request.UserId);
        return isAny; 
    }
}
