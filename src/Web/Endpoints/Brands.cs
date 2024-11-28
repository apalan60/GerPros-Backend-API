using GerPros_Backend_API.Application.Brands.Commands.CreateBrand;
using GerPros_Backend_API.Application.Brands.Commands.DeleteBrand;
using GerPros_Backend_API.Application.Brands.Commands.UpdateBrand;

namespace GerPros_Backend_API.Web.Endpoints;

public class Brand : EndpointGroupBase 
{
    public override void Map(WebApplication app)
    {
        app.MapGroup(this)
            .RequireAuthorization()
            .MapPost(CreateBrand)
            .MapPut(UpdateBrand, "{id}")
            .MapDelete(DeleteBrand, "{id}");
    }


    private Task<Guid> CreateBrand (ISender sender, CreateBrandCommand command)
    {
        return sender.Send(command);
    }

    private async Task<IResult> UpdateBrand(ISender sender, Guid id, UpdateBrandCommand command)
    {
        if (id != command.Id) return Results.BadRequest();
        await sender.Send(command);
        return Results.NoContent();
    }

    private async Task<IResult> DeleteBrand(ISender sender, Guid id)
    {
        await sender.Send(new DeleteBrandCommand(id));
        return Results.NoContent();
    }
}
