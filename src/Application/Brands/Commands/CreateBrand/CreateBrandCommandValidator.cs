using GerPros_Backend_API.Application.Common.Interfaces;

namespace GerPros_Backend_API.Application.Brands.Commands.CreateBrand;

public class CreateBrandCommandValidator : AbstractValidator<CreateBrandCommand>
{
    private readonly IApplicationDbContext _context;

    public CreateBrandCommandValidator(IApplicationDbContext context)
    {
        _context = context;
        RuleFor(v => v.Name)
            .MustAsync(BeUniqueName)
            .WithMessage("'{PropertyName}' must be unique.")
            .WithErrorCode("Unique");
    }

    private async Task<bool> BeUniqueName(CreateBrandCommand command, string name, CancellationToken cancellationToken)
    {
        return await _context.Brands.AllAsync(l => l.Name != name, cancellationToken);
    }
}
