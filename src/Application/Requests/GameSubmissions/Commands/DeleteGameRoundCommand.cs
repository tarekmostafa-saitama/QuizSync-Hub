using CleanArchitecture.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CleanArchitecture.Application.Requests.GameSubmissions.Commands;

public class DeleteGameRoundCommand:IRequest<bool>
{
    public int Identifier { get; set; }

    public DeleteGameRoundCommand(int identifier)
    {
        Identifier = identifier; 
    }
}

public class DeleteGameRoundCommandHandler : IRequestHandler<DeleteGameRoundCommand, bool>
{
    private readonly IApplicationDbContext _dbContext;

    public DeleteGameRoundCommandHandler(IApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    public async Task<bool> Handle(DeleteGameRoundCommand request, CancellationToken cancellationToken)
    {
        var round =await  _dbContext.GameSubmissions.Where(x => x.SubmissionIdentifier == request.Identifier).ToListAsync(cancellationToken);
        if(!round.Any()) return false;

        _dbContext.GameSubmissions.RemoveRange(round);
        await _dbContext.SaveChangesAsync(cancellationToken);
        return true; 

    }
}
