﻿using GerPros_Backend_API.Domain.Entities;

namespace GerPros_Backend_API.Application.Common.Interfaces;

public interface IApplicationDbContext
{
    DbSet<TodoList> TodoLists { get; }
    DbSet<TodoItem> TodoItems { get; }
    DbSet<ProductItem> ProductItems { get; }
    DbSet<Brand> Brands { get; }
    DbSet<BrandSeries> BrandSeries { get; }
    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}
