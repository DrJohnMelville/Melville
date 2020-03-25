using System;
using AspNetCoreLocalLog.LoggingMiddleware;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using Serilog.Configuration;
using Serilog.Core;
using Serilog.Events;
using Serilog.Sinks.SystemConsole.Themes;

namespace AspNetCoreLocalLog.LogSink
{
  public static class VolitileWriterSinkExtensions
  {
    // public static LoggerConfiguration VolitileSink(this LoggerSinkConfiguration lsc,
    //   ICircularMemorySink target) =>
    //   lsc.Sink(new VolitileSerilogSink(target));

    public static void AddLogRetrieval(this IServiceCollection servicies)
    {
      servicies.AddSingleton<VolitileSerilogSink>();
      servicies.AddSingleton<ICircularMemorySink, CircularMemorySink>();
      servicies.AddSingleton<LogRetrievalEndpoint>();
      servicies.AddSingleton<IRetrieveLog, RetrieveLog>();
    }
    public static IConfigureLogRetrieval UseLogRetrieval(this IApplicationBuilder builder)
    {
      GC.KeepAlive(builder.ApplicationServices.GetService<ILogger>()); // make sure the static is initalized
      return AddLogRetrievaliddleware(builder);
    }

    public static void AddSerilogLogger(this IServiceCollection services, Action<LoggerConfiguration>? configureLogger)
    {
      services.AddSingleton<ILogger>(isp =>
      {
        var factory = new LoggerConfiguration();
        configureLogger?.Invoke(factory);
        Log.Logger = factory
          .WriteTo.Console(
            outputTemplate:
            "[{Timestamp:HH:mm:ss} {Level}] {SourceContext}{NewLine}{Message:lj}{NewLine}{Exception}{NewLine}",
            theme: AnsiConsoleTheme.Literate)
          .WriteTo.Sink(isp.GetService<VolitileSerilogSink>())
          .CreateLogger();
        return Log.Logger;
      });
    }

    private static IConfigureLogRetrieval AddLogRetrievaliddleware(IApplicationBuilder builder)
    {
      var ret = builder.ApplicationServices.GetService<LogRetrievalEndpoint>();
      builder.Use(ret.Process);
      return ret;
    }
  }
}