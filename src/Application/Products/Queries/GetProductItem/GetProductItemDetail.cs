using GerPros_Backend_API.Application.Common.Interfaces;
using GerPros_Backend_API.Application.Products.Queries.GetProductsWithPagination;

namespace GerPros_Backend_API.Application.Products.Queries.GetProductItem;

public record GetProductItemDetail(Guid Id) : IRequest<ProductItemDto>;

public class GetProductItemDetailQueryHandler(IApplicationDbContext context)
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
        
        return entity.ToDto();
    }
}
