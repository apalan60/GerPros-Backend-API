using GerPros_Backend_API.Domain.Entities;

namespace GerPros_Backend_API.Application.Common.Interfaces;

public interface IApplicationDbContext
{
    DbSet<TodoList> TodoLists { get; }

    DbSet<TodoItem> TodoItems { get; }
    DbSet<ProductList> ProductLists { get; }
    DbSet<ProductItem> ProductItems { get; }
    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}
