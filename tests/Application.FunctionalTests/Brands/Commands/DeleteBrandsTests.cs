using GerPros_Backend_API.Application.Brands.Commands.CreateBrand;
using GerPros_Backend_API.Application.Brands.Commands.DeleteBrand;
using GerPros_Backend_API.Domain.Entities;

namespace GerPros_Backend_API.Application.FunctionalTests.Brands.Commands;

using static Testing;

public class DeleteBrandsTests : BaseTestFixture
{
    [Test]
    public async Task ShouldRequireValidBrand()
    {
        var command = new DeleteBrandCommand(new Guid());
        await FluentActions.Invoking(() => SendAsync(command)).Should().ThrowAsync<NotFoundException>();
    }

    [Test]
    public async Task ShouldDeleteBrand()
    {
        var id = await SendAsync(new CreateBrandCommand
        {
            Name = "New Brand" 
        });

        await SendAsync(new DeleteBrandCommand(id));

        var brand = await FindAsync<Brand>(id);

        brand.Should().BeNull();
    }
}
