namespace GerPros_Backend_API.Application.Posts.Queries.GetPostsWithPagination;

public class GetProductsWithPaginationQueryValidator : AbstractValidator<GetProductWithPaginationQuery>
{
    public GetProductsWithPaginationQueryValidator()
    {
        RuleFor(x => x.PageNumber)
            .GreaterThanOrEqualTo(1).WithMessage("PageNumber at least greater than or equal to 1.");

        RuleFor(x => x.PageSize)
            .GreaterThanOrEqualTo(1).WithMessage("PageSize at least greater than or equal to 1.");
    }
}
