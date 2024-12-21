using GerPros_Backend_API.Application.Common.Interfaces;
using GerPros_Backend_API.Domain.Entities;

namespace GerPros_Backend_API.Application.Posts.Commands.CreatePost;

public record CreatePostItemCommand : IRequest<Guid>
{
    public required string Title { get; init; }

    public string? Description { get; init; }

    public string? Content { get; init; }

    public string? CoverImage { get; init; }

    public string[]? Tags { get; init; }

    public string? ImagesKeys { get; set; }
}

public class CreatePostItemCommandHandler(IApplicationDbContext context)
    : IRequestHandler<CreatePostItemCommand, Guid>
{
    public async Task<Guid> Handle(CreatePostItemCommand request, CancellationToken cancellationToken)
    {
        Guid postId = Guid.NewGuid();
        var entity = new Post
        {
            Id = postId,
            Title = request.Title,
            Description = request.Description,
            Content = request.Content,
            CoverImage = request.CoverImage
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
