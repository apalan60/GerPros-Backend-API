using GerPros_Backend_API.Infrastructure.Identity;

namespace GerPros_Backend_API.Web.Endpoints;

public class Users : EndpointGroupBase
{
    public override void Map(WebApplication app)
    {
        app.MapGroup(this)
            .MapIdentityApi<ApplicationUser>();
    }
}
