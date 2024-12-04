using GerPros_Backend_API.Application.Common.Interfaces;
using GerPros_Backend_API.Domain.Entities;

namespace GerPros_Backend_API.Application.Series.Commands.CreateSeries
{
    public record CreateSeriesCommand : IRequest<Guid>
    {
        public string Name { get; init; } = null!;
        public Guid BrandId { get; set; }
    }

    public class CreateSeriesCommandHandler : IRequestHandler<CreateSeriesCommand, Guid>
    {
        private readonly IApplicationDbContext _context;

        public CreateSeriesCommandHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Guid> Handle(CreateSeriesCommand request, CancellationToken cancellationToken)
        {
            var entity = new BrandSeries
            {
                Id = new Guid(),
                BrandId = request.BrandId,
                Name = request.Name
            };

            _context.BrandSeries.Add(entity);

            await _context.SaveChangesAsync(cancellationToken);

            return entity.Id;
        }
    }
}
