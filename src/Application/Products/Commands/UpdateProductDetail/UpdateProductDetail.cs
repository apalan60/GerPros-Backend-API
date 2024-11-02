using GerPros_Backend_API.Application.Common.Interfaces;
using GerPros_Backend_API.Application.TodoItems.Commands.UpdateTodoItemDetail;
using GerPros_Backend_API.Domain.Enums;

namespace GerPros_Backend_API.Application.Products.Commands.UpdateProductDetail;

public record UpdateProductDetailCommand : IRequest
{
    public int Id { get; init; }

    public int ListId { get; init; }

    public PriorityLevel Priority { get; init; }

    public string? Note { get; init; }
}

public class UpdateProductDetailCommandHandler : IRequestHandler<UpdateTodoItemDetailCommand>
{
    private readonly IApplicationDbContext _context;

    public UpdateProductDetailCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task Handle(UpdateTodoItemDetailCommand request, CancellationToken cancellationToken)
    {
        var entity = await _context.TodoItems
            .FindAsync(new object[] { request.Id }, cancellationToken);

        Guard.Against.NotFound(request.Id, entity);

        entity.ListId = request.ListId;
        entity.Priority = request.Priority;
        entity.Note = request.Note;

        await _context.SaveChangesAsync(cancellationToken);
    }
}
