using GerPros_Backend_API.Application.Brands.Commands.CreateBrand;
using GerPros_Backend_API.Application.Brands.Commands.DeleteBrand;
using GerPros_Backend_API.Application.Brands.Commands.UpdateBrand;
using GerPros_Backend_API.Application.Brands.Queries.GetBrandsAndSeries;

namespace GerPros_Backend_API.Web.Endpoints;

public class Brands : EndpointGroupBase 
{
    public override void Map(WebApplication app)
    {
        app.MapGroup(this)
            .RequireAuthorization()
            .MapGet(GetBrands)
            .MapPost(CreateBrand)
            .MapPut(UpdateBrand, "{id}")
            .MapDelete(DeleteBrand, "{id}");
    }

    public Task<IEnumerable<BrandDto>> GetBrands(ISender sender, [AsParameters] GetBrandsAndSeriesQuery query)
    {
        return sender.Send(query);
    }
    
    public Task<Guid> CreateBrand (ISender sender, CreateBrandCommand command)
    {
        return sender.Send(command);
    }

    public async Task<IResult> UpdateBrand(ISender sender, Guid id, UpdateBrandCommand command)
    {
        if (id != command.Id) return Results.BadRequest();
        await sender.Send(command);
        return Results.NoContent();
    }

    public async Task<IResult> DeleteBrand(ISender sender, Guid id)
    {
        await sender.Send(new DeleteBrandCommand(id));
        return Results.NoContent();
    }
}
