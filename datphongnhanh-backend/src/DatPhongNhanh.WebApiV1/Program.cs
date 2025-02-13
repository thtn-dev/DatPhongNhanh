
using DatPhongNhanh.Data.Configurations;
using DatPhongNhanh.Data.DbContexts;
using DatPhongNhanh.Data.PostgreSql.Extensions;

namespace DatPhongNhanh.WebApiV1
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var connectionStrings = builder.Configuration.GetSection(nameof(ConnectionStringsConfiguration)).Get<ConnectionStringsConfiguration>();
            ArgumentNullException.ThrowIfNull(connectionStrings, nameof(connectionStrings));
            builder.Services.RegisterNpgSqlDbContexts<AppDbContext>(connectionStrings);

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
