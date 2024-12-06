namespace GerPros_Backend_API.Application.Common.Interfaces;

public interface IMigrationService
{
    Task MigrateAsync(); 
    
    Task SeedSampleDataAsync();
}
