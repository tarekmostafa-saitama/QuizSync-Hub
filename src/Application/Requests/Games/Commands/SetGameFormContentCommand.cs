using AutoMapper;
using CleanArchitecture.Application.Common.Interfaces;
using MediatR;

namespace CleanArchitecture.Application.Requests.Games.Commands;

public class SetGameFormContentCommand : IRequest<int>
{
    public int GameId { get; set; }
    public string FormContent { get; set; }

    public SetGameFormContentCommand(int gameId, string formContent)
    {
        GameId = gameId;
        FormContent = formContent;
    }
}

public class SetGameFormContentCommandHandler : IRequestHandler<SetGameFormContentCommand, int>
{
    private readonly IApplicationDbContext _dbContext;
    private readonly IMapper _mapper;

    public SetGameFormContentCommandHandler(IApplicationDbContext dbContext, IMapper mapper, ISender sender)
    {
        _dbContext = dbContext;
        _mapper = mapper;
    }

    public async Task<int> Handle(SetGameFormContentCommand request, CancellationToken cancellationToken)
    {
        var entity = await _dbContext.Games.FindAsync(request.GameId);
        if (entity == null)
            throw new Exception("No Game with this Id");
        entity.FormContent = request.FormContent;
        await _dbContext.SaveChangesAsync(cancellationToken);
        return entity.Id;
    }
}
