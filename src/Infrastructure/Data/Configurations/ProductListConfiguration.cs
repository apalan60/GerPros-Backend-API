using GerPros_Backend_API.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GerPros_Backend_API.Infrastructure.Data.Configurations;

public class ProductListConfiguration : IEntityTypeConfiguration<ProductList>
{
    public void Configure(EntityTypeBuilder<ProductList> builder)
    {
    }
}
