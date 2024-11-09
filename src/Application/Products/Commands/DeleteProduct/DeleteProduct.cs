using GerPros_Backend_API.Application.Common.Interfaces;
using GerPros_Backend_API.Domain.Events;

namespace GerPros_Backend_API.Application.Products.Commands.DeleteProduct;

public record DeleteProductCommand(Guid Id) : IRequest;

public class DeleteProductCommandHandler : IRequestHandler<DeleteProductCommand>
{
    private readonly IApplicationDbContext _context;

    public DeleteProductCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task Handle(DeleteProductCommand request, CancellationToken cancellationToken)
    {
        var entity = await _context.ProductItems
            .FindAsync([request.Id], cancellationToken);

        Guard.Against.NotFound(request.Id, entity);

        _context.ProductItems.Remove(entity);

        entity.AddDomainEvent(new ProductItemDeletedEvent(entity));

        await _context.SaveChangesAsync(cancellationToken);
    }
}
