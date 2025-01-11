using System.Reflection;
using GerPros_Backend_API.Application.Common.Interfaces;
using GerPros_Backend_API.Domain.Entities;
using GerPros_Backend_API.Infrastructure.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace GerPros_Backend_API.Infrastructure.Data;

public class ApplicationDbContext : IdentityDbContext<ApplicationUser>, IApplicationDbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

    public DbSet<ProductItem> ProductItems => Set<ProductItem>();
    
    public DbSet<Brand> Brands => Set<Brand>();
    
    public DbSet<BrandSeries> BrandSeries => Set<BrandSeries>();

    public DbSet<FaqCategory> FaqCategories => Set<FaqCategory>();

    public DbSet<Tag> Tags => Set<Tag>();

    public DbSet<Post> Posts => Set<Post>();
    
    public DbSet<PostTag> PostTags => Set<PostTag>();
    public async Task<bool> GetHealthStatus() => await Database.CanConnectAsync();
    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }
}
