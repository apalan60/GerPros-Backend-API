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
        PaginatedList<ProductItemDto> products = request switch
        {
            { BrandId: not null, SeriesId: not null } => await _context.ProductItems.Include(x => x.Brand)
                .Include(x => x.Series)
                .Where(x => x.BrandId == request.BrandId && x.SeriesId == request.SeriesId)
                .Select(x => x.ToDto())
                .PaginatedListAsync(request.PageNumber, request.PageSize),
            
            { Brand: not null, Series: not null } => await _context.ProductItems.Include(x => x.Brand)
                .Include(x => x.Series)
                .Where(x => x.Brand.Name == request.Brand && x.Series.Name == request.Series)
                .Select(x => x.ToDto())
                .PaginatedListAsync(request.PageNumber, request.PageSize),
            
            _ => throw new Exception(
                "Invalid query parameters, please provide either BrandId and SeriesId or Brand and Series")
        };

        return products;
    }
}
