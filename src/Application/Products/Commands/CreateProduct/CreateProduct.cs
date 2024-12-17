using GerPros_Backend_API.Application.Common.Interfaces;
using GerPros_Backend_API.Application.Common.Models;
using GerPros_Backend_API.Domain.Entities;
using GerPros_Backend_API.Domain.Enums;
using GerPros_Backend_API.Domain.Events;

namespace GerPros_Backend_API.Application.Products.Commands.CreateProduct;

public record CreateProductItemCommand : IRequest<Guid>
{
    public Guid SeriesId { get; init; }

    public string Name { get; init; } = null!;

    public decimal Price { get; init; }

    public UploadedFile? File { get; init; }

    public string? Detail { get; init; }
}

public class CreateProductItemCommandHandler(IApplicationDbContext context, IFileStorageService fileStorageService)
    : IRequestHandler<CreateProductItemCommand, Guid>
{
    public async Task<Guid> Handle(CreateProductItemCommand request, CancellationToken cancellationToken)
    {
        var series = await context.BrandSeries
            .Where(s => s.Id == request.SeriesId)
            .FirstOrDefaultAsync(cancellationToken);
        Guard.Against.NotFound(request.SeriesId, series);

        string? imageUrl = null;
        if (request.File?.Content is { Length: > 0 })
        {
            imageUrl = await fileStorageService.UploadAsync(
                request.File.Content,
                request.File.FileName ?? "unknown",
                request.File.ContentType ?? "application/octet-stream",
                FileCategory.Product,
                cancellationToken
            );
        }

        var entity = new ProductItem
        {
            Id = Guid.NewGuid(), 
            SeriesId = request.SeriesId,
            Name = request.Name,
            Price = request.Price,
            Image = imageUrl,
            Detail = request.Detail
        };

        entity.AddDomainEvent(new ProductItemCreatedEvent(entity));

        context.ProductItems.Add(entity);

        await context.SaveChangesAsync(cancellationToken);

        return entity.Id;
    }
}
