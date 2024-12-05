using GerPros_Backend_API.Application.Brands.Commands.CreateBrand;
using GerPros_Backend_API.Application.Products.Commands.CreateProduct;
using GerPros_Backend_API.Application.Products.Queries.GetProductsWithPagination;
using GerPros_Backend_API.Application.Series.Commands.CreateSeries;

namespace GerPros_Backend_API.Application.FunctionalTests.Products.Queries;

using static Testing;

public class GetProductsTests : BaseTestFixture
{
    const string TestBrand = "Test Brand";
    const string TestSeries = "Test Series 1-1";
    
    //Init
    [SetUp]
    public async Task Init()
    {
        await RunAsDefaultUserAsync();
        
        //Create Brands
        var brandId = await SendAsync(new CreateBrandCommand { Name = TestBrand });
        
        //Create Series
        var seriesId = await SendAsync(new CreateSeriesCommand { BrandId = brandId, Name = TestSeries });
        Guid seriesSecond = await SendAsync(new CreateSeriesCommand { BrandId = brandId, Name = "Test Series 1-2" });
        
        //Create Products
        await SendAsync(new CreateProductItemsCommand
        {
            Items = 
            [
                new CreateProductItemDto
                {
                    SeriesId = seriesId,
                    Name = "Test Product 1",
                    Price = 100,
                    Image = "test.jpg",
                    Detail = "Test Product 1 Detail"
                },
                new CreateProductItemDto
                {
                    SeriesId = seriesId,
                    Name = "Test Product 2",
                    Price = 200,
                    Image = "test.jpg",
                    Detail = "Test Product 2 Detail"
                },
                new CreateProductItemDto
                {
                    SeriesId = seriesSecond,
                    Name = "Test Product 3",
                    Price = 300,
                    Image = "test.jpg",
                    Detail = "Test Product 3 Detail"
                }
            ] 
        }); 
    }
    
    [Test]
    public async Task ShouldReturnSpecificListsAndItems()
    {
        var query = new GetProductWithPaginationQuery
        {
            Brand = TestBrand, 
            Series = TestSeries,
            PageNumber = 1,
            PageSize = 10
        };
        var result = await SendAsync(query);
        result.Items.Should().HaveCount(2);
        result.Items.Should().Contain(x => x.Name == "Test Product 1");
        result.Items.Should().Contain(x => x.Name == "Test Product 2");
    }

    [Test]
    public async Task ShouldReturnAllItems()
    {
        var query = new GetProductWithPaginationQuery
        {
            PageNumber = 1,
            PageSize = 10
        };
        var result = await SendAsync(query);
        result.Items.Should().HaveCount(3);
        result.Items.Should().Contain(x => x.Name == "Test Product 1");
        result.Items.Should().Contain(x => x.Name == "Test Product 2");
        result.Items.Should().Contain(x => x.Name == "Test Product 3");
            
    }
    
    [Test]
    public async Task ShouldNotDenyAnonymousUser()
    {
        var query = new GetProductWithPaginationQuery
        {
            Brand = "Test Brand",
            Series = "Test Series 1-1",
            PageNumber = 1,
            PageSize = 10
        };

        var action = () => SendAsync(query);
        
        await action.Should().NotThrowAsync<UnauthorizedAccessException>();
    }
}
