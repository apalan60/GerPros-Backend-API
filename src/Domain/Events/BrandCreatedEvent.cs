namespace GerPros_Backend_API.Domain.Events;

public class BrandCreatedEvent : BaseEvent
{
    public BrandCreatedEvent(Brand item)
    {
        Item = item;
    }

    public Brand Item { get; }
}
