using GerPros_Backend_API.Application.Common.Interfaces;

namespace GerPros_Backend_API.Application.Brands.Commands.UpdateBrand;

public class UpdateBrandCommandValidator : AbstractValidator<UpdateBrandCommand>
{
    private readonly IApplicationDbContext _context;

    public UpdateBrandCommandValidator(IApplicationDbContext context)
    {
        _context = context;
        RuleFor(v => v.Id)
            .NotEmpty().WithMessage("{PropertyName} is required.")
            .NotEqual(Guid.Empty).WithMessage("{PropertyName} must not be empty.");
        
        RuleFor(v => v.Name)
            .MustAsync(BeUniqueName)
            .WithMessage("'{PropertyName}' must be unique.")
            .WithErrorCode("Unique");
    }
    
    private async Task<bool> BeUniqueName(UpdateBrandCommand command, string name, CancellationToken cancellationToken)
    {
        return await _context.Brands
            .Where(b => b.Id != command.Id)
            .AllAsync(l => l.Name != name, cancellationToken);
    }
}
