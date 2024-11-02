using GerPros_Backend_API.Application.TodoLists.Commands.CreateTodoList;
using GerPros_Backend_API.Application.TodoLists.Commands.DeleteTodoList;
using GerPros_Backend_API.Domain.Entities;

namespace GerPros_Backend_API.Application.FunctionalTests.TodoLists.Commands;

using static Testing;

public class DeleteProductsTests : BaseTestFixture
{
    [Test]
    public async Task ShouldRequireValidTodoListId()
    {
        var command = new DeleteTodoListCommand(new Guid());
        await FluentActions.Invoking(() => SendAsync(command)).Should().ThrowAsync<NotFoundException>();
    }

    [Test]
    public async Task ShouldDeleteTodoList()
    {
        var listId = await SendAsync(new CreateTodoListCommand
        {
            Title = "New List"
        });

        await SendAsync(new DeleteTodoListCommand(listId));

        var list = await FindAsync<TodoList>(listId);

        list.Should().BeNull();
    }
}
