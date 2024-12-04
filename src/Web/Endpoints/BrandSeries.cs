using GerPros_Backend_API.Application.Series.Commands.CreateSeries;
using GerPros_Backend_API.Application.Series.Commands.DeleteSeries;
using GerPros_Backend_API.Application.Series.Commands.UpdateSeries;

namespace GerPros_Backend_API.Web.Endpoints;

public class BrandSeries : EndpointGroupBase 
{
    public override void Map(WebApplication app)
    {
        app.MapGroup(this)
            .RequireAuthorization()
            .MapPost(CreateBrandSeries)
            .MapPut(UpdateBrandSeries, "{id}")
            .MapDelete(DeleteBrandSeries, "{id}");
    }
    
    public Task<Guid> CreateBrandSeries (ISender sender, CreateSeriesCommand command)
    {
        return sender.Send(command);
    }

    public async Task<IResult> UpdateBrandSeries(ISender sender, Guid id, UpdateSeriesCommand command)
    {
        if (id != command.Id) return Results.BadRequest();
        await sender.Send(command);
        return Results.NoContent();
    }

    public async Task<IResult> DeleteBrandSeries(ISender sender, Guid id)
    {
        await sender.Send(new DeleteSeriesCommand(id));
        return Results.NoContent();
    }
}
