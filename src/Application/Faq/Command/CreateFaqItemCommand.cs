namespace GerPros_Backend_API.Application.Faq.Command;

public record CreateFaqItemCommand(
    string Question, 
    string Answer
);