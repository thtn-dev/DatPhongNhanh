using DatPhongNhanh.Application;
using DatPhongNhanh.Domain.User.Services;
using DatPhongNhanh.Shared;
using Newtonsoft.Json.Serialization;
using Newtonsoft.Json;

namespace DatPhongNhanh.WebApi.Extensions
{
    public static class ConfigServices
    {
        public static WebApplicationBuilder AddServices(this WebApplicationBuilder builder)
        {
            // Add services to the container.
            builder.Services.AddControllers()
                .AddNewtonsoftJson(options =>
                {
                    options.SerializerSettings.ContractResolver = new DefaultContractResolver();
                    options.SerializerSettings.Formatting = Formatting.Indented;
                    options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
                });

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.AddApplicationLayer();
            builder.Services.AddInfrastructureLayer(builder.Configuration);
            builder.Services.AddTransient<IDomainEventDispatcher, MediatRDomainEventDispatcher>();
            builder.Services.AddScoped<ICurrentUser, CurrentUser>();
            builder.Services.AddHttpContextAccessor();
            builder.Services.ConfigureCors();
            return builder;
        }

        private static void ConfigureCors(this IServiceCollection services)
        {
            services.AddCors(options =>
            {
                options.AddPolicy("DefaultDevCorsPolicy", builder =>
                {
                    // allow all
                    builder.AllowAnyOrigin()
                        .AllowAnyMethod()
                        .AllowAnyHeader();
                });

                options.AddPolicy("DefaultProdCorsPolicy", builder =>
                {
                    // allow only the specified origin
                    builder.WithOrigins("https://example.com")
                        .AllowAnyMethod()
                        .AllowAnyHeader();
                });
            });
        }
    }
}
