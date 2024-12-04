using GerPros_Backend_API.Application.Common.Interfaces;

namespace GerPros_Backend_API.Application.Series.Commands.CreateSeries;

public class CreateSeriesCommandValidator : AbstractValidator<CreateSeriesCommand>
{
    private readonly IApplicationDbContext _context;

    public CreateSeriesCommandValidator(IApplicationDbContext context)
    {
        _context = context;
        RuleFor(v => v.Name)
            .NotNull().WithMessage("Name is required.")
            .NotEmpty().WithMessage("Name is required.")
            .MustAsync(BeUniqueName)
            .WithMessage("'{PropertyName}' must be unique.")
            .WithErrorCode("Unique");
    }

    private async Task<bool> BeUniqueName(CreateSeriesCommand command, string name, CancellationToken cancellationToken)
    {
        return await _context.Brands.AllAsync(l => l.Name != name, cancellationToken);
    }
}
