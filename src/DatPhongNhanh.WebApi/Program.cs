using DatPhongNhanh.WebApi.Extensions;

namespace DatPhongNhanh.WebApi;

public class Program
{
    public static async Task Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        builder.AddServices();
        var app = builder.Build();
        app.AddPipelines();

        await app.RunAsync();
    }
}