using GerPros_Backend_API.Application.Common.Interfaces;
using GerPros_Backend_API.Application.Products.Queries.GetProductsWithPagination;
using GerPros_Backend_API.Domain.Enums;

namespace GerPros_Backend_API.Application.Products.Queries.GetProductItem;

public record GetProductItemDetail(Guid Id) : IRequest<ProductItemDto>;

public class GetProductItemDetailQueryHandler(IApplicationDbContext context, IFileStorageService fileStorageService)
    : IRequestHandler<GetProductItemDetail, ProductItemDto>
{
    public async Task<ProductItemDto> Handle(GetProductItemDetail request, CancellationToken cancellationToken)
    {
        var entity = await context.ProductItems
            .Include(x => x.BrandSeries)
            .ThenInclude(x => x.Brand)
            .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);
        
        if (entity == null) 
            Guard.Against.NotFound(request.Id, entity);

        if (entity.Image != null)
        {
            entity.Image = await fileStorageService.GetUrlAsync(entity.Image, FileCategory.Product);
        }

        return entity.ToDto();
    }
}
