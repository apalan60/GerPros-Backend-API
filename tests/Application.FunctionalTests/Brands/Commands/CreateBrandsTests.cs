using GerPros_Backend_API.Application.Brands.Commands.CreateBrand;
using GerPros_Backend_API.Application.Common.Exceptions;
using GerPros_Backend_API.Domain.Entities;

namespace GerPros_Backend_API.Application.FunctionalTests.Brands.Commands;

using static Testing;

public class CreateBrandsTests : BaseTestFixture
{
    [Test]
    public async Task ShouldRequireMinimumFields()
    {
        var command = new CreateBrandCommand();
        await FluentActions.Invoking(() => SendAsync(command)).Should().ThrowAsync<ValidationException>();
    }

    [Test]
    public async Task ShouldRequireName()
    {
        var command = new CreateBrandCommand
        {
            Name = string.Empty
        };

        await FluentActions.Invoking(() =>
            SendAsync(command)).Should().ThrowAsync<ValidationException>();
    }
    
    [Test]
    public async Task ShouldCreateBrand()
    {
        var command = new CreateBrandCommand
        {
            Name = "Test Brand"
        };

        var brandId = await SendAsync(command);

        var brand = await FindAsync<Brand>(brandId);

        brand.Should().NotBeNull();
        brand!.Name.Should().Be(command.Name);
    }
}
