using GerPros_Backend_API.Domain.Entities;

namespace GerPros_Backend_API.Application.Series.Queries;

public class BrandSeriesDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = null!;
    public Guid BrandId { get; set; }
}


public static class BrandSeriesMapping
{
    public static BrandSeriesDto ToDto(this BrandSeries entity)
    {
        return new BrandSeriesDto
        {
            Id = entity.Id,
            Name = entity.Name,
            BrandId = entity.BrandId
        };
    }
}
