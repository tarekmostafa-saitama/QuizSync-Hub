using CleanArchitecture.Application.Requests.Games.Queries;
using CleanArchitecture.Domain.Entities;
using System.Linq.Expressions;
using CleanArchitecture.Application.Common.Interfaces;
using CleanArchitecture.Application.Requests.Moderators.Commands;
using CleanArchitecture.Application.Requests.Moderators.Models;
using CleanArchitecture.Application.ViewModels;
using CleanArchitecture.Domain.Enums;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NToastNotify;
using Microsoft.AspNetCore.Identity;
using CleanArchitecture.Application.Requests.Games.Commands;
using CleanArchitecture.Application.Requests.Moderators.Queries;

namespace WebUI.Areas.Admin.Controllers;

[Authorize(Roles = "Admin")]
[Area("Admin")]
public class ModeratorsController : Controller
{


    private readonly IToastNotification _toastNotification;
    private readonly ISender _sender;
    private readonly IIdentityService _identityService;
    private readonly UserManager<ApplicationUser> _userManager;

    public ModeratorsController(IToastNotification toastNotification, ISender sender, IIdentityService identityService, UserManager<ApplicationUser> userManager)
    {
        _toastNotification = toastNotification;
        _sender = sender;
        _identityService = identityService;
        _userManager = userManager;
    }

    [HttpGet("Admin/Moderators/List")]
    public async Task<IActionResult> List()
    {
        var moderators =
            await _sender.Send(
                new GetModeratorsQuery());
       return View(moderators);
    }

    [HttpGet("Admin/Moderators/Create")]
    public  async Task<IActionResult> Create()
    {
        return PartialView(new CreateModeratorVm());
    }

    [HttpPost("Admin/Moderators/Create")]
    public async Task<IActionResult> Create(CreateModeratorVm model)
    {

        var res =   await  _sender.Send(new CreateModeratorCommand(model)); 
        return Json(res);
    }

    [HttpGet("Admin/Moderators/{userId}/ChangePasswordPartial")]
    public async Task<IActionResult> ChangePasswordPartial(string userId)
    {
        var alterPassword = new AlterUserPasswordViewModel() { UserId = userId };
        return PartialView("ChangePasswordPartial", alterPassword);
    }
    [HttpPost("Admin/Moderators/{userId}/ChangePassword")]
    public async Task<IActionResult> ChangePassword(string userId, AlterUserPasswordViewModel model)
    {
        var user = await _userManager.FindByIdAsync(userId);

        var token = await _userManager.GeneratePasswordResetTokenAsync(user);

        var result = await _userManager.ResetPasswordAsync(user, token, model.NewPassword);
        return result.Succeeded
            ? Json(new { success = true })
            : Json(new { success = false, message = result.Errors.Select(x => x.Description).First() });
    }
    [HttpGet("Admin/Moderators/{id}/Delete")]
    public async Task<IActionResult> Delete(string id)
    {
        var result = await _sender.Send(new DeleteModeratorCommand(id));
        return Json(result ? new { success = true, message = "Moderator Deleted Successfully" } : new { success = false, message = "Error While Deleting the moderator" });
    }



    [HttpGet("Admin/Moderators/{id}/AssignToGamePartial")]
    public async Task<IActionResult> AssignToGamePartial(string id)
    {
        var games = await _sender.Send(new GetGamesQuery());

        ViewBag.assignedGames = games.Where(x => x.ModeratorId == id).ToList();
        return PartialView(games);
    }

    [HttpPost("Admin/Moderators/{id}/AssignToGamePartial")]
    public async Task<IActionResult> AssignToGamePartial(string id, List<int> gameIds)
    {
        var model = await _sender.Send(new SetModeratorGameIdsCommand(id, gameIds));
        _toastNotification.AddSuccessToastMessage("Moderator Assigned Successfully");
        return RedirectToAction(nameof(List));
    }
}
