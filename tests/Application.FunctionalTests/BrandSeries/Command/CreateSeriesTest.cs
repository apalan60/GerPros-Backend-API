using GerPros_Backend_API.Application.Brands.Commands.CreateBrand;
using GerPros_Backend_API.Application.Common.Exceptions;
using GerPros_Backend_API.Application.Series.Commands.CreateSeries;

namespace GerPros_Backend_API.Application.FunctionalTests.BrandSeries.Command;

using static Testing;

public class CreateSeriesTest : BaseTestFixture
{
    [Test]
    public async Task ShouldRequireMinimumFields()
    {
        var command = new CreateSeriesCommand();
        await FluentActions.Invoking(() => SendAsync(command)).Should().ThrowAsync<ValidationException>();
    }

    [Test]
    public async Task ShouldRequireName()
    {
        var command = new CreateSeriesCommand
        {
            Name = string.Empty
        };

        await FluentActions.Invoking(() =>
            SendAsync(command)).Should().ThrowAsync<ValidationException>();
    }
    
    [Test]
    public async Task ShouldCreateSeries()
    {
        var brandId = await SendAsync(new CreateBrandCommand
        {
            Name = "Test Brand"
        });
            
        var command = new CreateSeriesCommand()
        {
            BrandId = brandId,
            Name = "Test Series"
        };

        var brandSeriesId = await SendAsync(command);

        var series = await FindAsync<Domain.Entities.BrandSeries>(brandSeriesId);

        series.Should().NotBeNull();
        series!.Name.Should().Be(command.Name);
    }

    [Test]
    public async Task ShouldAllowedDuplicateSeriesNameIfBrandIsDifference()
    {
        var brandId = await SendAsync(new CreateBrandCommand
        {
            Name = "BrandA"
        }); 
        
        var brandId2 = await SendAsync(new CreateBrandCommand
        {
            Name = "BrandB"
        });
        
        var command = new CreateSeriesCommand
        {
            BrandId = brandId,
            Name = "SeriesA"
        };
        
        await SendAsync(command);
        
        var differenceBrandCommand = new CreateSeriesCommand
        {
            BrandId = brandId2,
            Name = "SeriesA"
        };
        
        var sameBrandCommand = new CreateSeriesCommand
        {
            BrandId = brandId,
            Name = "SeriesA"
        };
        
        Assert.DoesNotThrowAsync(() => SendAsync(differenceBrandCommand));
        Assert.ThrowsAsync(typeof(ValidationException), () => SendAsync(sameBrandCommand));
    }
}
