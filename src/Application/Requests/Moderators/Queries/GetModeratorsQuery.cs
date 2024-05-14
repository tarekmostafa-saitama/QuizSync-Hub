using System.Linq.Expressions;
using AutoMapper;
using CleanArchitecture.Application.Common.Interfaces;
using CleanArchitecture.Application.Requests.Games.Models;
using CleanArchitecture.Application.Requests.Games.Queries;
using CleanArchitecture.Domain.Entities;
using CleanArchitecture.Domain.Enums;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace CleanArchitecture.Application.Requests.Moderators.Queries;

public class GetModeratorsQuery:IRequest<List<ApplicationUser>>
{


}

public class GetModeratorsQueryHandler : IRequestHandler<GetModeratorsQuery, List<ApplicationUser>>
{
    private readonly IApplicationDbContext _dbContext;
    private readonly IMapper _mapper;
    private readonly UserManager<ApplicationUser> _userManager;

    public GetModeratorsQueryHandler(IApplicationDbContext dbContext, IMapper mapper, UserManager<ApplicationUser> userManager)
    {
        _dbContext = dbContext;
        _mapper = mapper;
        _userManager = userManager;
    }
    public async Task<List<ApplicationUser>> Handle(GetModeratorsQuery request, CancellationToken cancellationToken)
    {

        var users = await _userManager.GetUsersInRoleAsync(Role.Moderator.ToString());

        foreach (var user in users)
        {
            user.Games = await _dbContext.Games.Where(x => x.ModeratorId == user.Id).ToListAsync(cancellationToken);
        }
        return users.ToList();
    }
}
