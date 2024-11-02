using GerPros_Backend_API.Application.Common.Interfaces;
using GerPros_Backend_API.Application.TodoItems.Commands.CreateTodoItem;
using GerPros_Backend_API.Domain.Entities;
using GerPros_Backend_API.Domain.Events;

namespace GerPros_Backend_API.Application.Products.Commands.CreateProduct;

public record CreateProductsCommand : IRequest<Guid>
{
    public int ListId { get; init; }

    public string? Title { get; init; }
}

public class CreateTodoItemCommandHandler : IRequestHandler<CreateTodoItemCommand, int>
{
    private readonly IApplicationDbContext _context;

    public CreateTodoItemCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<int> Handle(CreateTodoItemCommand request, CancellationToken cancellationToken)
    {
        var entity = new TodoItem
        {
            ListId = request.ListId,
            Title = request.Title,
            Done = false
        };

        entity.AddDomainEvent(new TodoItemCreatedEvent(entity));

        _context.TodoItems.Add(entity);

        await _context.SaveChangesAsync(cancellationToken);

        return entity.Id;
    }
}
