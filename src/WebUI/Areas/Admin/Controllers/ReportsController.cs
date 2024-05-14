using CleanArchitecture.Application.Common.Interfaces;
using CleanArchitecture.Application.Requests.GameSubmissions.Commands;
using CleanArchitecture.Application.Requests.GameSubmissions.Query;
using CleanArchitecture.Application.ViewModels;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NToastNotify;

namespace WebUI.Areas.Admin.Controllers;

[Authorize(Roles = "Admin")]
[Area("Admin")]
public class ReportsController : Controller
{
    private readonly IToastNotification _toastNotification;
    private readonly ISender _sender;
    private readonly IIdentityService _identityService;

    public ReportsController(IToastNotification toastNotification, ISender sender, IIdentityService identityService)
    {
        _toastNotification = toastNotification;
        _sender = sender;
        _identityService = identityService;
    }

    [HttpGet("Admin/Game/{gameId}/Reports")]
    public async Task<IActionResult> Reports(int gameId)
    {
        var data = await _sender.Send(new GetGameSubmissionsQuery(gameId) );
        var players = await _sender.Send(new GetRoundsPlayersReportQuery(gameId)); 
        GameReportsVm gameReportsVm = new GameReportsVm()
        {
            GameSubmissionVms = players , 
            GameRoundDataVm = data,
        };
        return View(gameReportsVm);
    }


    [HttpGet("Admin/Game/{gameId}/{identifier}/RoundDetails")]
    public async Task<IActionResult> RoundDetails(int gameId , int identifier)
    {
        var details =await  _sender.Send(new GetGameRoundDataQuery(gameId, identifier));
        return View(details);
    }

    [HttpGet("Admin/Game/{playerId}/PlayerDetails")]
    public async Task<IActionResult> PlayerDetailsPartial(int playerId)
    {
        var details = await _sender.Send(new GetPlayerSubmitDataQuery(playerId));
        return PartialView(details);
    }
    [HttpGet("Admin/Game/{identifier}/DeleteRound")]
    public async Task<IActionResult> DeleteRound(int identifier)
    {
        var result = await _sender.Send(new DeleteGameRoundCommand(identifier));
        return Json(result ? new { success = true, message = "Round deleted successfully." } : new { success = false, message = "Error while deleting ..." });
    }

    [HttpGet("Admin/Game/{id}/DeleteGameSubmit")]
    public async Task<IActionResult> DeleteGameSubmit(int id)
    {
        var result = await _sender.Send(new DeleteGameSubmitCommand(id));
        return Json(result ? new { success = true, message = "Submit deleted successfully." } : new { success = false, message = "Error while deleting ..." });
    }
}
