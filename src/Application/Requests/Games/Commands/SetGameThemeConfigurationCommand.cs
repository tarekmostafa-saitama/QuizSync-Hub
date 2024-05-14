using AutoMapper;
using CleanArchitecture.Application.Common.Helpers;
using CleanArchitecture.Application.Common.Interfaces;
using CleanArchitecture.Application.Requests.Games.Models;
using CleanArchitecture.Domain.Entities;
using MediatR;

namespace CleanArchitecture.Application.Requests.Games.Commands;

public class SetGameThemeConfigurationCommand : IRequest<int>
{
    public int GameId { get; set; }
    public ThemeConfigurationVm ThemeConfigurationVm { get; set; }

    public SetGameThemeConfigurationCommand(int gameId, ThemeConfigurationVm themeConfiguration)
    {
        GameId = gameId;
        ThemeConfigurationVm = themeConfiguration;
    }
}

public class SetGameThemeConfigurationCommandHandler : IRequestHandler<SetGameThemeConfigurationCommand, int>
{
    private readonly IApplicationDbContext _dbContext;
    private readonly IMapper _mapper;

    public SetGameThemeConfigurationCommandHandler(IApplicationDbContext dbContext, IMapper mapper, ISender sender)
    {
        _dbContext = dbContext;
        _mapper = mapper;
    }

    public async Task<int> Handle(SetGameThemeConfigurationCommand request, CancellationToken cancellationToken)
    {
        if (request.ThemeConfigurationVm.HeaderLogo != null)
        {
            if (!string.IsNullOrEmpty(request.ThemeConfigurationVm.HeaderLogoUrl))
            {
                FileManager.Delete(request.ThemeConfigurationVm.HeaderLogoUrl, "uploads");
            }

            request.ThemeConfigurationVm.HeaderLogoUrl =
                await FileManager.Upload(request.ThemeConfigurationVm.HeaderLogo, "uploads", false);
        }


        if (request.ThemeConfigurationVm.BackgroundImage != null)
        {
            if (!string.IsNullOrEmpty(request.ThemeConfigurationVm.BackgroundImageUrl))
            {
                FileManager.Delete(request.ThemeConfigurationVm.BackgroundImageUrl, "uploads");
            }

            request.ThemeConfigurationVm.BackgroundImageUrl =
                await FileManager.Upload(request.ThemeConfigurationVm.BackgroundImage, "uploads", false);
        }




        var entity = await _dbContext.Games.FindAsync(request.GameId);
        if (entity == null)
            throw new Exception("No game with this Id");

        var model = _mapper.Map<ThemeConfiguration>(request.ThemeConfigurationVm);
        entity.ThemeConfiguration = model; 

        await _dbContext.SaveChangesAsync(cancellationToken);
        return entity.Id;
    }
}