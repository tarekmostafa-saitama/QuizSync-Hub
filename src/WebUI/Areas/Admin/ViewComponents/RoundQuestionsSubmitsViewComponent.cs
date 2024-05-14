using CleanArchitecture.Application.Requests.GameSubmissions.Query;
using CleanArchitecture.Application.Requests.Questions.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace WebUI.Areas.Admin.ViewComponents;
public class RoundQuestionsSubmitsViewComponent : ViewComponent
{
    private readonly ISender _sender;
    public RoundQuestionsSubmitsViewComponent(ISender sender)
    {
        _sender = sender;
    }
    // add explicit date filter
    public async Task<IViewComponentResult> InvokeAsync(int identifier, QuestionVm question)
    {
        var result = await _sender.Send(new GetGameRoundQuestionsReportQuery() { Identifier = identifier, Question = question});
        return await Task.FromResult((IViewComponentResult)View("Reports", result));


    }
}
