using GerPros_Backend_API.Application.Common.Interfaces;
using GerPros_Backend_API.Domain.Entities;
using GerPros_Backend_API.Domain.Events;

namespace GerPros_Backend_API.Application.Brands.Commands.DeleteBrand;

public record DeleteBrandCommand(Guid Id) : IRequest;

public class DeleteBrandCommandHandler : IRequestHandler<DeleteBrandCommand>
{
    private readonly IApplicationDbContext _context;

    public DeleteBrandCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task Handle(DeleteBrandCommand request, CancellationToken cancellationToken)
    {
        var entity = await _context.Brands.FindAsync([request.Id], cancellationToken: cancellationToken);
        
        Guard.Against.NotFound(request.Id, entity);
        
        entity.IsDeleted = true;
        
        entity.AddDomainEvent(new BrandDeletedEvent(entity));

        await _context.SaveChangesAsync(cancellationToken);
        
    }
}
