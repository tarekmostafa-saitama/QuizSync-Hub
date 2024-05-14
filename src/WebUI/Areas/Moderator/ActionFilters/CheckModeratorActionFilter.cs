using CleanArchitecture.Application.Common.Interfaces;
using CleanArchitecture.Application.Requests.Moderators.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Microsoft.Extensions.DependencyInjection.Areas.Moderator.ActionFilters;

public class CheckModeratorActionFilter: IAsyncActionFilter
{
    private readonly ISender _sender;
    private readonly ICurrentUserService _currentUserService;

    public CheckModeratorActionFilter(ISender sender , ICurrentUserService currentUserService)
    {
        _sender = sender;
        _currentUserService = currentUserService;
    }
public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
{
    // execute any code before the action executes
    var loggedInUser = _currentUserService.UserId; 
    var code =int.Parse(context.HttpContext.Request.RouteValues["code"]?.ToString() ?? string.Empty);

    var hasAuth = await _sender.Send(new IsModeratorHasAccessToGame(code, loggedInUser));
    if (hasAuth == false)
    {
        context.Result = new BadRequestObjectResult("You don't have the access for this game .");
        return;
    }
    var result = await next();
    // execute any code after the action executes
}
}