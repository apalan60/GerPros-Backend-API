﻿using GerPros_Backend_API.Application.Common.Models;
using GerPros_Backend_API.Application.Products.Commands.CreateProduct;
using GerPros_Backend_API.Application.Products.Commands.DeleteProduct;
using GerPros_Backend_API.Application.Products.Commands.UpdateProduct;
using GerPros_Backend_API.Application.Products.Queries.GetProductsWithPagination;

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
            .MapGet(GetProductsWithPagination);
    }

    public Task<PaginatedList<ProductItemDto>> GetProductsWithPagination(ISender sender, [AsParameters] GetProductWithPaginationQuery query)
    {
        return sender.Send(query);
    }
    
    public Task<Guid> CreateProductItem (ISender sender, CreateProductItemCommand command)
    {
        return sender.Send(command);
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
