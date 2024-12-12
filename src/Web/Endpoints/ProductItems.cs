using GerPros_Backend_API.Application.Common.Models;
using GerPros_Backend_API.Application.Products.Commands.CreateProduct;
using GerPros_Backend_API.Application.Products.Commands.DeleteProduct;
using GerPros_Backend_API.Application.Products.Commands.UpdateProduct;
using GerPros_Backend_API.Application.Products.Queries.GetProductItem;
using GerPros_Backend_API.Application.Products.Queries.GetProductsWithPagination;
using Microsoft.AspNetCore.Mvc;

namespace GerPros_Backend_API.Web.Endpoints;

public class ProductItems : EndpointGroupBase
{
    public override void Map(WebApplication app)
    {
        app.MapGroup(this)
            .RequireAuthorization()
            .MapPost(CreateProductItem)
            .MapPut(UpdateProductItem, "{id}")
            .MapDelete(DeleteProductItem, "{id}");

        app.MapGroup(this)
            .AllowAnonymous()
            .MapGet(GetProductItemDetail, "{id}")
            .MapGet(GetProductsWithPagination);
    }

    public Task<ProductItemDto> GetProductItemDetail(ISender sender, Guid id)
    {
        return sender.Send(new GetProductItemDetail(id));
    }

    public Task<PaginatedList<ProductItemDto>> GetProductsWithPagination(ISender sender,
        [AsParameters] GetProductWithPaginationQuery query)
    {
        return sender.Send(query);
    }

    public async Task<Guid> CreateProductItem(
        ISender sender,
        [FromForm] Guid seriesId,
        [FromForm] string name,
        [FromForm] decimal price,
        [FromForm] string? detail,
        [FromForm] IFormFile? image)
    {
        UploadedFile? uploadedFile = null;
        if (image is { Length: > 0 })
        {
            uploadedFile = new UploadedFile(
                image.OpenReadStream(),
                image.FileName,
                image.ContentType
            );
        }

        var command = new CreateProductItemCommand
        {
            SeriesId = seriesId,
            Name = name,
            Price = price,
            Detail = detail,
            File = uploadedFile
        };

        return await sender.Send(command);
    }


    public async Task<IResult> UpdateProductItem(ISender sender, Guid id, UpdateProductCommand command)
    {
        if (id != command.Id) return Results.BadRequest();
        await sender.Send(command);
        return Results.NoContent();
    }

    public async Task<IResult> DeleteProductItem(ISender sender, Guid id)
    {
        await sender.Send(new DeleteProductCommand(id));
        return Results.NoContent();
    }
}
