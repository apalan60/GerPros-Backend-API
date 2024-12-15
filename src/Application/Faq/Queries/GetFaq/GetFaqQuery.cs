using GerPros_Backend_API.Application.Common.Interfaces;

namespace GerPros_Backend_API.Application.Faq.Queries.GetFaq;

public record GetFaqCategoriesQuery : IRequest<List<FaqCategoryDto>>;

public class GetFaqCategoriesQueryHandler(IApplicationDbContext context)
    : IRequestHandler<GetFaqCategoriesQuery, List<FaqCategoryDto>>
{
    public async Task<List<FaqCategoryDto>> Handle(
        GetFaqCategoriesQuery request, 
        CancellationToken cancellationToken)
    {
        var categories = await context.FaqCategories
            .ToListAsync(cancellationToken);
        
        return categories.Select(c => new FaqCategoryDto(
            c.Id,
            c.Name,
            c.FaqItems.Select(i => new FaqItemDto(
                i.Question,
                i.Answer
            )).ToList()
        )).ToList();
    }
}
