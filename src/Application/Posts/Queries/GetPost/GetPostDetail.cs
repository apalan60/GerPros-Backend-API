using GerPros_Backend_API.Application.Common.Interfaces;

namespace GerPros_Backend_API.Application.Posts.Queries.GetPost;

public record GetPostDetail(Guid Id) : IRequest<PostDto>;

public class GetPostDetailQueryHandler(IApplicationDbContext context)
    : IRequestHandler<GetPostDetail, PostDto>
{
    public async Task<PostDto> Handle(GetPostDetail request, CancellationToken cancellationToken)
    {
        var post = await context.Posts
            .Include(p => p.PostTags)
            .ThenInclude(pt => pt.Tag)
            .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

        Guard.Against.NotFound(request.Id, post);

        return new PostDto
        {
            Id = post.Id,
            Title = post.Title,
            Description = post.Description,
            Content = post.Content,
            CoverImage = post.CoverImage,
            Tags = post.PostTags.Select(t => t.Tag.Name).ToArray(),
            Created = post.Created,
            LastModified = post.LastModified,
            FileStorageInfo = post.FileStorageInfo
        };
    }
}
