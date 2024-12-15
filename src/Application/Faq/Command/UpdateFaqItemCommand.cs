namespace GerPros_Backend_API.Application.Faq.Command;

public record UpdateFaqItemCommand(
    Guid Id, 
    string Question, 
    string Answer
);