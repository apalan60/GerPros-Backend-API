using GerPros_Backend_API.Domain.Entities;

namespace GerPros_Backend_API.Application.Products.Queries.GetProductsWithPagination;

public class ProductBriefDto
{
    public Guid Id { get; init; }

    public Guid ListId { get; init; }
    
    public bool Done { get; init; }

    private class Mapping : Profile
    {
        public Mapping()
        {
            CreateMap<ProductItem, ProductBriefDto>();
        }
    }
}
