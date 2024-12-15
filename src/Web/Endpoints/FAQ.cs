using GerPros_Backend_API.Application.Faq.Command;
using GerPros_Backend_API.Application.Faq.Queries.GetFaq;

namespace GerPros_Backend_API.Web.Endpoints;

public class FAQ : EndpointGroupBase
{
    public override void Map(WebApplication app)
    {
        app.MapGroup(this)
            .MapGet(GetFaq);

        app.MapGroup(this)
            .RequireAuthorization()
            .MapPost(CreateFaq)
            .MapPut(UpdateFaq, "{id}");
    }

    public Task<List<FaqCategoryDto>> GetFaq(ISender sender,[AsParameters]GetFaqCategoriesQuery query)
    {
        return sender.Send(query);
    }
    
    public Task<FaqCategoryDto> CreateFaq(ISender sender, CreateFaqCommand command)
    {
        return sender.Send(command);
    }

    public async Task<IResult> UpdateFaq(ISender sender, Guid id, UpdateFaqCommand command)
    {
        if (id != command.Id) return Results.BadRequest();
        await sender.Send(command);
        return Results.NoContent();
    }

}
