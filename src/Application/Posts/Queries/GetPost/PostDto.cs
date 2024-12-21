using GerPros_Backend_API.Domain;

namespace GerPros_Backend_API.Application.Posts.Queries.GetPost;

public class PostDto
{
    public Guid Id { get; init; }
    
    public string Title { get; init; } = null!;

    public string? Description { get; init; }

    public string? Content { get; init; }

    public string? CoverImage { get; init; }

    public string[]? Tags { get; set; }
    
    public DateTimeOffset Created { get; set; }
    
    public DateTimeOffset? LastModified { get; set; }
    
    public ICollection<FileStorageInfo>? FileStorageInfo { get; set; }
}
