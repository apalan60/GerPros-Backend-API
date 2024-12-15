namespace GerPros_Backend_API.Application.Faq.Queries.GetFaq;

public record FaqCategoryDto(
    Guid Id, 
    string CategoryName, 
    List<FaqItemDto> FaqItems
);
