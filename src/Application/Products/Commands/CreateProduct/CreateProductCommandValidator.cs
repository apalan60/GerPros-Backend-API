using GerPros_Backend_API.Application.TodoItems.Commands.CreateTodoItem;

namespace GerPros_Backend_API.Application.Products.Commands.CreateProduct;

public class CreateProductCommandValidator : AbstractValidator<CreateTodoItemCommand>
{
    public CreateProductCommandValidator()
    {
        RuleFor(v => v.Title)
            .MaximumLength(200)
            .NotEmpty();
    }
}
