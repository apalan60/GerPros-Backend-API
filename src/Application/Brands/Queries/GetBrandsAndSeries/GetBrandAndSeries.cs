using GerPros_Backend_API.Application.Common.Interfaces;
using GerPros_Backend_API.Application.Common.Security;

namespace GerPros_Backend_API.Application.Brands.Queries.GetBrandsAndSeries;

[Authorize]
public record GetBrandsAndSeriesQuery : IRequest<IEnumerable<BrandDto>>;

public class GetBrandsAndSeriesQueryHandler : IRequestHandler<GetBrandsAndSeriesQuery, IEnumerable<BrandDto>>
{
    private readonly IApplicationDbContext _context;

    public GetBrandsAndSeriesQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<BrandDto>> Handle(GetBrandsAndSeriesQuery request, CancellationToken cancellationToken)
    {
        var brandList = await _context.Brands
            .Include(x => x.BrandSeries)
            .Select(x => x.ToDto())
            .ToListAsync(cancellationToken);
        return brandList;
    }
}
