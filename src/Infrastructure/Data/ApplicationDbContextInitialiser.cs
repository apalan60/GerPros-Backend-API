﻿using System.Text.Json;
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

        // Seed Brands and BrandSeries if necessary
        if (!_context.Brands.Any())
        {
            //Arteo 人字拼 一般款
            //ART AU系列 AR系列
            
            var artAuBrand = new Brand { Name = "ART" };
            var arteoBrand = new Brand { Name = "ARTEO" };

            var humanSeries = new BrandSeries { Name = "人字拼", BrandId = arteoBrand.Id };
            var normalSeries = new BrandSeries { Name = "一般款", BrandId = arteoBrand.Id };
            
            var artAuSeries = new BrandSeries { Name = "AU系列", BrandId = artAuBrand.Id };
            var artArSeries = new BrandSeries { Name = "AR系列", BrandId = artAuBrand.Id };

            _context.Brands.AddRange(artAuBrand, arteoBrand);
            _context.BrandSeries.AddRange(humanSeries, normalSeries, artAuSeries, artArSeries);

            await _context.SaveChangesAsync();
        }

        // Seed ProductItem if necessary
        if (!_context.ProductItems.Any())
        {
            var normalSeries = await _context.BrandSeries.FirstOrDefaultAsync(s => s.Name == "normal");

            if ( normalSeries != null)
            {
                _context.ProductItems.AddRange(new List<ProductItem>
                {
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
            const string faqJson = """
                                   {
                                   "地板常見問題": [
                                   {
                                   "question": "超耐磨木地板與SPC石塑地板差別?",
                                   "answer":
                                   "超耐磨木地板基體是原木纖維經由高壓壓制而成 底部有防潮平衡層 表層是木紋裝飾層加三氧化二鋁耐磨層\nSPC石塑地板基體是由石粉+塑料 混合而成 表層為抗UV貼合PVC複合材料"
                                   },
                                   {
                                   "question": "超耐磨木地板與SPC石塑地板優缺點",
                                   "answer":
                                   "超耐磨木地板\n優點:質感高 舒適溫潤 耐磨度高 耐用\n缺點:無法泡水 淹水\n\nSPC石塑地板\n優點:價位較低\n缺點:質感較差較硬 基面要平才能做(卡扣會斷) 耐磨度差 無法泡水 淹水(卡扣會斷)"
                                   },
                                   {
                                   "question": "超耐磨木地板防水嗎?",
                                   "answer":
                                   "超耐磨木地板在台灣已經將近30年歷史 現在基底密度已經達到HDF(高密度芯板) 而非以前的MDF(中密度芯板) 固然一般的生活抗潮 空氣濕度 拖地 都是可以適應的 只要不要泡水淹水基本上產品本身不會有問題"
                                   },
                                   {
                                   "question": "為什麼要留伸縮縫?",
                                   "answer":
                                   "因超耐磨木地板基底為實木木削 固然會隨著當地氣候濕度來進行 熱漲冷縮 所以我們標準工法都會在牆邊預留8MM伸縮縫 地板在剛做完半年內會慢慢的適應當地氣侯開始伸展 然而半年後基本上就不太會有變化了 除非外部的造成泡水淹水 固然我們都會提供施工一年保固 一年後基本上產品不會有大問題了"
                                   },
                                   {
                                   "question": "耐磨度標準AC3~AC5",
                                   "answer":
                                   "一般歐盟標準以AC做為表示\nAC3適合家用空間\nAC4適合商業空空間\nAC5適合高人流空間或輕工業空間"
                                   },
                                   {
                                   "question": "木地板進場施工時機",
                                   "answer": "建議抓細清潔前的一個工程\n這樣可以避免後續的工程不慎用傷地板"
                                   },
                                   {
                                   "question": "服務流程",
                                   "answer":
                                   "顧客資料填寫(地區 施作坪數 大樓OR透天 新屋OR翻新)\n指派當地業務與您接洽服務挑色 並做粗估的報價並完成付訂動作\n約時間場堪丈量精確坪數 與討論方向性 是否有額外增加的費用 比如登高費 搬運費 改門費用…等等\n討論施作日期 派工 施工\n驗收 尾款"
                                   },
                                   {
                                   "question": "地板保固",
                                   "answer": "家用品質保固15年 施工保固一年"
                                   }
                                   ],
                                   "室內設計常見問題": [
                                   {
                                   "question": "系統板/木心板等級",
                                   "answer": "系統板V313 E1等級以上/木心板F3等級以上/天花板用台製矽酸鈣板"
                                   },
                                   {
                                   "question": "施工工期",
                                   "answer": "依工程項目而定，100萬工程款約60個工作天"
                                   },
                                   {
                                   "question": "塑膠踢腳板需要拆除嗎",
                                   "answer":
                                   "若擔心塑膠踢腳板觀感不佳，可拆除再油漆修補，或拆除改以油性漆作為踢腳板"
                                   },
                                   {
                                   "question": "冷氣廠商有配合的嗎",
                                   "answer": "可自行找廠商，開工前再與我們現場討論配合"
                                   },
                                   {
                                   "question": "我要怎麼抓預算",
                                   "answer": "基礎裝修-新成屋3-5萬，15-30年中古屋5-8萬"
                                   },
                                   {
                                   "question": "簽約後要多久才能開工",
                                   "answer": "大部分情況下，1-2周即可開工"
                                   },
                                   {
                                   "question": "要提早多久找設計師",
                                   "answer": "交屋前3-4個月即可與我們聯繫，時間越充裕，規劃越完善"
                                   },
                                   {
                                   "question": "有低消嗎",
                                   "answer": "新成屋全室規劃30W起"
                                   },
                                   {
                                   "question": "丈量需要收費嗎",
                                   "answer": "我們目前免收丈量費，須提早跟我們預約"
                                   },
                                   {
                                   "question": "接案範圍到哪裡",
                                   "answer": "車程1小時內皆可接案"
                                   },
                                   {
                                   "question": "設計師會親自監工嗎",
                                   "answer": "會，我們公司皆為設計師親自處理每個細節"
                                   },
                                   {
                                   "question": "請問你們有作品集嗎",
                                   "answer": ""
                                   },
                                   {
                                   "question": "鬼月能開工嗎",
                                   "answer":
                                   "可在鬼月前完成開工儀式(如:在牆上打釘子以示開工)，通常只有入厝時間會避開農曆七月"
                                   },
                                   {
                                   "question": "有配合的家具廠商嗎",
                                   "answer": "凡與我們簽約的客戶，至１Ｆ東稻購買全商品可享９折優惠"
                                   },
                                   {
                                   "question": "可以不要做保護工程嗎",
                                   "answer": "施工難免會使地面或門受損，保護工程可避免造成損傷"
                                   },
                                   {
                                   "question": "你們假日或晚上可以約丈量嗎",
                                   "answer": "基本上以營業時間優先，若有特殊情況，可再與設計師討論"
                                   }
                                   ]
                                   }
                                   """;

            var faqData = JsonSerializer.Deserialize<Dictionary<string, List<FaqItemJson>>>(faqJson);

            if (faqData == null || faqData.Count == 0)
                throw new InvalidOperationException("FAQ JSON 資料無效或為空");

            var faqCategories = faqData.Select(category =>
            {
                Guid categoryId = Guid.NewGuid();
                return new FaqCategory
                {
                    Id = categoryId,
                    Name = category.Key,
                    FaqItems = category.Value.Select(item => new FaqItem
                    {
                       Question = item.Question, Answer = item.Answer
                    }).ToList()
                };
            }).ToList();

            _context.AddRange(faqCategories);
            await _context.SaveChangesAsync();
        }
    }
}
