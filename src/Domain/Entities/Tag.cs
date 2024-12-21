namespace GerPros_Backend_API.Domain.Entities;

public class Tag : BaseAuditableEntity
{
    public string Name { get; init; } = null!;
    
    public virtual ICollection<PostTag>? PostTags { get; init; }
}
