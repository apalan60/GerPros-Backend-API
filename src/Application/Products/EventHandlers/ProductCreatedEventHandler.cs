using GerPros_Backend_API.Domain.Events;
using Microsoft.Extensions.Logging;

namespace GerPros_Backend_API.Application.Products.EventHandlers;

public class ProductCreatedEventHandler : INotificationHandler<ProductItemCreatedEvent>
{
    private readonly ILogger<ProductCreatedEventHandler> _logger;

    public ProductCreatedEventHandler(ILogger<ProductCreatedEventHandler> logger)
    {
        _logger = logger;
    }

    public Task Handle(ProductItemCreatedEvent notification, CancellationToken cancellationToken)
    {
        _logger.LogInformation("GerPros_Backend_API Domain Event: {DomainEvent}", notification.GetType().Name);

        return Task.CompletedTask;
    }
}
