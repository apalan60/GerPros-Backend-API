using GerPros_Backend_API.Domain.Entities;

namespace GerPros_Backend_API.Application.Products.Queries.GetProductsWithPagination;

public class ProductItemDto
{
    public Guid Id { get; init; }
    
    public string? Name { get; init; }

    public decimal Price { get; init; }

    public string? Image { get; init; }

    public string? Detail { get; init; }

    public Brand Brand { get; init; } = null!;
    
    public BrandSeries Series { get; init; } = null!;
}

public static class ProductItemDtoMapping
{
    public static ProductItemDto ToDto(this ProductItem entity)
    {
        return new ProductItemDto
        {
            Id = entity.Id,
            Name = entity.Name,
            Price = entity.Price,
            Image = entity.Image,
            Detail = entity.Detail,
            Series = entity.BrandSeries
        };
    }
}
