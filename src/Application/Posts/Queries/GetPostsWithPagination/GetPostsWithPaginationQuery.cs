using GerPros_Backend_API.Application.Common.Interfaces;
using GerPros_Backend_API.Application.Common.Models;

namespace GerPros_Backend_API.Application.Posts.Queries.GetPostsWithPagination;

public record GetPostWithPaginationQuery : IRequest<PaginatedList<PostItemDto>>
{
    public string[]? Tags { get; set; }
    public int PageNumber { get; init; } = 1;
    public int PageSize { get; init; } = 10;
}

public class GetPostsWithPaginationQueryHandler(IApplicationDbContext context)
    : IRequestHandler<GetPostWithPaginationQuery,
        PaginatedList<PostItemDto>>
{
    public async Task<PaginatedList<PostItemDto>> Handle(GetPostWithPaginationQuery request,
        CancellationToken cancellationToken)
    {
        PaginatedList<PostItemDto>? result;
        if (request.Tags is not null && request.Tags.Length > 0)
        {
            var posts = context.Posts
                .Include(p => p.PostTags)
                .ThenInclude(pt => pt.Tag)
                .Where(p => p.PostTags.Any(pt => request.Tags.Contains(pt.Tag.Name)))
                .OrderByDescending(p => p.Created)
                .Select(p => new PostItemDto
                {
                    Id = p.Id,
                    Title = p.Title,
                    Description = p.Description,
                    CoverImage = p.CoverImage,
                    Tags = p.PostTags.Select(pt => pt.Tag.Name).ToArray(),
                    Created = p.Created,
                    LastModified = p.LastModified
                });

            result = await PaginatedList<PostItemDto>.CreateAsync(posts, request.PageNumber, request.PageSize);
            return result;
        }
        else
        {
            var posts = context.Posts
                .OrderByDescending(p => p.Created)
                .Select(p => new PostItemDto
                {
                    Id = p.Id,
                    Title = p.Title,
                    Description = p.Description,
                    CoverImage = p.CoverImage,
                    Tags = p.PostTags.Select(pt => pt.Tag.Name).ToArray(),
                    Created = p.Created,
                    LastModified = p.LastModified
                });

            result = await PaginatedList<PostItemDto>.CreateAsync(posts, request.PageNumber, request.PageSize);
            return result;
        }
    }
}
