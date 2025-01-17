﻿using GerPros_Backend_API.Domain.Events;
using Microsoft.Extensions.Logging;

namespace GerPros_Backend_API.Application.Brands.EventHandlers;

public class BrandCompletedEventHandler : INotificationHandler<ProductItemCreatedEvent>
{
    private readonly ILogger<BrandCompletedEventHandler> _logger;

    public BrandCompletedEventHandler(ILogger<BrandCompletedEventHandler> logger)
    {
        _logger = logger;
    }

    public  Task Handle(ProductItemCreatedEvent notification, CancellationToken cancellationToken)
    {
        _logger.LogInformation("GerPros_Backend_API Domain Event: {DomainEvent}", notification.GetType().Name);

        return Task.CompletedTask;
    }
}
