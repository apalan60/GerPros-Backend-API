using GerPros_Backend_API.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GerPros_Backend_API.Infrastructure.Data.Configurations;

public class BrandConfiguration : IEntityTypeConfiguration<Brand>
{
    public void Configure(EntityTypeBuilder<Brand> builder)
    {
        builder
            .HasIndex(b => b.Name)
            .IsUnique();

        builder
            .HasMany(b => b.BrandSeries);
    }
}
