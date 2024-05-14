    using CleanArchitecture.Application.Common.Interfaces;
using CleanArchitecture.Application.Requests.Games.Models;
using CleanArchitecture.Application.Requests.Games.Queries;
using CleanArchitecture.Domain.Entities;
using System.Linq.Expressions;
using CleanArchitecture.Application.Requests.Moderators.Queries;
    using CleanArchitecture.Infrastructure.SignalR.Hubs;
    using CleanArchitecture.Infrastructure.SignalR.Interfaces;
    using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NToastNotify;
using Microsoft.AspNetCore.SignalR;
    using Microsoft.Extensions.DependencyInjection.Areas.Moderator.ActionFilters;

    namespace WebUI.Areas.Moderator.Controllers;

[Area("Moderator")]
[Authorize(Roles = "Moderator")]
public class GamesController : Controller
{
    private readonly IToastNotification _toastNotification;
    private readonly ISender _sender;
    private readonly IHubContext<GameHub , IGameHub> _hubContext;
    private readonly IIdentityService _identityService;
    private readonly ICurrentUserService _currentUserService;
    private readonly IGameRoundDataService _gameRoundData;


    public GamesController(IToastNotification toastNotification,
        ISender sender,
        IHubContext<GameHub , IGameHub> hubContext,
        IIdentityService identityService ,
        ICurrentUserService currentUserService, 
        IGameRoundDataService gameRoundData )
    {
        _toastNotification = toastNotification;
        _sender = sender;
        _hubContext = hubContext;
        _identityService = identityService;
        _currentUserService = currentUserService;
        _gameRoundData = gameRoundData;
    }



    [HttpGet("Moderator/Games/List")]
    public async Task<IActionResult> List(string moderatorId)
    {
        var games = await _sender.Send(new GetModeratorAssignedGames(_currentUserService.UserId));
        return View(games);
    }

    [ServiceFilter(typeof(CheckModeratorActionFilter))]
    [HttpGet("Moderator/Games/{code}/GamePage")]
    public async Task<IActionResult> GamePage(int code)
    {
        //check current user
        var game = await _sender.Send(new GetGameByCodeQuery(code , new List<Expression<Func<Game, object>>>(){x=>x.Questions}));
        var isGameRunning = _gameRoundData.IsListContainGameCode(code);

        if (isGameRunning) return View("ErrorPage" , "Game Is Running in other tab"); 

        return View(game);
    }
}
