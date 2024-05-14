using CleanArchitecture.Application.Common.Interfaces;
using CleanArchitecture.Application.Requests.Games.Commands;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CleanArchitecture.Application.Requests.GameSubmissions.Commands;

public class DeleteGameSubmitCommand : IRequest<bool>
{
    public int SubmitId { get; }

    public DeleteGameSubmitCommand(int submitId)
    {
        SubmitId = submitId;
    }
}
public class DeleteGameSubmitCommandHandler : IRequestHandler<DeleteGameSubmitCommand, bool>
{
    private readonly IApplicationDbContext _dbContext;

    public DeleteGameSubmitCommandHandler(IApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    public async Task<bool> Handle(DeleteGameSubmitCommand request, CancellationToken cancellationToken)
    {
        var model = await _dbContext.GameSubmissions.AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == request.SubmitId, cancellationToken);
        if (model == null)
            return false;
        _dbContext.GameSubmissions.Remove(model);
        await _dbContext.SaveChangesAsync(cancellationToken);
        return true;
    }
}