using CleanArchitecture.Domain.Entities;
using CleanArchitecture.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Newtonsoft.Json;

namespace CleanArchitecture.Infrastructure.Persistence.Configurations;

public class QuestionConfiguration :IEntityTypeConfiguration<Question>
{
    public void Configure(EntityTypeBuilder<Question> builder)
    {
        ValueComparer<List<Choice>> comparer = new(
            (l, r) => JsonConvert.SerializeObject(l) == JsonConvert.SerializeObject(r),
            v => v == null ? 0 : JsonConvert.SerializeObject(v).GetHashCode(),
            v => JsonConvert.DeserializeObject<List<Choice>>(JsonConvert.SerializeObject(v)));

        var converter = new ValueConverter<List<Choice>, string>(
            v => JsonConvert.SerializeObject(v),
            v => string.IsNullOrWhiteSpace(v) ? new List<Choice>() : JsonConvert.DeserializeObject<List<Choice>>(v));


        builder
            .Property(e => e.Choices)
            .HasColumnType("nvarchar(max)")
            .HasConversion(converter)
            .Metadata.SetValueComparer(comparer);

    }
}