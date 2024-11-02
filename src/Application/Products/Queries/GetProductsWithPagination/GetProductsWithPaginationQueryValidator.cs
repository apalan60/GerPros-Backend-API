using GerPros_Backend_API.Application.TodoItems.Queries.GetTodoItemsWithPagination;

namespace GerPros_Backend_API.Application.Products.Queries.GetProductsWithPagination;

public class GetProductsWithPaginationQueryValidator : AbstractValidator<GetTodoItemsWithPaginationQuery>
{
    public GetProductsWithPaginationQueryValidator()
    {
        RuleFor(x => x.ListId)
            .NotEmpty().WithMessage("ListId is required.");

        RuleFor(x => x.PageNumber)
            .GreaterThanOrEqualTo(1).WithMessage("PageNumber at least greater than or equal to 1.");

        RuleFor(x => x.PageSize)
            .GreaterThanOrEqualTo(1).WithMessage("PageSize at least greater than or equal to 1.");
    }
}
