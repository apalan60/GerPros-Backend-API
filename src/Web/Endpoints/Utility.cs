using GerPros_Backend_API.Application.Brands;

namespace GerPros_Backend_API.Web.Endpoints;

public class Utility: EndpointGroupBase
{
    public override void Map(WebApplication app)
    {
        app.MapGroup(this)
            .MapPost(SeedDatabase);
    }
    
    public async Task<IResult> SeedDatabase(ISender sender, SeedDatabaseCommand command)
    {
        await sender.Send(command);
        return Results.NoContent();
    }
}
