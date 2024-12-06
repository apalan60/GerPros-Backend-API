using GerPros_Backend_API.Application.Common.Interfaces;

namespace GerPros_Backend_API.Application.Brands;

public class SeedDatabaseCommand : IRequest
{
    public required string SecretKey { get; set; }
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
        if (request.SecretKey != "@123456789")
        {
            throw new Exception("Invalid secret key");
        }
       
        await _migrationService.MigrateAsync();
        await _migrationService.SeedSampleDataAsync();
    }
}
