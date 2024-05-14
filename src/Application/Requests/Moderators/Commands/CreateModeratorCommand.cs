using AutoMapper;
using CleanArchitecture.Application.Common.Helpers;
using CleanArchitecture.Application.Common.Interfaces;
using CleanArchitecture.Application.Common.Models;
using CleanArchitecture.Application.Requests.Moderators.Models;
using CleanArchitecture.Domain.Entities;
using CleanArchitecture.Domain.Enums;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace CleanArchitecture.Application.Requests.Moderators.Commands;

public class CreateModeratorCommand:IRequest<Result>
{
    public CreateModeratorCommand(CreateModeratorVm createModerator)
    {
        CreateModeratorVm = createModerator;
    }

    public CreateModeratorVm CreateModeratorVm { get; set; }
}

public class CreateModeratorCommandHandler : IRequestHandler<CreateModeratorCommand, Result>
{
   
    private readonly UserManager<ApplicationUser> _userManager;
    public CreateModeratorCommandHandler(UserManager<ApplicationUser> userManager)
    {
        _userManager = userManager;
    }
    public async Task<Result> Handle(CreateModeratorCommand request, CancellationToken cancellationToken)
    {
        var applicationUser = new ApplicationUser()
        {
            Email = request.CreateModeratorVm.Email,
            UserName = request.CreateModeratorVm.Email,
            FullName = request.CreateModeratorVm.FullName
        };
            var addUseResult= await _userManager.CreateAsync(applicationUser, request.CreateModeratorVm.Password);


        if(addUseResult.Succeeded)
        {
            await _userManager.AddToRoleAsync(applicationUser, Role.Moderator.ToString());
         
        }
        
        return addUseResult.ToApplicationResult();

    }
}