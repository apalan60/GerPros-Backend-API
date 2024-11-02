namespace GerPros_Backend_API.Domain.Events;

public class ProductItemCreatedEvent : BaseEvent
{
    public ProductItemCreatedEvent(ProductItem item)
    {
        Item = item;
    }

    public ProductItem Item { get; }
}
