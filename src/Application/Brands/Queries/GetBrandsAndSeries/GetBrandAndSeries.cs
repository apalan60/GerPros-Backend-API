﻿using GerPros_Backend_API.Application.Common.Interfaces;

namespace GerPros_Backend_API.Application.Brands.Queries.GetBrandsAndSeries;

public record GetBrandsAndSeriesQuery(bool IsManager = false) : IRequest<IEnumerable<BrandDto>>;

public class GetBrandsAndSeriesQueryHandler : IRequestHandler<GetBrandsAndSeriesQuery, IEnumerable<BrandDto>>
{
    private readonly IApplicationDbContext _context;

    public GetBrandsAndSeriesQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<BrandDto>> Handle(GetBrandsAndSeriesQuery request,
        CancellationToken cancellationToken)
    {
        List<BrandDto> brandList;
        if (request.IsManager)
        {
            // For manager, return all brands whenever they have series or product or not
            brandList = await _context.Brands
                .AsNoTracking()
                .Include(x => x.BrandSeries)
                .Select(x => x.ToDto())
                .ToListAsync(cancellationToken);
        }
        else
        {
            brandList = await _context.Brands
                .AsNoTracking()
                .AsSplitQuery()
                .Include(x => x.BrandSeries)
                .ThenInclude(bs => bs.ProductItems)
                .Where(x => x.BrandSeries.Any(y => y.ProductItems.Count > 0))
                .Select(x => x.ToDto())
                .ToListAsync(cancellationToken);
        }

        return brandList;
    }
}
