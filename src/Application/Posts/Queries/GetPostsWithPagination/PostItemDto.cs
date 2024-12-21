using GerPros_Backend_API.Domain.Entities;

namespace GerPros_Backend_API.Application.Posts.Queries.GetPostsWithPagination;

public class ProductItemDto
{
    public Guid Id { get; init; }
    
    public string? Name { get; init; }

    public decimal Price { get; init; }

    public string? Image { get; set; }

    public string? Detail { get; init; }

    public required Guid BrandId { get; set; }
    
    public required string BrandName { get; set; }
    
    public required Guid SeriesId { get; set; }
    public required string SeriesName { get; set; }
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
            BrandId = entity.BrandSeries.BrandId,
            BrandName = entity.BrandSeries.Brand.Name,
            SeriesId = entity.SeriesId,
            SeriesName = entity.BrandSeries.Name
        };
    }
}
