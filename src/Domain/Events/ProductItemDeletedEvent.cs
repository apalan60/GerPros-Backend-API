namespace GerPros_Backend_API.Domain.Events;

public class ProductItemDeletedEvent : BaseEvent
{
    public ProductItemDeletedEvent(ProductItem item)
    {
        Item = item;
    }

    public ProductItem Item { get; }
}
