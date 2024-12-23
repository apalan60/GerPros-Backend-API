using GerPros_Backend_API.Application.Common.Interfaces;
using GerPros_Backend_API.Domain.Enums;

namespace GerPros_Backend_API.Application.Posts.Commands.DeletePost;

public record DeletePostCommand(Guid Id) : IRequest;

public class DeletePostCommandHandler(IApplicationDbContext context, IFileStorageService fileStorageService)
    : IRequestHandler<DeletePostCommand>
{
    public async Task Handle(DeletePostCommand request, CancellationToken cancellationToken)
    {
        var entity = await context.Posts
            .FindAsync([request.Id], cancellationToken);

        Guard.Against.NotFound(request.Id.ToString(), entity);

        context.Posts.Remove(entity);

        await context.SaveChangesAsync(cancellationToken);

        if (entity.FileStorageInfo!= null)
        {
            foreach (var file in entity.FileStorageInfo)
            {
                await fileStorageService.DeleteAsync(file.Key, FileCategory.Post, cancellationToken);
            }
        }
    }
}
