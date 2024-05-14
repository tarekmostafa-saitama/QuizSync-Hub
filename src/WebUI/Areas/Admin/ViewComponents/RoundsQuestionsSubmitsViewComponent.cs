using CleanArchitecture.Application.Requests.GameSubmissions.Query;
using CleanArchitecture.Application.Requests.Questions.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace WebUI.Areas.Admin.ViewComponents;
public class RoundsQuestionsSubmitsViewComponent : ViewComponent
{
    private readonly ISender _sender;
    public RoundsQuestionsSubmitsViewComponent(ISender sender)
    {
        _sender = sender;
    }
    // add explicit date filter
    public async Task<IViewComponentResult> InvokeAsync(int id, QuestionVm question)
    {
        var result = await _sender.Send(new GetGameRoundsQuestionsReportQuery() { GameId = id, Question = question });
        return await Task.FromResult((IViewComponentResult)View("Reports", result));


    }
}
