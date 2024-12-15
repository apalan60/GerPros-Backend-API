using GerPros_Backend_API.Application.Common.Interfaces;
using GerPros_Backend_API.Application.Faq.Queries.GetFaq;
using GerPros_Backend_API.Domain.Entities;

namespace GerPros_Backend_API.Application.Faq.Command;

public record CreateFaqCommand(string CategoryName, List<CreateOrUpdateFaqItemDto> FaqItems): IRequest<FaqCategoryDto>;

public class CreateFaqCategoryCommandHandler 
    : IRequestHandler<CreateFaqCommand, FaqCategoryDto>
{
    private readonly IApplicationDbContext _context;

    public CreateFaqCategoryCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<FaqCategoryDto> Handle(
        CreateFaqCommand request, 
        CancellationToken cancellationToken)
    {
        var category = new FaqCategory 
        {
            Name = request.CategoryName,
            FaqItems = request.FaqItems.Select(item => new FaqItem 
            {
                Question = item.Question,
                Answer = item.Answer
            }).ToList()
        };

        _context.FaqCategories.Add(category);
        await _context.SaveChangesAsync(cancellationToken);

        return new FaqCategoryDto(
            category.Id, 
            category.Name, 
            category.FaqItems.Select(item => new FaqItemDto(item.Question, item.Answer)).ToList()
        );
    }
}
