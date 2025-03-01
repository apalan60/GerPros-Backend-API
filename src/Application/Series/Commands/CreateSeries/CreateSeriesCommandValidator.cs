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
            .MustAsync(BeUniqueNameWhenSameBrand)
            .WithMessage("'{PropertyName}' must be unique.")
            .WithErrorCode("Unique");
        RuleFor(v => v.BrandId)
            .MustAsync(ShouldExistBrand)
            .WithMessage("Brand with id {PropertyValue} does not exist.");
    }

    private async Task<bool> BeUniqueNameWhenSameBrand(CreateSeriesCommand command, string name, CancellationToken cancellationToken) => 
        await _context.BrandSeries.AnyAsync(l => l.BrandId == command.BrandId && l.Name == name, cancellationToken) == false;
    

    private async Task<bool> ShouldExistBrand(CreateSeriesCommand command, Guid brandId, CancellationToken cancellationToken) => 
        await _context.Brands.AnyAsync(b => b.Id == brandId, cancellationToken);
}
