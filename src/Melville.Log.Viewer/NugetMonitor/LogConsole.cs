using System;
using System.Threading.Tasks;
using Melville.Log.Viewer.LogViews;
using Serilog.Events;

namespace Melville.Log.Viewer.NugetMonitor;

public interface ILogConsole
{
    void WriteToLog(string text, LogEventLevel level = LogEventLevel.Information);
    public event EventHandler<EventArgs>? WindowClosed;
}

public partial class LogConsole : ILogConsole, ILogConnection
{
    public void WriteToLog(string text, LogEventLevel level = LogEventLevel.Information) => 
        LogEventArrived.Write(this, text, level);

    public ValueTask SetDesiredLevel(LogEventLevel level) => ValueTask.CompletedTask;

    public event EventHandler<LogEventArrivedEventArgs>? LogEventArrived;
    public event EventHandler<EventArgs>? WindowClosed;
    public void StopReading()
    {
        WindowClosed?.Invoke(this, EventArgs.Empty);
    }
}