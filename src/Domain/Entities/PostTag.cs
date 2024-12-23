namespace GerPros_Backend_API.Domain.Entities;

public class PostTag 
{
    public Guid PostId { get; init; }
    
    public Guid TagId { get; init; }
    
    public Post Post { get; init; } = null!;
    
    public Tag Tag { get; init; } = null!;
}
