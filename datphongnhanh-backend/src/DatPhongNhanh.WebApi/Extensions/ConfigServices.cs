using Asp.Versioning;
using DatPhongNhanh.WebApi.AuthenticationHandler;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using NSwag.Generation.Processors.Security;
using NSwag;
using DatPhongNhanh.Data.Configurations;
using DatPhongNhanh.Data.DbContexts;
using DatPhongNhanh.Data.PostgreSql.Extensions;
using DatPhongNhanh.BusinessLogic;
using DatPhongNhanh.Data.Repositories.Interfaces;
using DatPhongNhanh.Data.Repositories;
using DatPhongNhanh.SharedKernel;
using FluentValidation.AspNetCore;
using FluentValidation;
using DatPhongNhanh.BusinessLogic.User.Interfaces;
using DatPhongNhanh.BusinessLogic.User;

namespace DatPhongNhanh.WebApi.Extensions
{
    public static class ConfigServices
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration configuration)
        {
            // Add services to the container.
            services.AddControllers();
            services.AddHttpClient();
            services.AddAuthentication("GoogleAuth")
                .AddScheme<AuthenticationSchemeOptions, GoogleAuthenticationHandler>("GoogleAuth", null);

            services.AddAuthorization(options =>
            {
                var defaultPolicy = new AuthorizationPolicyBuilder()
                    .AddAuthenticationSchemes("GoogleAuth")
                    .RequireAuthenticatedUser()
                    .Build();

                options.DefaultPolicy = defaultPolicy;
            });

            services.ConfigureOpenApiSwagger();
            services.ConfigDataAccess(configuration);
            services.AddValidators();
            services.AddBusinessLogic();


            return services;
        }

        private static void AddValidators(this IServiceCollection services)
        {
            services.AddFluentValidationAutoValidation();
            services.AddValidatorsFromAssemblyContaining<BusinessAssemblyRef>();
        }


        private static void ConfigureOpenApiSwagger(this IServiceCollection services)
        {
            services.AddApiVersioning(options =>
            {
                options.ReportApiVersions = true;
                options.AssumeDefaultVersionWhenUnspecified = true;
                options.DefaultApiVersion =  Program.Versions.First();
                options.ApiVersionReader = new UrlSegmentApiVersionReader();
            })
                .AddApiExplorer(options =>
                {
                    options.GroupNameFormat = "'v'VVV";
                    options.SubstituteApiVersionInUrl = true;
                    options.AddApiVersionParametersWhenVersionNeutral = true;
                    options.AssumeDefaultVersionWhenUnspecified = true;
                    options.DefaultApiVersion = Program.Versions.First();
                });

            foreach (var version in Program.Versions)
            {
                services.AddOpenApiDocument(config =>
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

                    });

                    config.OperationProcessors.Add(new AspNetCoreOperationSecurityScopeProcessor("OAuth2"));

                });
            }
        }

        private static void ConfigDataAccess(this IServiceCollection services, IConfiguration configuration)
        {
            var connectionStrings = configuration.GetSection(nameof(ConnectionStringsConfiguration)).Get<ConnectionStringsConfiguration>();
            ArgumentNullException.ThrowIfNull(connectionStrings, nameof(connectionStrings));
            services.RegisterNpgSqlDbContexts<AppDbContext>(connectionStrings);

            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IUserRepository, UserRepository>();
        }

        private static void AddBusinessLogic(this IServiceCollection services)
        {
            services.AddScoped<IUserService, UserService>();
        }

    }
}
