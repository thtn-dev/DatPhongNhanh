using DatPhongNhanh.Application.Common.Cache;
using DatPhongNhanh.Application.Common.Interfaces;
using DatPhongNhanh.Domain.Common.Options;
using DatPhongNhanh.Domain.Common.Services;
using DatPhongNhanh.Domain.Homestay.Services;
using DatPhongNhanh.Domain.User.Options;
using DatPhongNhanh.Domain.User.Services;
using DatPhongNhanh.Infrastructure.Cache;
using DatPhongNhanh.Infrastructure.Data.Contexts;
using DatPhongNhanh.Infrastructure.Data.Interceptors;
using DatPhongNhanh.Infrastructure.Homestay;
using DatPhongNhanh.Infrastructure.Services;
using DatPhongNhanh.Infrastructure.User;
using IdGen;
using IdGen.DependencyInjection;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Npgsql;
using System.Text;

namespace DatPhongNhanh.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructureLayer(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddJwt(configuration);
        services.AddEntityFramework(configuration);
        services.AddInfrasServices();
        services.AddCaching();
        services.AddIdGen();
        
        return services;
    }
    /// <summary>
    /// Configure Entity Framework
    /// </summary>
    /// <param name="services"></param>
    /// <param name="configuration"></param>
    private static void AddEntityFramework(this IServiceCollection services, IConfiguration configuration)
    {
        // Add Entity Framework
        var dbOptions = configuration.GetSection(DatabaseSetting.SettingKey).Get<DatabaseSetting>();
        ArgumentNullException.ThrowIfNull(dbOptions, nameof(DatabaseSetting));
        services.AddSingleton(dbOptions);
        services.AddTransient<ISaveChangesInterceptor, DateTrackingInterceptor>();
        services.AddTransient<ISaveChangesInterceptor, DispatchDomainEventsInterceptor>();

        services.AddDbContextPool<IApplicationDbContext, AppDbContext>((sp, options) =>
        {
            var connBuilder = new NpgsqlConnectionStringBuilder()
            {
                Host = dbOptions.Host,
                Port = dbOptions.Port,
                Username = dbOptions.Username,
                Password = dbOptions.Password,
                Database = dbOptions.Database,
                SslMode = dbOptions.SslMode == "Require" ? SslMode.Require : SslMode.Disable,
                Pooling = true,
                MaxPoolSize = 100,
                MinPoolSize = 10,
            };
            var conn = connBuilder.ToString();
            options.UseNpgsql(conn, builder =>
            {
                builder.MigrationsAssembly(typeof(AppDbContext).Assembly.FullName);
                builder.UseNetTopologySuite();
            });

            options.AddInterceptors(sp.GetServices<ISaveChangesInterceptor>());
        });

        services.AddScoped<IApplicationConnection>(sp => sp.GetRequiredService<AppDbContext>());
    }

    /// <summary>
    /// Configure AspNet Identity
    /// </summary>
    /// <param name="services"></param>
    /// <param name="configuration"></param>
    private static void AddJwt(this IServiceCollection services, IConfiguration configuration)
    {
        var jwtSetting = configuration.GetSection(JwtSetting.SettingKey).Get<JwtSetting>();
        ArgumentNullException.ThrowIfNull(jwtSetting, nameof(JwtSetting));
        services.AddSingleton(jwtSetting);

        services
            .AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.RequireHttpsMetadata = false;
                options.SaveToken = true;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = jwtSetting.Issuer,
                    ValidAudience = jwtSetting.Audience,
                    ClockSkew = TimeSpan.Zero,
                    RequireExpirationTime = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSetting.Secret)),
                };
            });
    }

    private static void AddIdGen(this IServiceCollection services)
    {
        services.AddIdGen(16, () =>
        {
            var epoc = new DateTime(2024, 11, 16, 3, 3, 3, DateTimeKind.Utc);
            var structure = IdStructure.Default;
            return new IdGeneratorOptions
            {
                TimeSource = new DefaultTimeSource(epoc),
                IdStructure = structure,
            };
        });
        services.AddSingleton<ISystemIdGenService, SystemIdGenService>();
    }

    private static void AddInfrasServices(this IServiceCollection services)
    {
        services.AddScoped<IJwtService, JwtService>();
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<IPasswordHasher, PasswordHasher>();
        services.AddScoped<IUserNormalize, UserNormalize>();
        services.AddSingleton(sp => PasswordHasherOptions.Default);
        services.AddScoped<IHomestayService, HomestayService>();
    }

    private static void AddCaching(this IServiceCollection services)
    {
        services.AddScoped<ICachePolicy, DefaultCachePolicy>();
        services.AddMemoryCache();
        services.AddScoped<ICacheService, MemoryCacheService>();
    }
}