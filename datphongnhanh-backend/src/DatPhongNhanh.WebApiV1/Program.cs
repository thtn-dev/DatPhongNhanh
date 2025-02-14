using Asp.Versioning;
using Asp.Versioning.ApiExplorer;
using DatPhongNhanh.BusinessLogic;
using DatPhongNhanh.BusinessLogic.Services;
using DatPhongNhanh.BusinessLogic.Services.Interfaces;
using DatPhongNhanh.Data.Configurations;
using DatPhongNhanh.Data.DbContexts;
using DatPhongNhanh.Data.PostgreSql.Extensions;
using DatPhongNhanh.Data.Repositories;
using DatPhongNhanh.Data.Repositories.Interfaces;
using DatPhongNhanh.WebApiV1.AuthenticationHandler;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using NSwag;
using NSwag.AspNetCore;
using NSwag.Generation.Processors.Security;

namespace DatPhongNhanh.WebApiV1
{
    public class Program
    {
        public static List<ApiVersion> Versions = [new ApiVersion(1, 0), new ApiVersion(2, 0)];

        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllers();

            builder.Services.AddHttpClient();
            builder.Services.AddAuthentication("GoogleAuth")
                .AddScheme<AuthenticationSchemeOptions, GoogleAuthenticationHandler>("GoogleAuth", null);

            builder.Services.AddAuthorization(options =>
            {
                var defaultPolicy = new AuthorizationPolicyBuilder()
                    .AddAuthenticationSchemes("GoogleAuth")
                    .RequireAuthenticatedUser()
                    .Build();

                options.DefaultPolicy = defaultPolicy;
            });


            builder.Services.AddApiVersioning(options =>
            {
                options.ReportApiVersions = true;
                options.AssumeDefaultVersionWhenUnspecified = true;
                options.DefaultApiVersion = Versions.First();
                options.ApiVersionReader = new UrlSegmentApiVersionReader();
            })
                .AddApiExplorer(options =>
                {
                    options.GroupNameFormat = "'v'VVV";
                    options.SubstituteApiVersionInUrl = true;
                    options.AddApiVersionParametersWhenVersionNeutral = true;
                    options.AssumeDefaultVersionWhenUnspecified = true;
                    options.DefaultApiVersion = Versions.First();
                });

            foreach (var version in Versions)
            {
                builder.Services.AddOpenApiDocument(config =>
                {
                    string strVer = "v" + version.MajorVersion;
                    config.Title = $"API DatPhongNhanh {strVer}";
                    config.Description = $"API DatPhongNhanh {strVer}";
                    config.Version = strVer;
                    config.DocumentName = strVer;
                    config.ApiGroupNames = [strVer];
                    config.PostProcess = document =>
                    {
                        document.Info.Version = strVer;
                        document.Info.Title = $"API DatPhongNhanh {strVer}";
                        document.Info.Description = $"API DatPhongNhanh {strVer}";
                    };

                    config.AddSecurity("OAuth2", new OpenApiSecurityScheme
                    {
                        Type = OpenApiSecuritySchemeType.OAuth2,
                        Flow = OpenApiOAuth2Flow.Implicit,
                        TokenUrl = "https://oauth2.googleapis.com/token",
                        AuthorizationUrl = "https://accounts.google.com/o/oauth2/v2/auth",
                        Scopes = new Dictionary<string, string>
                        {
                            { "openid", "OpenID Connect" },
                            { "profile", "Access user profile" },
                            { "email", "Access user email" }
                        }
                        //Flows = new OpenApiOAuthFlows
                        //{
                        //    Implicit = new OpenApiOAuthFlow
                        //    {
                        //        AuthorizationUrl = "https://accounts.google.com/o/oauth2/v2/auth",
                        //        Scopes = new Dictionary<string, string>
                        //        {
                        //            { "openid", "OpenID Connect" },
                        //            { "profile", "Access user profile" },
                        //            { "email", "Access user email" }
                        //        }
                        //    }
                        //}

                    });

                    config.OperationProcessors.Add(new AspNetCoreOperationSecurityScopeProcessor("OAuth2"));

                });
            }


            var connectionStrings = builder.Configuration.GetSection(nameof(ConnectionStringsConfiguration)).Get<ConnectionStringsConfiguration>();
            ArgumentNullException.ThrowIfNull(connectionStrings, nameof(connectionStrings));
            builder.Services.RegisterNpgSqlDbContexts<AppDbContext>(connectionStrings);

            builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
            builder.Services.AddScoped<IUserRepository, UserRepository>();
            builder.Services.AddScoped<IUserService, UserService>();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseOpenApi();
                app.UseSwaggerUi(settings =>
                {
                    IApiVersionDescriptionProvider apiVersionDescriptionProvider = app.Services.GetRequiredService<IApiVersionDescriptionProvider>();
                    foreach (var description in apiVersionDescriptionProvider.ApiVersionDescriptions)
                    {
                        settings.SwaggerRoutes.Add(new SwaggerUiRoute(
                            $"API DatPhongNhanh {description.GroupName}",
                            $"/swagger/{description.GroupName}/swagger.json"));
                    }

                    settings.OAuth2Client = new OAuth2ClientSettings
                    {
                        AppName = "DatPhongNhanh",
                        Scopes = { "openid", "profile", "email" },
                    };
                });
            }

            app.UseHttpsRedirection();
            app.UseAuthentication();
            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
