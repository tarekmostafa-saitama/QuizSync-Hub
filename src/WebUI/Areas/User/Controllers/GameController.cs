using CleanArchitecture.Application.Requests.Games.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace WebUI.Areas.User.Controllers;


[Area("User")]
public class GameController : Controller
{
    private readonly ISender _sender;

    public GameController(ISender sender)
    {
        _sender = sender;
    }
    [HttpGet("{gameCode}/Users/{gameName}")]
    public async Task<IActionResult> Index(int gameCode)
    {
        var game = await _sender.Send(new GetGameByCodeQuery(gameCode)); 
        return View(game);
    }
}
