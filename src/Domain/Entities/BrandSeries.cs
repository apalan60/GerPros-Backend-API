namespace GerPros_Backend_API.Domain.Entities;

public class BrandSeries : BaseAuditableEntity
{
    /// <summary>
    /// unique, non-clustering index
    /// </summary>
    public string Name { get; set; } = null!;

    /// <summary>
    /// foreign key
    /// </summary>
    public Guid BrandId { get; set; }
    
    public virtual Brand Brand { get; init; } = null!;
    
    public virtual ICollection<ProductItem> ProductItems { get; init; } = new List<ProductItem>();
}
