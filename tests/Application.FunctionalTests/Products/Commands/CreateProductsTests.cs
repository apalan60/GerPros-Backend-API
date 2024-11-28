using GerPros_Backend_API.Application.Brands.Commands.CreateBrand;
using GerPros_Backend_API.Application.Common.Exceptions;
using GerPros_Backend_API.Application.Products.Commands.CreateProduct;
using GerPros_Backend_API.Application.Series.Commands.CreateSeries;
using GerPros_Backend_API.Domain.Entities;

namespace GerPros_Backend_API.Application.FunctionalTests.Products.Commands;

using static Testing;

public class CreateProductsTests : BaseTestFixture
{
    [Test]
    public async Task ShouldRequireMinimumFields()
    {
        var command = new CreateProductItemCommand();
        await FluentActions.Invoking(() => SendAsync(command)).Should().ThrowAsync<ValidationException>();
    }

    [Test]
    public async Task ShouldRequireName()
    {
        var command = new CreateProductItemCommand
        {
            SeriesId = Guid.NewGuid(),
            Price = 1.23m
        };

        await FluentActions.Invoking(() =>
            SendAsync(command)).Should().ThrowAsync<ValidationException>();
    }

    [Test]
    public async Task ShouldCreateBrandSeriesFirst()
    {
        var command = new CreateProductItemCommand
        {
            SeriesId = Guid.NewGuid(),
            Name = "Product Name",
            Price = 1.23m
        };

        await FluentActions.Invoking(() =>
            SendAsync(command)).Should().ThrowAsync<NotFoundException>();
    }
    
    [Test]
    public async Task ShouldCreateProduct()
    {
        var userId = await RunAsDefaultUserAsync();
        
        var brandId = await SendAsync(new CreateBrandCommand
        {
            Name = "Brand Name"
        });
        
        var seriesId = await SendAsync(new CreateBrandSeriesCommand
        {
            BrandId = brandId,
            Name = "Series Name"
        });

        var command = new CreateProductItemCommand
        {
            SeriesId = seriesId,
            Name = "Product Name",
            Price = 1.23m
        };

        var id = await SendAsync(command);

        var list = await FindAsync<ProductItem>(id);

        list.Should().NotBeNull();
        list!.Name.Should().Be(command.Name);
        list.Price.Should().Be(command.Price);
        list.CreatedBy.Should().Be(userId);
        list.Created.Should().BeCloseTo(DateTime.Now, TimeSpan.FromMilliseconds(10000));
    }
}
