using GerPros_Backend_API.Application.Brands.Commands.CreateBrand;
using GerPros_Backend_API.Application.Products.Commands.CreateProduct;
using GerPros_Backend_API.Application.Products.Queries.GetProductsWithPagination;
using GerPros_Backend_API.Application.Series.Commands.CreateSeries;

namespace GerPros_Backend_API.Application.FunctionalTests.Products.Queries;

using static Testing;

public class GetProductsTests : BaseTestFixture
{
    const string TestBrand1 = "Test Brand";
    const string TestSeries11 = "Test Series 1-1";
    const string TestSeries12 = "Test Series 1-2";
    
    const string TestBrand2 = "Test Brand 2";
    const string TestSeries21 = "Test Series 2-1";
    
    //Init
    [SetUp]
    public async Task Init()
    {
        await RunAsDefaultUserAsync();
        
        //Create Brands
        var brandId = await SendAsync(new CreateBrandCommand { Name = TestBrand1 });
        var brandSecond = await SendAsync(new CreateBrandCommand { Name = TestBrand2 });
        
        //Create Series
        var seriesId = await SendAsync(new CreateSeriesCommand { BrandId = brandId, Name = TestSeries11 });
        Guid seriesSecond = await SendAsync(new CreateSeriesCommand { BrandId = brandId, Name = TestSeries12 });
        
        var seriesThird = await SendAsync(new CreateSeriesCommand { BrandId = brandSecond, Name = TestSeries21 });
        
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
                },
                new CreateProductItemDto
                {
                    SeriesId = seriesThird,
                    Name = "Test Product 4",
                    Price = 400,
                    Image = "test.jpg",
                    Detail = "Test Product 4 Detail"
                }
            ] 
        }); 
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
        result.Items.Should().HaveCount(4);
        result.Items.Should().Contain(x => x.Name == "Test Product 1");
        result.Items.Should().Contain(x => x.Name == "Test Product 2");
        result.Items.Should().Contain(x => x.Name == "Test Product 3");
        result.Items.Should().Contain(x => x.Name == "Test Product 4");
            
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
    
    [Test]
    public async Task ShouldFilterByBrandAndSeries()
    {
        //Brand and Series
        var query = new GetProductWithPaginationQuery
        {
            Brand = TestBrand1,
            Series = TestSeries11,
            PageNumber = 1,
            PageSize = 10
        };
        var result = await SendAsync(query);
        result.Items.Should().HaveCount(2);
        result.Items.Should().Contain(x => x.Name == "Test Product 1");
        result.Items.Should().Contain(x => x.Name == "Test Product 2");
    }
    
    //Brand
    [Test]
    public async Task ShouldFilterByBrand()
    {
        var query = new GetProductWithPaginationQuery
        {
            Brand = TestBrand1,
            PageNumber = 1,
            PageSize = 10
        };
        var result = await SendAsync(query);
        result.Items.Should().HaveCount(3);
        result.Items.Should().Contain(x => x.Name == "Test Product 1");
        result.Items.Should().Contain(x => x.Name == "Test Product 2");
        result.Items.Should().Contain(x => x.Name == "Test Product 3");
    }
    
    //Series
    [Test]
    public async Task ShouldFilterBySeries()
    {
        var query = new GetProductWithPaginationQuery
        {
            Series = TestSeries11,
            PageNumber = 1,
            PageSize = 10
        };
        var result = await SendAsync(query);
        result.Items.Should().HaveCount(2);
        result.Items.Should().Contain(x => x.Name == "Test Product 1");
        result.Items.Should().Contain(x => x.Name == "Test Product 2");
    }
    
}
