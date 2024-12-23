namespace GerPros_Backend_API.Application.Posts.Queries.GetPostsWithPagination;

public class PostItemDto
{
    public Guid Id { get; init; }
    
    public string Title { get; init; } = null!;

    public string? Description { get; init; }

    public string? CoverImage { get; init; }

    public string[]? Tags { get; set; }
    
    public DateTimeOffset Created { get; set; }
    
    public DateTimeOffset? LastModified { get; set; }
}
