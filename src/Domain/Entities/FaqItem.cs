namespace GerPros_Backend_API.Domain.Entities;

public class FaqItem
{
    public Guid Id { get; set; }
    
    public string Question { get; set; } = null!;
    
    public string Answer { get; set; } = null!;
    
    public Guid FaqCategoryId { get; set; }
    
    public virtual FaqCategory Category { get; set; } = null!;
}
