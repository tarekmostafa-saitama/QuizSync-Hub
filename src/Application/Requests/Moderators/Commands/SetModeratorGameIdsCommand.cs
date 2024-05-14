using CleanArchitecture.Application.Common.Exceptions;
using CleanArchitecture.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CleanArchitecture.Application.Requests.Moderators.Commands;

public class SetModeratorGameIdsCommand : IRequest<Unit>
{
    public string ModeratorId { get; set; }
    public List<int> Ids { get; set; }

    public SetModeratorGameIdsCommand(string moderatorId , List<int> ids)
    {
        ModeratorId = moderatorId;
        Ids = ids;
    }
}
public class SetModeratorGameIdsCommandHandler : IRequestHandler<SetModeratorGameIdsCommand, Unit>
{
    private readonly IApplicationDbContext _dbContext;

    public SetModeratorGameIdsCommandHandler(IApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    public async Task<Unit> Handle(SetModeratorGameIdsCommand request, CancellationToken cancellationToken)
    {
        foreach (var id in request.Ids)
        {
           var game= await _dbContext.Games.FindAsync(id);
           if(game == null) 
             throw new  NotFoundException("Game not found", id);
           if (game.ModeratorId != null)
           {
               var moderator = await _dbContext.ApplicationUsers.FirstOrDefaultAsync(x =>x.Id==game.ModeratorId,cancellationToken);
               moderator.Games.Remove(game);
               await _dbContext.SaveChangesAsync(cancellationToken); 
           }

           game.ModeratorId = request.ModeratorId;
           _dbContext.Games.Update(game);
           await _dbContext.SaveChangesAsync(cancellationToken); 
        }

        return Unit.Value;
    }
}