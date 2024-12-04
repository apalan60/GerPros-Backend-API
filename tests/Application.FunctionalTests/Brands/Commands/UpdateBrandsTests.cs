using GerPros_Backend_API.Application.Common.Exceptions;
using GerPros_Backend_API.Application.Brands.Commands.CreateBrand;
using GerPros_Backend_API.Application.Brands.Commands.UpdateBrand;
using GerPros_Backend_API.Domain.Entities;

namespace GerPros_Backend_API.Application.FunctionalTests.Brands.Commands;

using static Testing;

public class UpdateBrandsTests : BaseTestFixture
{
    [Test]
    public async Task ShouldRequireValidBrandId()
    {
        var command = new UpdateBrandCommand { Id = Guid.NewGuid(), Name = "New Name" };
        await FluentActions.Invoking(() => SendAsync(command)).Should().ThrowAsync<NotFoundException>();
    }

    [Test]
    public async Task ShouldRequireUniqueName()
    {
        var listId = await SendAsync(new CreateBrandCommand
        {
            Name = "New List"
        });

        await SendAsync(new CreateBrandCommand
        {
            Name = "Other List"
        });

        var command = new UpdateBrandCommand
        {
            Id = listId,
            Name = "Other List"
        };

        (await FluentActions.Invoking(() =>
            SendAsync(command))
                .Should().ThrowAsync<ValidationException>().Where(ex => ex.Errors.ContainsKey("Name")))
                .And.Errors["Name"].Should().Contain("'Name' must be unique.");
    }

    [Test]
    public async Task ShouldUpdateBrand()
    {
        var userId = await RunAsDefaultUserAsync();

        var listId = await SendAsync(new CreateBrandCommand
        {
            Name = "New List"
        });

        var command = new UpdateBrandCommand
        {
            Id = listId,
            Name = "Updated List Name"
        };

        await SendAsync(command);

        var list = await FindAsync<Brand>(listId);

        list.Should().NotBeNull();
        list!.Name.Should().Be(command.Name);
        list.LastModifiedBy.Should().NotBeNull();
        list.LastModifiedBy.Should().Be(userId);
        list.LastModified.Should().BeCloseTo(DateTime.Now, TimeSpan.FromMilliseconds(10000));
    }
}
