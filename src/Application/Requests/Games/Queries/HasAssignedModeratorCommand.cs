using CleanArchitecture.Application.Common.Exceptions;
using CleanArchitecture.Application.Common.Interfaces;
using MediatR;

namespace CleanArchitecture.Application.Requests.Games.Queries;

public class HasAssignedModeratorCommand:IRequest<bool>
{
    public int GameId { get; set; }

    public HasAssignedModeratorCommand(int gameId)
    {
        GameId = gameId;
    }
}

public class HasAssignedModeratorCommandHandler : IRequestHandler<HasAssignedModeratorCommand, bool>
{
    private readonly IApplicationDbContext _context;

    public HasAssignedModeratorCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }
    public async Task<bool> Handle(HasAssignedModeratorCommand request, CancellationToken cancellationToken)
    {
        var game = await _context.Games.FindAsync(request.GameId);
        if (game == null) throw new NotFoundException("No Game Found");

        return game.ModeratorId != null; 
    }
}