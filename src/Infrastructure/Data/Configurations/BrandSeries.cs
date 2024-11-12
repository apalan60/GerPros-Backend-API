using GerPros_Backend_API.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GerPros_Backend_API.Infrastructure.Data.Configurations;

public class BrandSeriesConfiguration : IEntityTypeConfiguration<BrandSeries>
{
    public void Configure(EntityTypeBuilder<BrandSeries> builder)
    {
        builder
            .HasIndex(s => s.Name)
            .IsUnique();

        builder
            .HasOne(s => s.Brand)
            .WithMany(b => b.BrandSeries);
    }
}
