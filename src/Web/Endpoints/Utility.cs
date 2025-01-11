using GerPros_Backend_API.Application.Brands;
using GerPros_Backend_API.Application.Common.Interfaces;

namespace GerPros_Backend_API.Web.Endpoints;

public class Utility: EndpointGroupBase
{
    public override void Map(WebApplication app)
    {
        app.MapGroup(this)
            .RequireSecretKeyValidation()
            .MapPost(SeedDatabase, "SeedDatabase")
            .MapGet(GetConfig, "Config")
            .MapGet(GetHealthStatus, "HealthCheck/Database");
    }

    public async Task<IResult> SeedDatabase(ISender sender, SeedDatabaseCommand command)
    {
        await sender.Send(command);
        return Results.NoContent();
    }
    
    public Task<List<string?>> GetConfig(IConfiguration configuration)
    {
        return Task.FromResult((List<string?>)
        [
            configuration["ConnectionStrings:DefaultConnection"],
            configuration["ConnectionStrings:RDSConnection"],
            configuration["CloudFrontSettings:PrivateKey"],
        ]);
    }
    
    public async Task<IResult> GetHealthStatus(IApplicationDbContext context)
    {
        return await context.GetHealthStatus()? 
            Results.Ok("Database is healthy") : 
            Results.Problem("Database is unhealthy");
    }
}
