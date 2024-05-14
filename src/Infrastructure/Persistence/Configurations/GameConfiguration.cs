using System.Reflection.Emit;
using CleanArchitecture.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.Extensions.Hosting;

namespace CleanArchitecture.Infrastructure.Persistence.Configurations;

public class GameConfiguration : IEntityTypeConfiguration<Game>
{
    public void Configure(EntityTypeBuilder<Game> builder)
    {
        builder.OwnsOne(p => p.ThemeConfiguration);

         builder
        .HasOne(s => s.Moderator)
        .WithMany(ad => ad.Games)
        .HasForeignKey(ad => ad.ModeratorId)
        .OnDelete(DeleteBehavior.SetNull);
    }
}