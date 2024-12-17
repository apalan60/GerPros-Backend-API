using System.Linq.Expressions;
using GerPros_Backend_API.Application.Common.Interfaces;
using GerPros_Backend_API.Application.Common.Mappings;
using GerPros_Backend_API.Application.Common.Models;
using GerPros_Backend_API.Domain.Entities;
using GerPros_Backend_API.Domain.Enums;

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

public class GetProductsWithPaginationQueryHandler(IApplicationDbContext context, IFileStorageService fileStorageService)
    : IRequestHandler<GetProductWithPaginationQuery,
        PaginatedList<ProductItemDto>>
{
    public async Task<PaginatedList<ProductItemDto>> Handle(GetProductWithPaginationQuery request,
        CancellationToken cancellationToken)
    {
        PaginatedList<ProductItemDto>? result = null;
        if (request.BrandId is not null && request.SeriesId is not null)
        {
            result = await GetPaginatedProductsAsync(
                x => x.SeriesId == request.SeriesId && x.BrandSeries.BrandId == request.BrandId,
                request.PageNumber,
                request.PageSize);
        }

        if (request.Brand is not null)
        {
            var brandId = await context.Brands
                .Where(x => x.Name == request.Brand)
                .Select(x => x.Id)
                .FirstOrDefaultAsync(cancellationToken);

            if (brandId == Guid.Empty)
                return new PaginatedList<ProductItemDto>(new List<ProductItemDto>(), 0, 0, 0);

            if (request.Series is null)
            {
                result = await GetPaginatedProductsAsync(
                    x => x.BrandSeries.BrandId == brandId,
                    request.PageNumber,
                    request.PageSize);
            }

            result = await GetPaginatedProductsAsync(
                x => x.BrandSeries.BrandId == brandId && x.BrandSeries.Name == request.Series,
                request.PageNumber,
                request.PageSize);
        }

        if (request.Brand is null && request.Series is null)
        {
            result = await GetPaginatedProductsAsync(
                x => true,
                request.PageNumber,
                request.PageSize);
        }

        if (result is null)
            throw new Exception("Invalid query parameters, please provide either BrandId and SeriesId or Brand and Series or Brand or none.");

        foreach (var item in result.Items)
        {
            if (item.Image is not null)
            {
                item.Image = await fileStorageService.GetUrlAsync(item.Image, FileCategory.Product);
            }
        }
        return result ?? throw new Exception("Invalid query parameters, please provide either BrandId and SeriesId or Brand and Series or Brand or none.");
    }

    private async Task<PaginatedList<ProductItemDto>> GetPaginatedProductsAsync(
        Expression<Func<ProductItem, bool>> predicate,
        int pageNumber,
        int pageSize) =>
        await context.ProductItems
            .Where(predicate)
            .Include(x => x.BrandSeries)
            .Include(x => x.BrandSeries.Brand)
            .OrderByDescending(x => x.Created)
            .Select(x => x.ToDto())
            .PaginatedListAsync(pageNumber, pageSize);
}
