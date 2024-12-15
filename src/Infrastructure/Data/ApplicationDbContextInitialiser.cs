using System.Text.Json;
using GerPros_Backend_API.Domain.Constants;
using GerPros_Backend_API.Domain.Entities;
using GerPros_Backend_API.Infrastructure.Identity;
using GerPros_Backend_API.Infrastructure.Seed;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace GerPros_Backend_API.Infrastructure.Data;

public static class InitialiserExtensions
{
    public static async Task InitialiseDatabaseAsync(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();

        var initialiser = scope.ServiceProvider.GetRequiredService<ApplicationDbContextInitialiser>();

        await initialiser.InitialiseAsync();

        await initialiser.SeedAsync();
    }
}

public class ApplicationDbContextInitialiser
{
    private readonly ILogger<ApplicationDbContextInitialiser> _logger;
    private readonly ApplicationDbContext _context;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;

    public ApplicationDbContextInitialiser(ILogger<ApplicationDbContextInitialiser> logger,
        ApplicationDbContext context, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
    {
        _logger = logger;
        _context = context;
        _userManager = userManager;
        _roleManager = roleManager;
    }

    public async Task InitialiseAsync()
    {
        try
        {
            await _context.Database.MigrateAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while initialising the database.");
            throw;
        }
    }

    public async Task SeedAsync()
    {
        try
        {
            await TrySeedAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while seeding the database.");
            throw;
        }
    }

    public async Task TrySeedAsync()
    {
        // Default roles
        var administratorRole = new IdentityRole(Roles.Administrator);

        if (_roleManager.Roles.All(r => r.Name != administratorRole.Name))
        {
            await _roleManager.CreateAsync(administratorRole);
        }

        // Default users
        var administrator =
            new ApplicationUser { UserName = "administrator@localhost", Email = "administrator@localhost" };

        if (_userManager.Users.All(u => u.UserName != administrator.UserName))
        {
            await _userManager.CreateAsync(administrator, "Administrator1!");
            if (!string.IsNullOrWhiteSpace(administratorRole.Name))
            {
                await _userManager.AddToRolesAsync(administrator, new[] { administratorRole.Name });
            }
        }

        // Default data
        // Seed, if necessary
        if (!_context.TodoLists.Any())
        {
            _context.TodoLists.Add(new TodoList
            {
                Title = "Todo List",
                Items =
                {
                    new TodoItem { Title = "Make a todo list 📃" },
                    new TodoItem { Title = "Check off the first item ✅" },
                    new TodoItem { Title = "Realise you've already done two things on the list! 🤯" },
                    new TodoItem { Title = "Reward yourself with a nice, long nap 🏆" },
                }
            });

            await _context.SaveChangesAsync();
        }

        // Seed Brands and BrandSeries if necessary
        if (!_context.Brands.Any())
        {
            var artfloorBrand = new Brand { Name = "Artfloor" };
            var arteoBrand = new Brand { Name = "Arteo" };

            var urbanSeries = new BrandSeries { Name = "Urban", BrandId = artfloorBrand.Id };
            var normalSeries = new BrandSeries { Name = "normal", BrandId = arteoBrand.Id };

            _context.Brands.AddRange(artfloorBrand, arteoBrand);
            _context.BrandSeries.AddRange(urbanSeries, normalSeries);

            await _context.SaveChangesAsync();
        }

        // Seed ProductItem if necessary
        if (!_context.ProductItems.Any())
        {
            var artfloorBrand = await _context.Brands.FirstOrDefaultAsync(b => b.Name == "Artfloor");
            var arteoBrand = await _context.Brands.FirstOrDefaultAsync(b => b.Name == "Arteo");

            var urbanSeries = await _context.BrandSeries.FirstOrDefaultAsync(s => s.Name == "Urban");
            var normalSeries = await _context.BrandSeries.FirstOrDefaultAsync(s => s.Name == "normal");

            if (urbanSeries != null && normalSeries != null)
            {
                _context.ProductItems.AddRange(new List<ProductItem>
                {
                    new()
                    {
                        Name = "奶油色橡木",
                        Price = 7200.00M,
                        Image =
                            "https://your-s3-bucket.s3.amazonaws.com/images/product1.jpg?AWSAccessKeyId=AKIAIOSFODNN7...&Expires=1600000000&Signature=abcdefghij...",
                        SeriesId = urbanSeries.Id,
                        BrandSeries = urbanSeries
                    },
                    new()
                    {
                        Name = "棕色橡木",
                        Price = 2000.00M,
                        Image =
                            "https://your-s3-bucket.s3.amazonaws.com/images/product2.jpg?AWSAccessKeyId=AKIAIOSFODNN7...&Expires=1600000000&Signature=abcdefghij...",
                        SeriesId = normalSeries.Id,
                        BrandSeries = normalSeries
                    }
                });

                await _context.SaveChangesAsync();
            }
        }

        // Seed FaqCategory and FaqItem if necessary
        if (!_context.FaqCategories.Any())
        {
            var filePath = Path.Combine("src", "Infrastructure", "Seed", "faq.json");
            var faqJson = await File.ReadAllTextAsync(filePath);
            var faqData = JsonSerializer.Deserialize<Dictionary<string, List<FaqItemJson>>>(faqJson);

            if (faqData == null || faqData.Count == 0)
                throw new InvalidOperationException("FAQ JSON 資料無效或為空");

            var faqCategories = faqData.Select(category => new FaqCategory
            {
                Id = Guid.NewGuid(),
                Name = category.Key,
                FaqItems = category.Value.Select(item => new FaqItem
                {
                    Id = Guid.NewGuid(), Question = item.Question, Answer = item.Answer
                }).ToList()
            }).ToList();

            _context.AddRange(faqCategories);
            await _context.SaveChangesAsync();
        }
    }
}
