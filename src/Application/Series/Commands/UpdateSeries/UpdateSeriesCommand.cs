using GerPros_Backend_API.Application.Common.Interfaces;
using GerPros_Backend_API.Application.Products.Commands.UpdateProduct;

namespace GerPros_Backend_API.Application.Series.Commands.UpdateSeries;

public record UpdateSeriesCommand : IRequest
{
    public Guid Id { get; init; }
    
    public string Name { get; init; } = null!;
}

public class UpdateProductCommandHandler : IRequestHandler<UpdateSeriesCommand>
{
    private readonly IApplicationDbContext _context;

    public UpdateProductCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task Handle(UpdateSeriesCommand request, CancellationToken cancellationToken)
    {
        var entity = await _context.BrandSeries.FindAsync([request.Id], cancellationToken: cancellationToken);

        Guard.Against.NotFound(request.Id, entity);

        entity.Name = request.Name;

        await _context.SaveChangesAsync(cancellationToken);
    }
}
