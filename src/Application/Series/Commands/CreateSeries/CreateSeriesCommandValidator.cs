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
        RuleFor(v => v.BrandId)
            .MustAsync(ShouldExistBrand)
            .WithMessage("Brand with id {PropertyValue} does not exist.");
    }

    private async Task<bool> BeUniqueName(CreateSeriesCommand command, string name, CancellationToken cancellationToken) => 
        await _context.BrandSeries.AllAsync(l => l.Name != name, cancellationToken);
    

    private async Task<bool> ShouldExistBrand(CreateSeriesCommand command, Guid brandId, CancellationToken cancellationToken) => 
        await _context.Brands.AnyAsync(b => b.Id == brandId, cancellationToken);
}
