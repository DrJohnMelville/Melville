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
        /// <summary>
        /// The hub logger requires two configuration entries
        /// In Startup.ConFigureServices add:
        ///    services.AddLoggingHub();
        ///
        /// In Startup.Configure inside the call to App.UseEndpoints add
        ///             app.UseEndpoints(endpoints =>
        /// {
        ///       ...
        ///     endpoints.UseLoggingHub();
        /// });
        /// Additionally to make ASp.Net use Serilog add to Program.cs
        /// Host.CreateDefaultBuilder.UseSerilog(). ...
        /// </summary>
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

        /// <summary>
        /// The hub logger requires two configuration entries
        /// In Startup.ConFigureServices add:
        ///    srvices.AddLoggingHub();
        ///
        /// In Startup.Configure inside the call to App.UseEndpoints add
        ///             app.UseEndpoints(endpoints =>
        /// {
        ///       ...
        ///     endpoints.UseLoggingHub();
        /// });
        /// Additionally to make ASp.Net use Serilog add to Program.cs
        /// Host.CreateDefaultBuilder.UseSerilog(). ...
        /// </summary>
        public static void UseLoggingHub(this IEndpointRouteBuilder endpoints, 
            params string[] authorizationPolicyNames)
        {
            var hubLogEventSink = endpoints.ServiceProvider.GetRequiredService<HubLogEventSink>();
            Log.Logger = new LoggerConfiguration()
                .Enrich.FromLogContext()
                .WriteTo.Console()
                .WriteTo.Sink(hubLogEventSink, LogEventLevel.Information, hubLogEventSink.LevelSwitch)
                .CreateLogger();
            var hubEndpoint = endpoints.MapHub<LoggingHub>("/MelvilleSpecialLoggingHubWellKnownUrl");
            if (authorizationPolicyNames.Length > 0)
            {
                hubEndpoint.RequireAuthorization(authorizationPolicyNames);
            }
        }
    }
}