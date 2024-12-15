namespace GerPros_Backend_API.Domain.Entities;

public class FaqCategory
{
    public Guid Id { get; set; }
    
    public string Name { get; set; } = null!;
    public List<FaqItem> FaqItems { get; set; } = [];
}
