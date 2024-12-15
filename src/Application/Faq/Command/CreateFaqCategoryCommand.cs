namespace GerPros_Backend_API.Application.Faq.Command;

public record CreateFaqCategoryCommand(
    string CategoryName, 
    List<CreateFaqItemCommand> FaqItems
);