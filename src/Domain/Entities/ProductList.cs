namespace GerPros_Backend_API.Domain.Entities;

public class ProductList : BaseAuditableEntity
{
    public IList<ProductItem> Items { get; private set; } = new List<ProductItem>();
}
