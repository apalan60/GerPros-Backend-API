using GerPros_Backend_API.Domain.Entities;

namespace GerPros_Backend_API.Application.Common.Interfaces;

public interface IApplicationDbContext
{
    DbSet<ProductItem> ProductItems { get; }
    DbSet<Brand> Brands { get; }
    DbSet<BrandSeries> BrandSeries { get; }
    DbSet<FaqCategory> FaqCategories { get; }
    DbSet<Tag> Tags { get; }
    DbSet<Post> Posts { get; }
    DbSet<PostTag> PostTags { get; }
    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}
