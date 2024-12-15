namespace GerPros_Backend_API.Application.Faq.Command;

public record UpdateFaqCategoryCommand(
    Guid Id, 
    string CategoryName, 
    List<UpdateFaqItemCommand> FaqItems
);