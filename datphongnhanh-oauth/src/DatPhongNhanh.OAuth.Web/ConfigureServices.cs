using DatPhongNhanh.OAuth.Business.AppClaims;
using DatPhongNhanh.OAuth.Data.DbContexts;
using DatPhongNhanh.OAuth.Data.Entities.OpenIddict;
using DatPhongNhanh.OAuth.Data.Provider.Extensions;
using Microsoft.AspNetCore.Identity;
using OpenIddict.Abstractions;
using OpenIddict.Validation.AspNetCore;
using Vite.AspNetCore;

namespace DatPhongNhanh.OAuth.Web;

public static class ServiceCollectionExtensions
{
    public static void ConfigureOpenIddict(this IServiceCollection services)
    {
        services.AddOpenIddict()
           .AddCore(options =>
           {
               options.UseEntityFrameworkCore()
                   .UseDbContext<ApplicationDbContext>()
                   .ReplaceDefaultEntities<ApplicationEntity, AuthorizationEntity, ScopeEntity, TokenEntity, long>();
           })
           .AddServer(options =>
           {
               options.SetTokenEndpointUris("connect/token");

               options.AllowClientCredentialsFlow();

               options.UseAspNetCore()
                   .EnableTokenEndpointPassthrough()
                   .EnableAuthorizationEndpointPassthrough();

               options.AddDevelopmentEncryptionCertificate()
                   .AddDevelopmentSigningCertificate();

               options.SetAccessTokenLifetime(TimeSpan.FromMinutes(30));
               options.DisableAccessTokenEncryption();
               options.RegisterScopes("api", OpenIddictConstants.Scopes.OpenId, OpenIddictConstants.Scopes.Email,
                   OpenIddictConstants.Scopes.Profile, OpenIddictConstants.Scopes.Phone,
                   OpenIddictConstants.Scopes.Roles, OpenIddictConstants.Scopes.Address,
                   OpenIddictConstants.Scopes.OfflineAccess);
           })
           .AddValidation(options =>
           {
               options.UseLocalServer();

               options.UseAspNetCore();
           });

        services.AddTransient<DefaultOpenIddictClaimsPrincipalHandler>();
        services.Configure<OpenIddictClaimsPrincipalOptions>(options =>
        {
            options.ClaimsPrincipalHandlers.Add<DefaultOpenIddictClaimsPrincipalHandler>();
        });
        services.AddScoped<OpenIddictClaimsPrincipalManager>();

    }

    public static void ConfigureViteIntegration(this IServiceCollection services)
    {
        services.AddViteServices(options =>
        {
            options.Server.Https = true;
            options.Server.UseReactRefresh = true;
            options.Server.PackageDirectory = "ClientApp";
            options.Base = "dist";
            options.Server.AutoRun = false;
        });
    }

    public static void ConfigureIdentity(this IServiceCollection services)
    {
        services.AddIdentity<ApplicationUser, ApplicationRole>(options =>
            {
                options.User.RequireUniqueEmail = true;
                options.SignIn.RequireConfirmedAccount = false;
                options.SignIn.RequireConfirmedEmail = false;
                options.SignIn.RequireConfirmedPhoneNumber = false;
            })
           .AddEntityFrameworkStores<ApplicationDbContext>()
           .AddDefaultTokenProviders();
    }

    public static void ConfigureEntityFramework(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("DefaultConnection");
        ArgumentException.ThrowIfNullOrWhiteSpace(connectionString);
        services.RegisterNpgSqlDbContexts<ApplicationDbContext>(connectionString);
    }
}
