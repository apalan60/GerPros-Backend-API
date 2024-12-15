using System.Text.Json;
using GerPros_Backend_API.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GerPros_Backend_API.Infrastructure.Data.Configurations;

public class FaqCategoryConfiguration : IEntityTypeConfiguration<FaqCategory>
{
    public void Configure(EntityTypeBuilder<FaqCategory> builder)
    {
        //fix: warn: Microsoft.EntityFrameworkCore.Model.Validation[10620]  The property 'FaqCategory.FaqItems' is a collection or enumeration type with a value converter but with no value comparer. Set a value comparer to ensure the collection/enumeration elements are compared correctly.
        var comparer = new ValueComparer<List<FaqItem>>(
            (c1, c2) => c1 != null && c2 != null && c1.SequenceEqual(c2),
            c => c.Aggregate(0, (a, v) => HashCode.Combine(a, v.GetHashCode())),
            c => c.ToList()
        );
        builder
            .Property(f => f.FaqItems)
            .HasColumnType("jsonb")
            .HasConversion(
                v => JsonSerializer.Serialize(v, null as JsonSerializerOptions),
                v => JsonSerializer.Deserialize<List<FaqItem>>(v, (JsonSerializerOptions)null!)!
            )
            .Metadata.SetValueComparer(comparer);
    }
}
