using GerPros_Backend_API.Application.Brands.Commands.CreateBrand;
using GerPros_Backend_API.Application.Common.Exceptions;
using GerPros_Backend_API.Application.Series.Commands.CreateSeries;

namespace GerPros_Backend_API.Application.FunctionalTests.BrandSeries.Command;

using static Testing;

public class CreateSeriesTest
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
    public async Task ShouldCreateBrand()
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
}
