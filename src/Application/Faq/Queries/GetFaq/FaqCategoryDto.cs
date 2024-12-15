public record FaqCategoryDto(
    Guid Id, 
    string CategoryName, 
    List<FaqItemDto> FaqItems
);
