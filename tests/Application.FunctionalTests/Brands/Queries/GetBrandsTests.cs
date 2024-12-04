using GerPros_Backend_API.Application.Brands.Commands.CreateBrand;
using GerPros_Backend_API.Application.Brands.Queries.GetBrandsAndSeries;
using GerPros_Backend_API.Application.Series.Commands.CraeteSeries;

namespace GerPros_Backend_API.Application.FunctionalTests.Brands.Queries;

using static Testing;

public class GetBrandsTests : BaseTestFixture
{
    [Test]
    public async Task ShouldReturnAllListsAndItems()
    {
        await RunAsDefaultUserAsync();

        var id = await SendAsync(new CreateBrandCommand { Name = "Test Brand" });
        var id2 = await SendAsync(new CreateBrandCommand { Name = "Test Brand 2" });
        
        await SendAsync(new CreateBrandSeriesCommand { BrandId = id, Name = "Test Series 1-1" });
        await SendAsync(new CreateBrandSeriesCommand { BrandId = id, Name = "Test Series 1-2" });
        await SendAsync(new CreateBrandSeriesCommand { BrandId = id2, Name = "Test Series 2-1" });
        await SendAsync(new CreateBrandSeriesCommand { BrandId = id2, Name = "Test Series 2-2" });

        var query = new GetBrandsAndSeriesQuery();
        var result = await SendAsync(query);

        IEnumerable<BrandDto> brandDtos = result as BrandDto[] ?? result.ToArray();
        brandDtos.Should().HaveCount(2);
        var brand1 = brandDtos.Single(x => x.Name == "Test Brand");
        var brand2 = brandDtos.Single(x => x.Name == "Test Brand 2");
        brand1.Series.Should().HaveCount(2);
        brand1.Series!.First().Name.Should().Be("Test Series 1-1");
        brand1.Series!.Last().Name.Should().Be("Test Series 1-2");
        brand2.Series.Should().HaveCount(2);
        brand2.Series!.First().Name.Should().Be("Test Series 2-1");
        brand2.Series!.Last().Name.Should().Be("Test Series 2-2");
    }

    [Test]
    public async Task ShouldNotDenyAnonymousUser()
    {
        var query = new GetBrandsAndSeriesQuery();

        var action = () => SendAsync(query);
        
        await action.Should().NotThrowAsync<UnauthorizedAccessException>();
    }
}
