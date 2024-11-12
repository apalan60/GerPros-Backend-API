namespace GerPros_Backend_API.Domain.Entities;

public class ProductItem : BaseAuditableEntity
{
    /// <summary>
    /// foreign key
    /// </summary>
    public Guid SeriesId { get; set; }

    public string? Name { get; set; }

    public decimal Price { get; set; }

    public string? Image { get; set; }

    public string? Detail { get; set; }
    
    public BrandSeries BrandSeries { get; init; } = null!;
    
}
