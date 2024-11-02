namespace GerPros_Backend_API.Domain.Entities;

public class ProductList : BaseAuditableEntity
{
    public string? Title { get; set; }

    public Colour Colour { get; set; } = Colour.White;

    public IList<ProductItem> Items { get; private set; } = new List<ProductItem>();
}
