using GerPros_Backend_API.Application.Common.Interfaces;
using GerPros_Backend_API.Application.Common.Models;
using GerPros_Backend_API.Domain.Enums;

namespace GerPros_Backend_API.Application.Products.Commands.UpdateProduct;

public record UpdateProductCommand : IRequest
{
    public Guid Id { get; set; }

    public Guid? SeriesId { get; init; }

    public string? Name { get; init; }

    public decimal? Price { get; init; }

    public UploadedFile? File { get; init; }

    public string? Detail { get; init; }
}

public class UpdateProductCommandHandler(IApplicationDbContext context, IFileStorageService fileStorageService)
    : IRequestHandler<UpdateProductCommand>
{
    public async Task Handle(UpdateProductCommand request, CancellationToken cancellationToken)
    {
        var entity = await context.ProductItems
            .FindAsync([request.Id], cancellationToken);
        Guard.Against.NotFound(request.Id, entity);
        
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

        entity.SeriesId = request.SeriesId ?? entity.SeriesId;
        entity.Name = request.Name ?? entity.Name;
        entity.Price = request.Price ?? entity.Price;
        entity.Image = imageUrl ?? entity.Image; 
        entity.Detail = request.Detail ?? entity.Detail;

        await context.SaveChangesAsync(cancellationToken);
    }
}
