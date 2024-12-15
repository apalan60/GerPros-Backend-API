using GerPros_Backend_API.Application.Common.Interfaces;

namespace GerPros_Backend_API.Application.Faq.Command;

public class CreateFaqCommandValidator : AbstractValidator<CreateFaqCommand>
{
    private readonly IApplicationDbContext _context;

    public CreateFaqCommandValidator(IApplicationDbContext context)
    {
        _context = context;
        RuleFor(v => v.CategoryName)
            .NotNull().WithMessage("Name is required.")
            .NotEmpty().WithMessage("Name is required.")
            .MustAsync(BeUniqueName)
            .WithMessage("'{PropertyName}' must be unique.")
            .WithErrorCode("Unique");
    }

    private async Task<bool> BeUniqueName(CreateFaqCommand command, string name, CancellationToken cancellationToken)
    {
        return await _context.FaqCategories.AllAsync(l => l.Name != name, cancellationToken);
    }
}
