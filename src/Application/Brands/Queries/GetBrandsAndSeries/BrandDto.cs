using GerPros_Backend_API.Application.Series.Queries;
using GerPros_Backend_API.Domain.Entities;

namespace GerPros_Backend_API.Application.Brands.Queries.GetBrandsAndSeries;

public class BrandDto
{
    public Guid Id { get; init; }
    
    public string Name { get; init; } = null!;

    public IEnumerable<BrandSeriesDto>? Series { get; init; }
}

public static class BrandDtoMapping 
{
    public static BrandDto ToDto(this Brand entity)
    {
        return new BrandDto
        {
            Id = entity.Id,
            Name = entity.Name,
            Series = entity.BrandSeries.Select(s => s.ToDto())
        };
    }
}
