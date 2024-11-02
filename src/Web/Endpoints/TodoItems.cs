using GerPros_Backend_API.Application.Common.Models;
using GerPros_Backend_API.Application.TodoItems.Commands.CreateTodoItem;
using GerPros_Backend_API.Application.TodoItems.Commands.DeleteTodoItem;
using GerPros_Backend_API.Application.TodoItems.Commands.UpdateTodoItem;
using GerPros_Backend_API.Application.TodoItems.Commands.UpdateTodoItemDetail;
using GerPros_Backend_API.Application.TodoItems.Queries.GetTodoItemsWithPagination;

namespace GerPros_Backend_API.Web.Endpoints;

public class TodoItems : EndpointGroupBase
{
    public override void Map(WebApplication app)
    {
        app.MapGroup(this)
            .RequireAuthorization()
            .MapGet(GetTodoItemsWithPagination)
            .MapPost(CreateTodoItem)
            .MapPut(UpdateTodoItem, "{id}")
            .MapPut(UpdateTodoItemDetail, "UpdateDetail/{id}")
            .MapDelete(DeleteTodoItem, "{id}");
    }

    public Task<PaginatedList<TodoItemBriefDto>> GetTodoItemsWithPagination(ISender sender, [AsParameters] GetTodoItemsWithPaginationQuery query)
    {
        return sender.Send(query);
    }

    public Task<Guid> CreateTodoItem(ISender sender, CreateProductItemCommand command)
    {
        return sender.Send(command);
    }

    public async Task<IResult> UpdateTodoItem(ISender sender, Guid id, UpdateTodoItemCommand command)
    {
        if (id != command.Id) return Results.BadRequest();
        await sender.Send(command);
        return Results.NoContent();
    }

    public async Task<IResult> UpdateTodoItemDetail(ISender sender, Guid id, UpdateTodoItemDetailCommand command)
    {
        if (id != command.Id) return Results.BadRequest();
        await sender.Send(command);
        return Results.NoContent();
    }

    public async Task<IResult> DeleteTodoItem(ISender sender, Guid id)
    {
        await sender.Send(new DeleteTodoItemCommand(id));
        return Results.NoContent();
    }
}
