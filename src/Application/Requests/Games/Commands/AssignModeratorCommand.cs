using CleanArchitecture.Application.Common.Exceptions;
using CleanArchitecture.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CleanArchitecture.Application.Requests.Games.Commands;

public class AssignModeratorCommand : IRequest<Unit>
{
    public int GameId { get; }
    public string ModeratorId { get; }

    public AssignModeratorCommand(int gameId, string moderatorId)
    {
        GameId = gameId;
        ModeratorId = moderatorId;
    }
}
public class AssignModeratorCommandHandler : IRequestHandler<AssignModeratorCommand, Unit>
{
    private readonly IApplicationDbContext _dbContext;

    public AssignModeratorCommandHandler(IApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Unit> Handle(AssignModeratorCommand request, CancellationToken cancellationToken)
    {

        var game =
            await _dbContext.Games.FirstOrDefaultAsync(x => x.Id == request.GameId, cancellationToken);
        if (game == null)
            throw new NotFoundException("No game with this id found " , request.GameId);
        game.ModeratorId = request.ModeratorId;
        await _dbContext.SaveChangesAsync(cancellationToken);
        return Unit.Value;
    }
}