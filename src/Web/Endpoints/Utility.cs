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

    private static async Task<IResult> SeedDatabase(ISender sender)
    {
        await sender.Send(new SeedDatabaseCommand());
        return Results.NoContent();
    }

    private static Task<List<string?>> GetConfig(IConfiguration configuration)
    {
        return Task.FromResult((List<string?>)
        [
            configuration["ConnectionStrings:DefaultConnection"],
            configuration["ConnectionStrings:RDSConnection"],
            configuration["CloudFrontSettings:PrivateKey"],
            configuration["SecretSettings:SecretKey"]
        ]);
    }

    private static async Task<IResult> GetHealthStatus(IApplicationDbContext context)
    {
        return await context.GetHealthStatus()? 
            Results.Ok("Database is healthy") : 
            Results.Problem("Database is unhealthy");
    }
}
