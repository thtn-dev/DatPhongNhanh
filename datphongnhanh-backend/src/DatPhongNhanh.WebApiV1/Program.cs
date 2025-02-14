
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
using NSwag;
using NSwag.AspNetCore;
using NSwag.Generation.Processors;

namespace DatPhongNhanh.WebApiV1
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllers();

            builder.Services.AddApiVersioning(options =>
            {
                options.ReportApiVersions = true;
                options.AssumeDefaultVersionWhenUnspecified = true;
                options.DefaultApiVersion = new ApiVersion(1, 0);
                options.ApiVersionReader = new UrlSegmentApiVersionReader();
            })
                .AddApiExplorer(options =>
                {
                    options.GroupNameFormat = "'v'VVV";
                    options.SubstituteApiVersionInUrl = true;
                    options.AddApiVersionParametersWhenVersionNeutral = true;
                    options.AssumeDefaultVersionWhenUnspecified = true;
                });

            builder.Services.AddOpenApiDocument(config =>
            {
                config.DocumentName = "v1";
                config.Version = "1.0";
                config.Title = "My API V1";
                config.Description = "API Documentation for Version 1.0";
                config.ApiGroupNames = ["v1"];

            });

            //builder.Services.AddOpenApiDocument(config =>
            //{
            //    config.DocumentName = "v2";
            //    config.ApiGroupNames = ["v2"] ;
            //    config.Title = "API v2";
            //    config.Version = "2.0";
            //});
            //var buildServiceProvider = builder.Services.BuildServiceProvider();
            //var apiVersionDescriptionProvider = buildServiceProvider.GetRequiredService<IApiVersionDescriptionProvider>();

            //foreach (var apiVersionDescription in apiVersionDescriptionProvider.ApiVersionDescriptions)
            //{
            //    builder.Services.AddSwaggerDocument(config =>
            //    {
            //        config.DocumentName = "v" + apiVersionDescription.ApiVersion.ToString();
            //        config.Version = apiVersionDescription.ApiVersion.ToString();
            //        config.PostProcess = document =>
            //        {
            //            document.Info.Title = "My API V" + apiVersionDescription.ApiVersion.ToString();
            //            document.Info.Description = "API Documentation for Version " + apiVersionDescription.ApiVersion.ToString();
            //            document.Info.Version = apiVersionDescription.ApiVersion.ToString();
            //        };
            //        config.ApiGroupNames = new[] { apiVersionDescription.ApiVersion.ToString() };
            //    });
            //}

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
                app.UseOpenApi(options =>
                {
                });


                app.UseSwaggerUi(settings =>
                {
                    IApiVersionDescriptionProvider apiVersionDescriptionProvider = app.Services.GetRequiredService<IApiVersionDescriptionProvider>();
                    //settings.SwaggerRoutes.Add(new NSwag.AspNetCore.SwaggerUiRoute("v1", "/swagger/v1/swagger.json"));
                    //settings.SwaggerRoutes.Add(new NSwag.AspNetCore.SwaggerUiRoute("v2", "/swagger/v2/swagger.json"));
                    foreach (var description in apiVersionDescriptionProvider.ApiVersionDescriptions)
                    {
                        settings.SwaggerRoutes.Add(new SwaggerUiRoute(
                            $"API {description.GroupName}",
                            $"/swagger/{description.GroupName}/swagger.json"));

                    }

                });


                //app.UseReDoc(options =>
                //{
                //    options.Path = "/redoc";
                //});
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
