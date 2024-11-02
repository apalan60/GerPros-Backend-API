namespace GerPros_Backend_API.Domain.Entities;

public class ProductItem : BaseAuditableEntity
{
    public Guid ListId { get; init; }

    public string? Brand { get; init; }

    public string? Series { get; init; }

    public string? Name { get; init; }

    public decimal Price { get; init; }

    public string? Image { get; init; }

    public string? Detail { get; init; }

    private bool _done;
    public bool Done
    {
        get => _done;
        set
        {
            if (value && !_done)
            {
                AddDomainEvent(new ProductItemCompletedEvent(this));
            }

            _done = value;
        }
    }

    public ProductList List { get; set; } = null!;
}
