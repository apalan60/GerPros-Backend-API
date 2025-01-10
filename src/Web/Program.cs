using Amazon;
using GerPros_Backend_API.Infrastructure;
using GerPros_Backend_API.Infrastructure.Data;

var builder = WebApplication.CreateBuilder(args);

// add system manager parameter store
var awsOptions = builder.Configuration.GetAWSOptions();
awsOptions.Region ??= RegionEndpoint.GetBySystemName(builder.Configuration["AWS:Region"]);

var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Development";
builder.Configuration.AddSystemsManager(config =>
{
    config.AwsOptions = awsOptions;
    config.Path = $"/gerpros/{environment}";
    config.Optional = false;
    config.ReloadAfter = TimeSpan.FromDays(1);
    config.OnLoadException += exceptionContext =>
    {
        Console.WriteLine($"Error loading parameter: {exceptionContext.Exception.Message}");
        exceptionContext.Ignore = false; // 是否忽略加載錯誤
    };
});

// Add services to the container.
builder.Services.AddKeyVaultIfConfigured(builder.Configuration);

builder.Services.AddApplicationServices();
builder.Services.AddInfrastructureServices(builder.Configuration);
builder.Services.AddWebServices();

// Add CORS policy
// TODO - remove this in production
const string corsPolicyName = "AllowAll";
builder.Services.AddCors(options =>
{
    options.AddPolicy(corsPolicyName,
        policy => policy
            .AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader());
});

var app = builder.Build();

// Enable CORS middleware
// TODO - remove this in production
app.UseCors(corsPolicyName);

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    await app.InitialiseDatabaseAsync();
}
else
{
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHealthChecks("/health");
app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseSwaggerUi(settings =>
{
    settings.Path = "/api";
    settings.DocumentPath = "/api/specification.json";
});

app.MapControllerRoute(
    name: "default",
    pattern: "{controller}/{action=Index}/{id?}");

app.UseExceptionHandler(options => { });

app.Map("/", () => Results.Redirect("/api"));

//Avoiding CSRF attacks
// builder.Services.AddAntiforgery(options =>
// {
//     options.Cookie.Name = ".AspNetCore.Antiforgery"; // 自訂 Cookie 名稱
//     options.HeaderName = "X-CSRF-TOKEN"; // 用於 AJAX 或 API 測試
// });

app.MapEndpoints();

//health check
app.UseHealthChecks("/health");

app.Run();

public partial class Program
{
}
