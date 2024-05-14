using CleanArchitecture.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace CleanArchitecture.Application.Common.Interfaces;

public interface IApplicationDbContext
{
   
    DbSet<Game> Games { get; set; }
    DbSet<Question> Questions { get; set; }
    DbSet<ApplicationUser> ApplicationUsers { get; set; }
    DbSet<GameSubmission> GameSubmissions { get; set; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}
