using GerPros_Backend_API.Application.Brands.EventHandlers;
using GerPros_Backend_API.Application.Common.Interfaces;
using GerPros_Backend_API.Domain.Entities;
using GerPros_Backend_API.Domain.Events;

namespace GerPros_Backend_API.Application.Brands.Commands.CreateBrand
{
    public record CreateBrandCommand : IRequest<Guid>
    {
        public string Name { get; init; } = null!;
    }

    public class CreateBrandCommandHandler : IRequestHandler<CreateBrandCommand, Guid>
    {
        private readonly IApplicationDbContext _context;

        public CreateBrandCommandHandler(IApplicationDbContext context)
        {
            _context = context;
        }
        

        public async Task<Guid> Handle(CreateBrandCommand request, CancellationToken cancellationToken)
        {
            var entity = new Brand
            {
                Name = request.Name
            };

            entity.AddDomainEvent(new BrandCreatedEvent(entity));
            _context.Brands.Add(entity);
            
            await _context.SaveChangesAsync(cancellationToken);

            return entity.Id;
        }
    }
}
