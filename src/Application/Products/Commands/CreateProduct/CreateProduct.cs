using GerPros_Backend_API.Application.Common.Interfaces;
using GerPros_Backend_API.Domain.Entities;
using GerPros_Backend_API.Domain.Events;

namespace GerPros_Backend_API.Application.Products.Commands.CreateProduct;

public record CreateProductItemCommand : IRequest<Guid>
{
    
}

public class CreateProductItemCommandHandler : IRequestHandler<CreateProductItemCommand, Guid>
{
    private readonly IApplicationDbContext _context;

    public CreateProductItemCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Guid> Handle(CreateProductItemCommand request, CancellationToken cancellationToken)
    {
        var entity = new ProductItem { };
        
        entity.AddDomainEvent(new ProductItemCreatedEvent(entity));

        _context.ProductItems.Add(entity);

        await _context.SaveChangesAsync(cancellationToken);

        return entity.Id;
    }
}
