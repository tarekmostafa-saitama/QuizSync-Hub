using System.Linq.Expressions;
using CleanArchitecture.Application.Common.Interfaces;
using CleanArchitecture.Application.Common.Models;
using CleanArchitecture.Application.Requests.Games.Commands;
using CleanArchitecture.Application.Requests.Games.Models;
using CleanArchitecture.Application.Requests.Games.Queries;
using CleanArchitecture.Application.Requests.Moderators.Queries;
using CleanArchitecture.Application.Requests.Questions.Commands;
using CleanArchitecture.Application.Requests.Questions.Models;
using CleanArchitecture.Application.Requests.Questions.Queries;
using CleanArchitecture.Domain.Entities;
using CleanArchitecture.Domain.Enums;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NToastNotify;

namespace WebUI.Areas.Admin.Controllers;


[Authorize (Roles="Admin")]
[Area("Admin")] 
public class GamesController : Controller
{
    private readonly IToastNotification _toastNotification;
    private readonly ISender _sender;
    private readonly IIdentityService _identityService;

    public GamesController(IToastNotification toastNotification, ISender sender, IIdentityService identityService)
    {
        _toastNotification = toastNotification;
        _sender = sender;
        _identityService = identityService;
    }


    #region GamePart

    [HttpGet("Admin/Games/List")]
    public async Task<IActionResult> List()
    {
        var games = await _sender.Send(new GetGamesQuery(new List<Expression<Func<Game, object>>>() { x => x.Questions , y=>y.Moderator }));
        return View(games);
    }
    [HttpGet("Admin/Games/{id}/Data")]
    public async Task<IActionResult> Data(int id)
    {
        if (id == default)
            return View(new GameVm());
        var model = await _sender.Send(new GetGameQuery(id, new List<Expression<Func<Game, object>>>() { x => x.Questions }));
        return View(model);
    }
    [HttpPost("Admin/Games/{id}/BasicInfo")]
    public async Task<IActionResult> BasicInfo(int id, GameVm gameVm)
    {
        var newId = await _sender.Send(new SetGameInfoCommand(gameVm));
        _toastNotification.AddSuccessToastMessage("Game basic information saved successfully.");
        return RedirectToAction(nameof(Data), new { id = newId });
    }
    [HttpPost("Admin/Games/{id}/FormData")]
    public async Task<IActionResult> FormData(int id, string form)
    {
        var newId = await _sender.Send(new SetGameFormContentCommand(id, form));
        _toastNotification.AddSuccessToastMessage("Game form saved successfully.");
        return Json(true);
    }

    [HttpPost("Admin/Games/{id}/Theme")]
    public async Task<IActionResult> Theme(int id, ThemeConfigurationVm model)
    {
        await _sender.Send(new SetGameThemeConfigurationCommand(id, model));
        _toastNotification.AddSuccessToastMessage("Game theme saved successfully.");
        return Json(true);
    }

    [HttpGet("Admin/Games/{id}/Delete")]
    public async Task<IActionResult> Delete(int id)
    {
        var result = await _sender.Send(new DeleteGameCommand(id));
        return Json(result ? new { success = true, message = "Game Deleted Successfully" } : new { success = false, message = "Error While Deleting the game" });
    }
    #endregion


    #region QuestionsPArt
    [HttpGet("Admin/Games/{id}/{questionId}/SetQues")]
    public async Task<IActionResult> SetQuestion(int id, int questionId)
    {
        if (questionId == default)
            return PartialView(new QuestionVm());
        var model = await _sender.Send(new GetQuestionQuery(questionId));
        return PartialView(model);
    }

    [HttpPost("Admin/Games/{id}/{questionId}/SetQues")]
    public async Task<IActionResult> SetQuestion(int id, QuestionVm ques, IFormCollection col)
    {

        await _sender.Send(new SetQuestionCommand(id, ques));
        TempData["Success"] = "Question saved successfully.";
        return Json(true);
    }

    [HttpGet("Admin/Games/{questionId}/deleteQues")]
    public async Task<IActionResult> DeleteQuestion(int questionId)
    {
        var result = await _sender.Send(new DeleteQuestionCommand(questionId ));
        return Json(result ? new { success = true, message = "Question Deleted Successfully" } : new { success = false, message = "Error While Deleting the question" });
    }

    [HttpPost("Admin/Games/{gameId}/RearrangeQuestions")]

    public async Task<IActionResult> RearrangeQuestions(int gameId,[FromBody] List<RearrangeQuestionsVm> model)
    {
        var result = await _sender.Send(new RearrangeQuestionCommand(model, gameId));

        return Json(result);

    }
    #endregion



    #region ModeratorPart


    [HttpGet("Admin/Games/{id}/AssignToModeratorPartial")]
    public async Task<IActionResult> AssignToModeratorPartial(int id)
    {
        var game =await _sender.Send(new GetGameQuery(id));
        var moderators = await _sender.Send(new GetModeratorsQuery());

        if (game.ModeratorId!=null)
        {
            moderators = moderators.Where(x => x.Id != game.ModeratorId).ToList(); 
        }
        ViewBag.Assigned= game.ModeratorId != null;
        return PartialView(moderators);
    }
    [HttpPost("Admin/Games/{id}/AssignToModeratorPartial")]
    public async Task<IActionResult> AssignToModeratorPartial(int id, string moderatorId)
    {
        var model = await _sender.Send(new AssignModeratorCommand(id, moderatorId));
        _toastNotification.AddSuccessToastMessage("Moderator Assigned Successfully");
        return RedirectToAction(nameof(List));
    }
    #endregion



}
