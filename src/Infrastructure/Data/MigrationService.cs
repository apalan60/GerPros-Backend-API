using GerPros_Backend_API.Application.Common.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace GerPros_Backend_API.Infrastructure.Data;

public class MigrationService(IServiceProvider serviceProvider) : IMigrationService
{
    public async Task MigrateAsync()
    {
        using var scope = serviceProvider.CreateScope();
        await scope.ServiceProvider.GetRequiredService<ApplicationDbContext>().Database.MigrateAsync();
    }

    public async Task SeedSampleDataAsync()
    {
        using var scope = serviceProvider.CreateScope();
        await scope.ServiceProvider.GetRequiredService<ApplicationDbContextInitialiser>().SeedAsync();
    }
}
