using GerPros_Backend_API.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GerPros_Backend_API.Infrastructure.Data.Configurations;

public class FaqCategoryConfiguration : IEntityTypeConfiguration<FaqCategory>
{
    public void Configure(EntityTypeBuilder<FaqCategory> builder)
    {
        builder
            .Property(f => f.FaqItems)
            .HasColumnType("jsonb");
        
    }
}

