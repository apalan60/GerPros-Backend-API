using GerPros_Backend_API.Domain.Entities;

namespace GerPros_Backend_API.Application.Products.Queries.GetProductsWithPagination;

public class ProductBriefDto
{
    public int Id { get; init; }

    public int ListId { get; init; }

    public string? Title { get; init; }

    public bool Done { get; init; }

    private class Mapping : Profile
    {
        public Mapping()
        {
            CreateMap<ProductItem, ProductBriefDto>();
        }
    }
}
