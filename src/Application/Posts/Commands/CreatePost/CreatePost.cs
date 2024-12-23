using GerPros_Backend_API.Application.Common.Interfaces;
using GerPros_Backend_API.Domain;
using GerPros_Backend_API.Domain.Entities;

namespace GerPros_Backend_API.Application.Posts.Commands.CreatePost;

public record CreatePostCommand : IRequest<Guid>
{
    public required string Title { get; init; }

    public string? Description { get; init; }

    public string? Content { get; init; }

    public string? CoverImage { get; init; }

    public string[]? Tags { get; init; }

    public FileStorageInfo[]? FileStorageInfo { get; set; }
}

public class CreatePostItemCommandHandler(IApplicationDbContext context)
    : IRequestHandler<CreatePostCommand, Guid>
{
    public async Task<Guid> Handle(CreatePostCommand request, CancellationToken cancellationToken)
    {
        Guid postId = Guid.NewGuid();
        var entity = new Post
        {
            Id = postId,
            Title = request.Title,
            Description = request.Description,
            Content = request.Content,
            CoverImage = request.CoverImage,
            FileStorageInfo = request.FileStorageInfo
        };

        if (request.Tags == null)
        {
            context.Posts.Add(entity);
            await context.SaveChangesAsync(cancellationToken);
            return postId;
        }

        foreach (var tag in request.Tags)
        {
            var tagEntity = await context.Tags
                .Where(t => t.Name == tag)
                .FirstOrDefaultAsync(cancellationToken);

            if (tagEntity == null)
            {
                tagEntity = new Tag { Id = Guid.NewGuid(), Name = tag };
                context.Tags.Add(tagEntity);
            }

            context.PostTags.Add(new PostTag { PostId = postId, TagId = tagEntity.Id });
        }

        context.Posts.Add(entity);
        await context.SaveChangesAsync(cancellationToken);
        return entity.Id;
    }
}
