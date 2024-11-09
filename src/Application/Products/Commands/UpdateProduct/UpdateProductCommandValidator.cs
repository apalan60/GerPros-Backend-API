namespace GerPros_Backend_API.Application.Products.Commands.UpdateProduct;

public class UpdateProductCommandValidator : AbstractValidator<UpdateProductCommand>
{
    public UpdateProductCommandValidator()
    {
        RuleFor(v => v.Name)
            .NotNull()
            .NotEmpty();

        RuleFor(v => v.Price)
            .GreaterThanOrEqualTo(0);
        
        RuleFor(v => v.Detail)
            .MaximumLength(1000)
            .WithMessage("Detail must be less than 1000 characters.");
    }
}
