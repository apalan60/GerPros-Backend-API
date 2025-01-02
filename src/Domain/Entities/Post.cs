namespace GerPros_Backend_API.Domain.Entities;

public class Post : BaseAuditableEntity
{
    public string Title { get; set; } = null!;

    public string? Description { get; set; }

    public string? Content { get; set; }

    public string? CoverImage { get; set; }

    public virtual ICollection<PostTag> PostTags { get; init; } = [];
    
    /// <summary>
    /// jsonb
    /// </summary>
    public ICollection<FileStorageInfo>? FileStorageInfo { get; set; }
}
