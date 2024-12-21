using GerPros_Backend_API.Application.Common.Interfaces;
using GerPros_Backend_API.Domain.Enums;

namespace GerPros_Backend_API.Application.Posts.Commands.DeletePost;

public record DeletePostCommand(Guid Id) : IRequest;

public class DeletePostCommandHandler : IRequestHandler<DeletePostCommand>
{
    private readonly IApplicationDbContext _context;
    private readonly IFileStorageService _fileStorageService;

    public DeletePostCommandHandler(IApplicationDbContext context, IFileStorageService fileStorageService)
    {
        _context = context;
        _fileStorageService = fileStorageService;
    }

    public async Task Handle(DeletePostCommand request, CancellationToken cancellationToken)
    {
        var entity = await _context.PostItems
            .FindAsync([request.Id], cancellationToken);

        Guard.Against.NotFound<PostItem>(request.Id, entity);

        _context.PostItems.Remove(entity);

        entity.AddDomainEvent(new PostItemDeletedEvent(entity));

        await _context.SaveChangesAsync(cancellationToken);

        if (entity.Image != null)
        {
            await _fileStorageService.DeleteAsync(entity.Image, FileCategory.Post, cancellationToken);
        }
    }
}
