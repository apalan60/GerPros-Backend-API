using GerPros_Backend_API.Application.TodoItems.EventHandlers;
using GerPros_Backend_API.Domain.Events;
using Microsoft.Extensions.Logging;

namespace GerPros_Backend_API.Application.Products.EventHandlers;

public class ProductCompletedEventHandler : INotificationHandler<TodoItemCompletedEvent>
{
    private readonly ILogger<TodoItemCompletedEventHandler> _logger;

    public ProductCompletedEventHandler(ILogger<TodoItemCompletedEventHandler> logger)
    {
        _logger = logger;
    }

    public Task Handle(TodoItemCompletedEvent notification, CancellationToken cancellationToken)
    {
        _logger.LogInformation("GerPros_Backend_API Domain Event: {DomainEvent}", notification.GetType().Name);

        return Task.CompletedTask;
    }
}
