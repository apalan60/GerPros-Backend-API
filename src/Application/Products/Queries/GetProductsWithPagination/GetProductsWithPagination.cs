using GerPros_Backend_API.Application.Common.Interfaces;
using GerPros_Backend_API.Application.Common.Mappings;
using GerPros_Backend_API.Application.Common.Models;

namespace GerPros_Backend_API.Application.Products.Queries.GetProductsWithPagination;


public record GetProductWithPaginationQuery : IRequest<PaginatedList<ProductItemDto>>
{
    public Guid? BrandId { get; init; }
    public Guid? SeriesId { get; init; }
    public string? Brand { get; init; }
    public string? Series { get; init; }
    public int PageNumber { get; init; } = 1;
    public int PageSize { get; init; } = 10;
}

public class GetProductsWithPaginationQueryHandler : IRequestHandler<GetProductWithPaginationQuery, PaginatedList<ProductItemDto>>
{
    private readonly IApplicationDbContext _context;

    public GetProductsWithPaginationQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<PaginatedList<ProductItemDto>> Handle(GetProductWithPaginationQuery request, CancellationToken cancellationToken)
    {
        PaginatedList<ProductItemDto> products;
        switch (request)
        {
            case { BrandId: not null, SeriesId: not null }:
                products = await _context.ProductItems
                    .Where(x => x.SeriesId == request.SeriesId && x.BrandSeries.BrandId == request.BrandId)
                    .OrderByDescending(x => x.Created)
                    .Select(x => x.ToDto())
                    .PaginatedListAsync(request.PageNumber, request.PageSize);
                break;
            case { Brand: not null, Series: not null }:
                var brandId = await _context.Brands
                    .Where(x => x.Name == request.Brand)
                    .Select(x => x.Id)
                    .FirstOrDefaultAsync(cancellationToken);
                
                if (brandId == Guid.Empty)
                {
                    return new PaginatedList<ProductItemDto>(new List<ProductItemDto>(), 0, 0, 0);
                }
                
                products = await _context.ProductItems
                    .Where(x => x.BrandSeries.BrandId == brandId && x.BrandSeries.Name == request.Series)
                    .Select(x => x.ToDto())
                    .PaginatedListAsync(request.PageNumber, request.PageSize);
                break;
            default:
                throw new Exception(
                    "Invalid query parameters, please provide either BrandId and SeriesId or Brand and Series");
        }

        return products;
    }
}
