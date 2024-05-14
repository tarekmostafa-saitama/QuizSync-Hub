using CleanArchitecture.Domain.Entities;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using CleanArchitecture.Domain.SignalRModels;

namespace CleanArchitecture.Infrastructure.Persistence.Configurations;

public class PlayerConfiguration : IEntityTypeConfiguration<GameSubmission>
{
    public void Configure(EntityTypeBuilder<GameSubmission> builder)
    {
        ValueComparer<List<SubmittedQuestionAndAnswers>> comparer = new(
            (l, r) => JsonConvert.SerializeObject(l) == JsonConvert.SerializeObject(r),
            v => v == null ? 0 : JsonConvert.SerializeObject(v).GetHashCode(),
            v => JsonConvert.DeserializeObject<List<SubmittedQuestionAndAnswers>>(JsonConvert.SerializeObject(v)));

        var converter = new ValueConverter<List<SubmittedQuestionAndAnswers>, string>(
            v => JsonConvert.SerializeObject(v),
            v => string.IsNullOrWhiteSpace(v) ? new List<SubmittedQuestionAndAnswers>() : JsonConvert.DeserializeObject<List<SubmittedQuestionAndAnswers>>(v));


        builder
            .Property(e => e.SubmittedQuestionAndAnswersList)
            .HasColumnType("nvarchar(max)")
            .HasConversion(converter)
            .Metadata.SetValueComparer(comparer);

    }
}