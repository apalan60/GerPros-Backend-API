using GerPros_Backend_API.Application.Products.Commands.DeleteProduct;
using GerPros_Backend_API.Application.TodoLists.Commands.CreateTodoList;
using GerPros_Backend_API.Application.TodoLists.Commands.DeleteTodoList;
using GerPros_Backend_API.Domain.Entities;

namespace GerPros_Backend_API.Application.FunctionalTests.Products.Commands;

using static Testing;

public class DeleteProductsTests : BaseTestFixture
{
    [Test]
    public async Task ShouldRequireValidProductId()
    {
        var command = new DeleteProductCommand(new Guid());
        await FluentActions.Invoking(() => SendAsync(command)).Should().ThrowAsync<NotFoundException>();
    }

    [Test]
    public async Task ShouldDeleteProduct()
    {
        await RunAsDefaultUserAsync();
        
        var brandId = Guid.NewGuid();
        var seriesId = Guid.NewGuid();
        var productId = Guid.NewGuid();
        
        await AddAsync(new Brand
        {
            Id = brandId,
            Name = "Test Brand",
        });
        
        await AddAsync(new Domain.Entities.BrandSeries
        {
            Id = seriesId,
            Name = "Test Series",
            BrandId = brandId
        });
        
        await AddAsync(new ProductItem
        {
            Id = productId,
            SeriesId = seriesId,
            Name = "Test Product",
        });
       
        await SendAsync(new DeleteProductCommand(productId));
        
        var product = await FindAsync<ProductItem>(productId);
        product.Should().BeNull();
    }
}
