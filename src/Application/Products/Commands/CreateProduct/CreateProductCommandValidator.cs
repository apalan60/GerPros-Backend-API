using GerPros_Backend_API.Application.TodoItems.Commands.CreateTodoItem;

namespace GerPros_Backend_API.Application.Products.Commands.CreateProduct;

public class CreateProductCommandValidator : AbstractValidator<TodoItems.Commands.CreateTodoItem.CreateProductItemCommand>
{
    public CreateProductCommandValidator()
    {
        RuleFor(v => v.Title)
            .MaximumLength(200)
            .NotEmpty();
    }
}
