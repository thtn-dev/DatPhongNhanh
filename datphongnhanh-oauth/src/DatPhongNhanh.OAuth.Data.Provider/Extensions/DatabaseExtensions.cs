using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace DatPhongNhanh.OAuth.Data.Provider.Extensions;

public static class DatabaseExtensions
{
    public static void RegisterNpgSqlDbContexts<TAppDbContext>(this IServiceCollection services,
        string connectionString)
        where TAppDbContext : DbContext
    {
        services.AddDbContextPool<DbContext, TAppDbContext>((_, opts) =>
        {
            opts.UseNpgsql(connectionString,
                options => { options.MigrationsAssembly(typeof(MigrationAssembly).Assembly.FullName); });
        });
    }
}