using System.Linq;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using Serilog.Events;

namespace AspNetCoreLocalLog.HubLog
{
    public static class ConfigureHubLog
    {
        public static void AddLoggingHub(this IServiceCollection services)
        {
            services.AddSignalR();
            services.AddResponseCompression(opts =>
            {
                opts.MimeTypes = ResponseCompressionDefaults.MimeTypes.Concat(
                    new []{"application/octet-stream"});
            });
            services.AddSingleton<INotifyEvent<LogEventLevel>,NotifyEvent<LogEventLevel>>();
            services.AddTransient<HubLogEventSink, HubLogEventSink>();
            services.AddTransient<ILogger>(isp => Log.Logger);
        }

        public static void UseLoggingHub(this IEndpointRouteBuilder endpoints)
        {
            var hubLogEventSink = endpoints.ServiceProvider.GetRequiredService<HubLogEventSink>();
            Log.Logger = new LoggerConfiguration()
                .Enrich.FromLogContext()
                .WriteTo.Console()
                .WriteTo.Sink(hubLogEventSink, LogEventLevel.Information, hubLogEventSink.LevelSwitch)
                .CreateLogger();
            endpoints.MapHub<LoggingHub>("/MelvilleSpecialLoggingHubWellKnownUrl");

        }
    }
}