using System;
using System.Windows.Input;
using Serilog;
using Serilog.Configuration;
using Serilog.Core;
using Serilog.Events;

namespace WebDashboard.Startup
{
  public class DynamicLogSink: ILogEventSink
  {
    private readonly LogEventLevel filteredLevel;
    private readonly ILogEventSink innerSink;

    public DynamicLogSink(ILogEventSink innerSink, LogEventLevel filteredLevel = LogEventLevel.Information)
    {
      this.innerSink = innerSink;
      this.filteredLevel = filteredLevel;
    }

    public void Emit(LogEvent logEvent)
    {
      if (logEvent.Level >= filteredLevel || Keyboard.PrimaryDevice.IsKeyToggled(Key.Scroll))
      {
        innerSink.Emit(logEvent);
      }
    }
  }

  public static class LoggerConfigurationAsyncExtensions

  {
    /// <summary>
    /// Configure a sink to be invoked asynchronously, on a background worker thread.
    /// </summary>
    /// <param name="loggerSinkConfiguration">The <see cref="LoggerSinkConfiguration"/> being configured.</param>
    /// <param name="configure">An action that configures the wrapped sink.</param>
    /// <param name="bufferSize">The size of the concurrent queue used to feed the background worker thread. If
    /// the thread is unable to process events quickly enough and the queue is filled, depending on
    /// <paramref name="blockWhenFull"/> the queue will block or subsequent events will be dropped until
    /// room is made in the queue.</param>
    /// <param name="blockWhenFull">Block when the queue is full, instead of dropping events.</param>
    /// <returns>A <see cref="LoggerConfiguration"/> allowing configuration to continue.</returns>
    public static LoggerConfiguration Dynamic(
      this LoggerSinkConfiguration loggerSinkConfiguration,
      Action<LoggerSinkConfiguration> configure,
      LogEventLevel upperLevel = LogEventLevel.Information)
    {
      return LoggerSinkConfiguration.Wrap(loggerSinkConfiguration,
        i => new DynamicLogSink(i, upperLevel), configure, LevelAlias.Minimum, null);
    }
  }
}