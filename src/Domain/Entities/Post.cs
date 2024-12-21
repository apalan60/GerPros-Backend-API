namespace GerPros_Backend_API.Domain.Entities;

public class Post : BaseAuditableEntity
{
    public string Title { get; init; } = null!;

    public string? Description { get; init; }

    public string? Content { get; init; }

    public string? CoverImage { get; init; }

    public string? FileInfo { get; set; }
    public virtual ICollection<PostTag>? PostTags { get; init; }
}
