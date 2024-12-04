using GerPros_Backend_API.Application.Common.Interfaces;

namespace GerPros_Backend_API.Application.Series.Commands.DeleteSeries
{
    public record DeleteSeriesCommand(Guid Id) : IRequest;

    public class DeleteSeriesCommandHandler : IRequestHandler<DeleteSeriesCommand>
    {
        private readonly IApplicationDbContext _context;

        public DeleteSeriesCommandHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task Handle(DeleteSeriesCommand request, CancellationToken cancellationToken)
        {
            var entity = await _context.BrandSeries.FindAsync([request.Id], cancellationToken: cancellationToken);

            Guard.Against.NotFound(request.Id, entity);

            _context.BrandSeries.Remove(entity);
            
            await _context.SaveChangesAsync(cancellationToken);
        }
    }
}
