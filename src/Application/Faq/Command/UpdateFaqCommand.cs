using GerPros_Backend_API.Application.Common.Interfaces;
using GerPros_Backend_API.Domain.Entities;

namespace GerPros_Backend_API.Application.Faq.Command;

public record UpdateFaqCommand(
    Guid Id, 
    string CategoryName, 
    List<CreateOrUpdateFaqItemDto> FaqItems
) : IRequest<Unit>;


public class UpdateFaqCommandHandler(IApplicationDbContext context) : IRequestHandler<UpdateFaqCommand, Unit>
{
    public async Task<Unit> Handle(
        UpdateFaqCommand request, 
        CancellationToken cancellationToken)
    {
        var faqCategory = await context.FaqCategories
            .FirstOrDefaultAsync(category => category.Id == request.Id, cancellationToken);
        
        Guard.Against.NotFound(request.Id, faqCategory);

        faqCategory.Name = request.CategoryName;
        faqCategory.FaqItems = request.FaqItems.Select(item => new FaqItem
        {
            Question = item.Question,
            Answer = item.Answer
        }).ToList();
        
        await context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}


