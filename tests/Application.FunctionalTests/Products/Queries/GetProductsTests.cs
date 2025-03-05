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
        
        // Series with same names but different brands
        var seriesSameName1 = await SendAsync(new CreateSeriesCommand { BrandId = brandSecond, Name = "壁畫"});
        var seriesSameName2 = await SendAsync(new CreateSeriesCommand { BrandId = brandId, Name = "壁畫"});
        
        await SendAsync(new CreateProductItemsCommand
        {
            Items = 
            [
                new CreateProductItemDto
                {
                    SeriesId = seriesSameName1,
                    Name = "Test Product 5",
                    Price = 500,
                    Image = "test.jpg",
                    Detail = "Test Product 5 Detail"
                },
                new CreateProductItemDto
                {
                    SeriesId = seriesSameName2,
                    Name = "Test Product 6",
                    Price = 600,
                    Image = "test.jpg",
                    Detail = "Test Product 6 Detail"
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
        result.Items.Should().HaveCount(6);
        result.Items.Should().Contain(x => x.Name == "Test Product 1");
        result.Items.Should().Contain(x => x.Name == "Test Product 2");
        result.Items.Should().Contain(x => x.Name == "Test Product 3");
        result.Items.Should().Contain(x => x.Name == "Test Product 4");
        result.Items.Should().Contain(x => x.Name == "Test Product 5");
        result.Items.Should().Contain(x => x.Name == "Test Product 6");
            
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
        result.Items.Should().HaveCount(4);
        result.Items.Should().Contain(x => x.Name == "Test Product 1");
        result.Items.Should().Contain(x => x.Name == "Test Product 2");
        result.Items.Should().Contain(x => x.Name == "Test Product 3");
    }
    
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


        var querySameNameSeries = new GetProductWithPaginationQuery { Series = "壁畫", PageNumber = 1, PageSize = 3 };
        var resultSameNameSeries = await SendAsync(querySameNameSeries);
        resultSameNameSeries.Items.Should().HaveCount(2);
        resultSameNameSeries.Items.Should().Contain(x => x.Name == "Test Product 5");
        resultSameNameSeries.Items.Should().Contain(x => x.Name == "Test Product 6");
    }

    [Test]
    public async Task ShouldGetSpecificProductItemsWhenSeriesNameAreSame()
    {
        var brand3 = await SendAsync(new CreateBrandCommand { Name = "Brand 3" });
        var sameNameSeries = await SendAsync(new CreateSeriesCommand { BrandId = brand3, Name = TestSeries12 });
        await SendAsync(new CreateProductItemsCommand
        {
            Items = 
            [
                new CreateProductItemDto
                {
                    SeriesId = sameNameSeries,
                    Name = "Test Product 5",
                    Price = 500,
                    Image = "test.jpg",
                    Detail = "Test Product 5 Detail"
                },
                new CreateProductItemDto
                {
                    SeriesId = sameNameSeries,
                    Name = "Test Product 6",
                    Price = 600,
                    Image = "test.jpg",
                    Detail = "Test Product 6 Detail"
                }
            ] 
        });
        
        
        //Assert
        var query = new GetProductWithPaginationQuery
        {
            Brand = "Brand 3",
            Series = TestSeries12,
            PageNumber = 1,
            PageSize = 10
        };
        var result = await SendAsync(query);
        result.Items.Should().HaveCount(2);
    }

}
