using DatPhongNhanh.OAuth.Business.AppClaims;
using DatPhongNhanh.OAuth.Data.DbContexts;
using DatPhongNhanh.OAuth.Data.Entities.Identity;
using DatPhongNhanh.OAuth.Data.Entities.OpenIddict;
using DatPhongNhanh.OAuth.Data.Provider.Extensions;
using DatPhongNhanh.OAuth.SharedKernel.Configurations;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using OpenIddict.Abstractions;
using OpenIddict.Validation.AspNetCore;

namespace DatPhongNhanh.OAuth.Web;

public abstract class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddControllersWithViews();
        builder.Services.AddRazorPages();
        var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
        ArgumentException.ThrowIfNullOrWhiteSpace(connectionString);
        builder.Services.RegisterNpgSqlDbContexts<ApplicationDbContext>(connectionString);

        builder.Services.AddOpenIddict()
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
                options.RegisterScopes(new[]
                {
                    "api",
                    OpenIddictConstants.Scopes.OpenId,
                    OpenIddictConstants.Scopes.Email,
                    OpenIddictConstants.Scopes.Profile,
                    OpenIddictConstants.Scopes.Phone,
                    OpenIddictConstants.Scopes.Roles,
                    OpenIddictConstants.Scopes.Address,
                    OpenIddictConstants.Scopes.OfflineAccess
                });
            })
            .AddValidation(options =>
            {
                options.UseLocalServer();

                options.UseAspNetCore();
            });

        builder.Services.AddTransient<DefaultOpenIddictClaimsPrincipalHandler>();
        builder.Services.Configure<OpenIddictClaimsPrincipalOptions>(options =>
        {
            options.ClaimsPrincipalHandlers.Add<DefaultOpenIddictClaimsPrincipalHandler>();
        });
        builder.Services.AddScoped<OpenIddictClaimsPrincipalManager>();

        builder.Services.AddIdentity<ApplicationUser, ApplicationRole>(options =>
            {
            })
            .AddEntityFrameworkStores<ApplicationDbContext>()
            .AddDefaultTokenProviders();
        
        builder.Services.AddAuthentication(options =>
        {
            options.DefaultScheme = OpenIddictValidationAspNetCoreDefaults.AuthenticationScheme;
        });

        builder.Services.ConfigureApplicationCookie(options =>
        {
            options.LoginPath = "/account/login";
            options.Cookie.Name = "x_auth";
        });

        builder.Services.AddAntiforgery(options =>
        {
            options.Cookie.Name = "x_xsrf";
        });
        builder.Services.AddAuthorization();
        builder.Services.AddHostedService<SeedDataWorker>();
        
        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseExceptionHandler("/Home/Error");
            // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
            app.UseHsts();

            using var scope = app.Services.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            context.Database.EnsureCreated();
        }

        app.UseHttpsRedirection();
        app.UseRouting();
        app.UseAuthentication();
        app.UseAuthorization();
        app.MapStaticAssets();
        app.MapControllers();
        app.MapRazorPages();
        app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}")
            .WithStaticAssets();

        app.Run();
    }
}