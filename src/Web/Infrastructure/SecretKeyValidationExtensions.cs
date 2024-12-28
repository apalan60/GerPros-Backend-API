namespace GerPros_Backend_API.Web.Infrastructure;

public static class SecretKeyValidationExtensions
{
    public static RouteGroupBuilder RequireSecretKeyValidation(this RouteGroupBuilder builder)
    {
        return builder.AddEndpointFilter(async (context, next) =>
        {
            var httpContext = context.HttpContext;
            var configuration = httpContext.RequestServices.GetRequiredService<IConfiguration>();
            var secretKey = configuration["SecretSettings:SecretKey"];

            if (httpContext.Request.Headers.TryGetValue("SecretKey", out var providedKey) && providedKey == secretKey)
            {
                return await next(context);
            }

            httpContext.Response.StatusCode = StatusCodes.Status401Unauthorized;
            await httpContext.Response.WriteAsync("Unauthorized: Invalid or missing SecretKey");
            return null;

        });
    }
}
