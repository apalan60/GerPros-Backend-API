namespace GerPros_Backend_API.Domain.Events;

public class BrandDeletedEvent : BaseEvent
{
    public BrandDeletedEvent(Brand brand)
    {
        Brand = brand;
    }

    public Brand Brand { get; }
}
