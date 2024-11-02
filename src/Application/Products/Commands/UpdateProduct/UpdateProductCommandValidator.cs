using GerPros_Backend_API.Application.TodoItems.Commands.UpdateTodoItem;

namespace GerPros_Backend_API.Application.Products.Commands.UpdateProduct;

public class UpdateProductCommandValidator : AbstractValidator<UpdateTodoItemCommand>
{
    public UpdateProductCommandValidator()
    {
        RuleFor(v => v.Title)
            .MaximumLength(200)
            .NotEmpty();
    }
}
