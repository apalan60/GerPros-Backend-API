using GerPros_Backend_API.Application.Common.Interfaces;
using GerPros_Backend_API.Domain.Entities;
using GerPros_Backend_API.Domain.Events;

namespace GerPros_Backend_API.Application.Products.Commands.CreateProduct;

public record CreateProductItemCommand : IRequest<Guid>
{
    public Guid SeriesId { get; init; }

    public string Name { get; init; } = null!;

    public decimal Price { get; init; }

    public string? Image { get; init; }

    public string? Detail { get; init; }
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
        var series = await _context.BrandSeries
            .Where(s => s.Id == request.SeriesId )
            .FirstOrDefaultAsync(cancellationToken);
        Guard.Against.NotFound(request.SeriesId, series);
        
        var entity = new ProductItem
        {
            Id = new Guid(),
            SeriesId = request.SeriesId,
            Name = request.Name,
            Price = request.Price,
            Image = request.Image,
            Detail = request.Detail
        };
        
        entity.AddDomainEvent(new ProductItemCreatedEvent(entity));

        _context.ProductItems.Add(entity);

        await _context.SaveChangesAsync(cancellationToken);

        return entity.Id;
    }
}
