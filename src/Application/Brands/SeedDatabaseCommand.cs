using GerPros_Backend_API.Application.Common.Interfaces;

namespace GerPros_Backend_API.Application.Brands;
public class SeedDatabaseCommand : IRequest
{
    public bool SeedSampleData { get; set; } = false;
}

public class SeedDatabaseCommandHandler : IRequestHandler<SeedDatabaseCommand>
{
    private readonly IMigrationService _migrationService;

    public SeedDatabaseCommandHandler(IMigrationService migrationService)
    {
        _migrationService = migrationService;
    }

    public async Task Handle(SeedDatabaseCommand request, CancellationToken cancellationToken)
    {
        await _migrationService.MigrateAsync();
        
        if (request.SeedSampleData)
            await _migrationService.SeedSampleDataAsync();
    }
}
