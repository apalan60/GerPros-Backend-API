using GerPros_Backend_API.Application.Common.Interfaces;

namespace GerPros_Backend_API.Application.Brands.Commands.UpdateBrand;

public record UpdateBrandCommand : IRequest
{
    public Guid Id { get; init; }
    public string Name { get; init; } = null!;
}

public class UpdateBrandCommandHandler : IRequestHandler<UpdateBrandCommand>
{
    private readonly IApplicationDbContext _context;

    public UpdateBrandCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task Handle(UpdateBrandCommand request, CancellationToken cancellationToken)
    {
        var entity = await _context.Brands.FindAsync([request.Id], cancellationToken: cancellationToken);
        
        Guard.Against.NotFound(request.Id, entity);
        
        entity.Name = request.Name;

        await _context.SaveChangesAsync(cancellationToken);
    }
}
