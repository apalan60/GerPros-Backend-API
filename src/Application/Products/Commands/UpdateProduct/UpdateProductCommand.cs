using GerPros_Backend_API.Application.Common.Interfaces;

namespace GerPros_Backend_API.Application.Products.Commands.UpdateProduct;

public record UpdateProductCommand : IRequest
{
    public Guid Id { get; set; }
    
    public Guid? SeriesId { get; init; }
    
    public string? Name { get; init; }

    public decimal? Price { get; init; }

    public string? Image { get; init; }

    public string? Detail { get; init; }
}

public class UpdateProductCommandHandler : IRequestHandler<UpdateProductCommand>
{
    private readonly IApplicationDbContext _context;

    public UpdateProductCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task Handle(UpdateProductCommand request, CancellationToken cancellationToken)
    {
        var entity = await _context.ProductItems
            .FindAsync([request.Id], cancellationToken);
        Guard.Against.NotFound(request.Id, entity);

        entity.SeriesId = request.SeriesId ?? entity.SeriesId;
        entity.Name = request.Name ?? entity.Name;
        entity.Price = request.Price ?? entity.Price;
        entity.Image = request.Image ?? entity.Image;
        entity.Detail = request.Detail ?? entity.Detail;

        await _context.SaveChangesAsync(cancellationToken);
    }
}
