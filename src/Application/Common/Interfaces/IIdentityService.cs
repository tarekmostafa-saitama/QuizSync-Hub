using CleanArchitecture.Application.Common.Models;
using CleanArchitecture.Domain.Entities;

namespace CleanArchitecture.Application.Common.Interfaces;

public interface IIdentityService
{
    Task<List<ApplicationUser>> GetUsersInRoleAsync(string roleName);

    Task<string> GetUserNameAsync(string userId);
    Task<ApplicationUser> GetCurrentUserAsync();

    Task<bool> IsInRoleAsync(string userId, string role);

    Task<bool> AuthorizeAsync(string userId, string policyName);

    Task<(Result Result, string UserId)> CreateUserAsync(string userName, string password);

    Task<Result> DeleteUserAsync(string userId);
}
