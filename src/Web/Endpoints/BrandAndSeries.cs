using GerPros_Backend_API.Application.Brands.Queries.GetBrandsAndSeries;
using GerPros_Backend_API.Application.Common.Models;
using GerPros_Backend_API.Application.Products.Commands.CreateProduct;
using GerPros_Backend_API.Application.Products.Commands.DeleteProduct;
using GerPros_Backend_API.Application.Products.Commands.UpdateProduct;
using GerPros_Backend_API.Application.Products.Queries.GetProductsWithPagination;

namespace GerPros_Backend_API.Web.Endpoints;

public class Brand : EndpointGroupBase 
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
    
    public Task<Guid> CreateBrand (ISender sender, CreateProductItemCommand command)
    {
        return sender.Send(command);
    }

    public async Task<IResult> UpdateBrand(ISender sender, Guid id, UpdateProductCommand command)
    {
        if (id != command.Id) return Results.BadRequest();
        await sender.Send(command);
        return Results.NoContent();
    }

    public async Task<IResult> DeleteBrand(ISender sender, Guid id)
    {
        await sender.Send(new DeleteProductCommand(id));
        return Results.NoContent();
    }
}
