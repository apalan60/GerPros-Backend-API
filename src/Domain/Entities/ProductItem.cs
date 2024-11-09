namespace GerPros_Backend_API.Domain.Entities;

public class ProductItem : BaseAuditableEntity
{
    public Guid BrandId { get; set; }

    public Guid SeriesId { get; set; }

    public string? Name { get; set; }

    public decimal Price { get; set; }

    public string? Image { get; set; }

    public string? Detail { get; set; }

    public Brand Brand { get; init; } = null!;
    
    public Series Series { get; init; } = null!;
    
}
