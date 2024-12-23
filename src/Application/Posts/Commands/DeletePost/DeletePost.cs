using GerPros_Backend_API.Application.Common.Interfaces;
using GerPros_Backend_API.Domain.Enums;

namespace GerPros_Backend_API.Application.Posts.Commands.DeletePost;

public record DeletePostCommand(Guid Id) : IRequest;

public class DeletePostCommandHandler(IApplicationDbContext context, IFileStorageService fileStorageService)
    : IRequestHandler<DeletePostCommand>
{
    public async Task Handle(DeletePostCommand request, CancellationToken cancellationToken)
    {
        var post = await context.Posts
            .FindAsync([request.Id], cancellationToken);

        Guard.Against.NotFound(request.Id.ToString(), post);

        var postTags = await context.PostTags
            .Where(pt => pt.PostId == request.Id)
            .ToListAsync(cancellationToken);

        context.Posts.Remove(post);
        context.PostTags.RemoveRange(postTags);

        var unusedTags = await context.Tags
            .Where(t => t.PostTags != null && t.PostTags.Count == 0)
            .ToListAsync(cancellationToken);

        context.Tags.RemoveRange(unusedTags);

        await context.SaveChangesAsync(cancellationToken);

        if (post.FileStorageInfo != null)
        {
            foreach (var file in post.FileStorageInfo)
            {
                await fileStorageService.DeleteAsync(file.StorageKey, FileCategory.Post, cancellationToken);
            }
        }
    }
}
