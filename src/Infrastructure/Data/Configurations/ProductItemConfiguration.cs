using GerPros_Backend_API.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GerPros_Backend_API.Infrastructure.Data.Configurations;

public class ProductItemConfiguration : IEntityTypeConfiguration<ProductItem>
{
    public void Configure(EntityTypeBuilder<ProductItem> builder)
    {
        builder.Property(p => p.Price)
            .HasPrecision(18, 2);
        
        builder.HasOne(p => p.BrandSeries)
            .WithMany(b => b.ProductItems)
            .HasForeignKey(p => p.SeriesId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
