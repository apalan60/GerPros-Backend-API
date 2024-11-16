using GerPros_Backend_API.Application.Common.Interfaces;
using GerPros_Backend_API.Domain.Entities;
using GerPros_Backend_API.Domain.Events;

namespace GerPros_Backend_API.Application.Products.Commands.CreateProduct;

public record CreateProductItemsCommand : IRequest<List<Guid>>
{
    public List<CreateProductItemDto> Items { get; init; } = [];
}

public record CreateProductItemDto
{
    public Guid SeriesId { get; init; }
    public string Name { get; init; } = null!;
    public decimal Price { get; init; }
    public string? Image { get; init; }
    public string? Detail { get; init; }
}

public class CreateProductItemsCommandHandler : IRequestHandler<CreateProductItemsCommand, List<Guid>>
{
    private readonly IApplicationDbContext _context;

    public CreateProductItemsCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<List<Guid>> Handle(CreateProductItemsCommand request, CancellationToken cancellationToken)
    {
        var productIds = new List<Guid>();

        foreach (var item in request.Items)
        {
            var series = await _context.BrandSeries
                .Where(s => s.Id == item.SeriesId)
                .FirstOrDefaultAsync(cancellationToken);
            Guard.Against.NotFound(item.SeriesId, series);

            var entity = new ProductItem
            {
                Id = Guid.NewGuid(),
                SeriesId = item.SeriesId,
                Name = item.Name,
                Price = item.Price,
                Image = item.Image,
                Detail = item.Detail
            };

            entity.AddDomainEvent(new ProductItemCreatedEvent(entity));

            _context.ProductItems.Add(entity);
            productIds.Add(entity.Id);
        }

        await _context.SaveChangesAsync(cancellationToken);

        return productIds;
    }
}

