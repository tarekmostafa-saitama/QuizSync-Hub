using CleanArchitecture.Application.Common.Interfaces;
using CleanArchitecture.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CleanArchitecture.Application.Requests.Games.Commands;

public class DeleteGameCommand : IRequest<bool>
{
    public int GameId { get; }

    public DeleteGameCommand(int gameId)
    {
        GameId = gameId;
    }
}
public class DeleteGameCommandHandler : IRequestHandler<DeleteGameCommand, bool>
{
    private readonly IApplicationDbContext _dbContext;

    public DeleteGameCommandHandler(IApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    public async Task<bool> Handle(DeleteGameCommand request, CancellationToken cancellationToken)
    {
        var model = await _dbContext.Games.AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == request.GameId, cancellationToken);
        if (model == null)
            return false;
        _dbContext.Games.Remove(model);
        await _dbContext.SaveChangesAsync(cancellationToken);
        return true;
    }
}