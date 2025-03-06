using DatPhongNhanh.Data.Configurations;
using DatPhongNhanh.Data.DbContexts.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace DatPhongNhanh.Data.PostgreSql.Extensions
{
    public static class DatabaseExtensions
    {
        public static void RegisterNpgSqlDbContexts<TAppDbContext>(this IServiceCollection services, ConnectionStringsConfiguration connectionStrings)
            where TAppDbContext : DbContext, IAppDbContext
        {
            services.AddDbContextPool<IAppDbContext, TAppDbContext>((_, opts)=>
            {
                opts.UseNpgsql(connectionStrings.AppConnectionString, options =>
                {
                    options.MigrationsAssembly(typeof(DatabaseExtensions).Assembly.FullName);
                });
            });
        }
    }
}
