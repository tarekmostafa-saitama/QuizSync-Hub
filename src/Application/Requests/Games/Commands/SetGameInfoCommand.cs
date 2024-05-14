using AutoMapper;
using CleanArchitecture.Application.Common.Helpers;
using CleanArchitecture.Application.Common.Interfaces;
using CleanArchitecture.Application.Requests.Games.Models;
using CleanArchitecture.Domain.Entities;
using MediatR;

namespace CleanArchitecture.Application.Requests.Games.Commands;

public class SetGameInfoCommand : IRequest<int>
{
    public GameVm Model { get; set; }

    public SetGameInfoCommand(GameVm model)
    {
        Model = model;
    }
}

public class SetGameInfoCommandHandler : IRequestHandler<SetGameInfoCommand, int>
{
    private readonly IApplicationDbContext _dbContext;
    private readonly IMapper _mapper;
    private readonly ISender _sender;

    public SetGameInfoCommandHandler(IApplicationDbContext dbContext, IMapper mapper, ISender sender)
    {
        _dbContext = dbContext;
        _mapper = mapper;
        _sender = sender;
    }

    public async Task<int> Handle(SetGameInfoCommand request, CancellationToken cancellationToken)
    {
        var model = _mapper.Map<Game>(request.Model);

        if (model.Id == default)
        {

            Game newGame = new Game
            {
                Name = model.Name, 
                Code=Utilities.GenerateRandomNumbers()  ,
                IsVrEnabled = model.IsVrEnabled, 
                ThemeConfiguration = new ThemeConfiguration()
            };

            _dbContext.Games.Add(newGame);
            await _dbContext.SaveChangesAsync(cancellationToken);
            return newGame.Id;
        }
        else
        {
            var game = await _dbContext.Games.FindAsync(model.Id);

            if (game != null)
            {
                game.Name = model.Name;
                game.IsVrEnabled = model.IsVrEnabled;
            }
            await _dbContext.SaveChangesAsync(cancellationToken);

            return game.Id;
        }
    }
}
