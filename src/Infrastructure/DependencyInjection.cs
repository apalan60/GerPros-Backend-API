﻿using Amazon;
using Amazon.Runtime;
using Amazon.S3;
using Amazon.SimpleEmail;
using GerPros_Backend_API.Application.Common.Interfaces;
using GerPros_Backend_API.Domain.Constants;
using GerPros_Backend_API.Infrastructure.Data;
using GerPros_Backend_API.Infrastructure.Data.Interceptors;
using GerPros_Backend_API.Infrastructure.Files;
using GerPros_Backend_API.Infrastructure.Identity;
using GerPros_Backend_API.Infrastructure.Mails;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace GerPros_Backend_API.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services,
        IConfiguration configuration)
    {
        string? connectionString = configuration.GetConnectionString(configuration["ASPNETCORE_ENVIRONMENT"] == "Development"
            ? "DefaultConnection"
            : "RDSConnection");
        
        Guard.Against.Null(connectionString, message: "Connection string 'DefaultConnection' not found.");

        services.AddScoped<ISaveChangesInterceptor, AuditableEntityInterceptor>();
        services.AddScoped<ISaveChangesInterceptor, DispatchDomainEventsInterceptor>();

        services.AddDbContext<ApplicationDbContext>((sp, options) =>
        {
            options.AddInterceptors(sp.GetServices<ISaveChangesInterceptor>());

            options.UseNpgsql(connectionString);
        });

        services.AddScoped<IApplicationDbContext>(provider => provider.GetRequiredService<ApplicationDbContext>());

        services.AddScoped<ApplicationDbContextInitialiser>();

        services.AddSingleton<IMigrationService, MigrationService>();

        services.AddAuthentication()
            .AddBearerToken(IdentityConstants.BearerScheme);

        services.AddAuthorizationBuilder();

        services
            .AddIdentityCore<ApplicationUser>()
            .AddRoles<IdentityRole>()
            .AddEntityFrameworkStores<ApplicationDbContext>()
            .AddApiEndpoints();

        services.AddSingleton(TimeProvider.System);
        services.AddTransient<IIdentityService, IdentityService>();

        services.AddAuthorization(options =>
            options.AddPolicy(Policies.CanPurge, policy => policy.RequireRole(Roles.Administrator)));

        services.AddSingleton<IFileStorageService, FileStorageService>();
        services.AddSingleton<ICDNService, CloudFrontService>();

        // S3 setting
        services.Configure<S3Settings>(configuration.GetSection("S3Settings"));
        services.AddSingleton<IAmazonS3>(sp =>
        {
            var s3Setting = sp.GetRequiredService<IOptions<S3Settings>>().Value;
            var config = new AmazonS3Config { RegionEndpoint = RegionEndpoint.GetBySystemName(s3Setting.Region) };
            return new AmazonS3Client(config);
        });

        // CloudFront setting
        services.Configure<CloudFrontSettings>(configuration.GetSection("CloudFrontSettings"));

        // Secret setting
        services.Configure<SecretSettings>(configuration.GetSection("SecretSettings"));

        // Email Service
        services.AddSingleton<IEmailService>(sp =>
        {
            var region = RegionEndpoint.GetBySystemName(configuration["AWS:Region"]);
            return new AwsSesEmailService(region.SystemName, configuration["SES:VerifiedSenderEmail"]!);
        });
        // services.AddSingleton<IEmailService>(sp =>
        // {
        // const string smtpServer = "smtp.gmail.com";
        // const int smtpPort = 587;
        // const string gmailAccount = "";
        // const string gmailPassword = "";
        // return new MailKitEmailService(smtpServer, smtpPort, gmailAccount, gmailPassword);
        // });

        return services;
    }
}
