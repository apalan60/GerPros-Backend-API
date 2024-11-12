namespace GerPros_Backend_API.Domain.Entities;

public class Brand : BaseAuditableEntity
{
    /// <summary>
    /// unique, non-clustering index
    /// </summary>
    public string Name { get; set; } = null!;
    
    public ICollection<BrandSeries> BrandSeries { get; init; } = new List<BrandSeries>();
}
