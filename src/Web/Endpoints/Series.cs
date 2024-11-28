using GerPros_Backend_API.Application.Brands.Queries.GetBrandsAndSeries;
using GerPros_Backend_API.Application.Series.Commands.CraeteSeries;

namespace GerPros_Backend_API.Web.Endpoints;

public class Series :EndpointGroupBase
{
    public override void Map(WebApplication app)
    {
        app.MapGroup(this)
            .RequireAuthorization()
            .MapGet(GetSeries)
            .MapPost(CreateSeries);
    }

    private Task<Guid> CreateSeries(ISender sender, CreateBrandSeriesCommand command)
    {
        return sender.Send(command);
    }
    
    private Task<IEnumerable<BrandDto>> GetSeries(ISender sender, [AsParameters] GetBrandsAndSeriesQuery query)
    {
        return sender.Send(query);
    }
    
}
