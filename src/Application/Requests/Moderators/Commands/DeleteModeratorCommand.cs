using CleanArchitecture.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace CleanArchitecture.Application.Requests.Moderators.Commands;

public class DeleteModeratorCommand : IRequest<bool>
{
    public string Id { get; set; }

    public DeleteModeratorCommand(string id )
    {
        Id = id; 
    }
}
public class DeleteModeratorCommandHandler : IRequestHandler<DeleteModeratorCommand, bool>
{
    private readonly UserManager<ApplicationUser> _userManager;

    public DeleteModeratorCommandHandler(UserManager<ApplicationUser> userManager)
    {
        _userManager = userManager;
    }
    public async Task<bool> Handle(DeleteModeratorCommand request, CancellationToken cancellationToken)
    {
        var user = await _userManager.FindByIdAsync(request.Id);
        if (user == null) return false;
        await _userManager.DeleteAsync(user);
        return true;
    }
}