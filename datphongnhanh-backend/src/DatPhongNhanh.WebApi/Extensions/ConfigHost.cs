using Serilog;
using Serilog.Events;
using Serilog.Sinks.SystemConsole.Themes;

namespace DatPhongNhanh.WebApi.Extensions
{
    public static class ConfigHost
    {
        public static void ConfigAppHost(this IHostBuilder host)
        {
            host.UseSerilog((ctx, cfg) =>
            {
                cfg.ReadFrom.Configuration(ctx.Configuration)
                .MinimumLevel.Information()
                .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
                .MinimumLevel.Override("Microsoft.AspNetCore", LogEventLevel.Warning)
                .Enrich.FromLogContext()
                .Enrich.WithProperty("ApplicationName", "DatPhongNhanh.WebApi")
                .WriteTo.Console(
                    outputTemplate: "[{Timestamp:HH:mm:ss} {Level:u3}] [{SourceContext}] {Message:lj}{NewLine}{Exception}",
                    theme: AnsiConsoleTheme.Code)
                 .WriteTo.File(
                    path: "logs/datphongnhanh-.log",
                    rollingInterval: RollingInterval.Day,
                    outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} [{Level:u3}] [{SourceContext}] {Message:lj}{NewLine}{Exception}");
            });
        }
    }
}
