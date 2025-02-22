using Asp.Versioning;
using Asp.Versioning.ApiExplorer;
using DatPhongNhanh.WebApi.Extensions;
using NSwag.AspNetCore;
using Serilog;
using Serilog.Events;

namespace DatPhongNhanh.WebApi
{
    public class Program
    {
        public static readonly List<ApiVersion> Versions = [new ApiVersion(1, 0)];

        public static async Task Main(string[] args)
        {
            try
            {
                var builder = WebApplication.CreateBuilder(args);
                builder.Host.ConfigAppHost();
                builder.Services.AddApplicationServices(builder.Configuration);
                var app = builder.Build();
                ConfigPipelines(app);
                await app.RunAsync();
            }
            finally
            {

            }
        }

        private static WebApplication ConfigPipelines(WebApplication app)
        {
            if (app.Environment.IsDevelopment())
            {
                app.UseSerilogRequestLogging(options =>
                    {
                        options.MessageTemplate = "HTTP {RequestMethod} {RequestPath} responded {StatusCode} in {Elapsed:0.0000} ms";

                        // Customize the logging level
                        options.GetLevel = (httpContext, elapsed, ex) => LogEventLevel.Information;

                        // Enrich the logging message
                        options.EnrichDiagnosticContext = (diagnosticContext, httpContext) =>
                        {
                            diagnosticContext.Set("RequestHost", httpContext.Request.Host.Value!);
                            diagnosticContext.Set("RequestScheme", httpContext.Request.Scheme);
                            diagnosticContext.Set("UserAgent", httpContext.Request.Headers["User-Agent"].FirstOrDefault()!);
                        };
                    });
            }
                
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
            return app;
        }
    }
}
