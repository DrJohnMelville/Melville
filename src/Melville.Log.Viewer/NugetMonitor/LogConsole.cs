using System;
using System.Threading.Tasks;
using Melville.Log.Viewer.LogViews;
using Microsoft.Extensions.Logging;

namespace Melville.Log.Viewer.NugetMonitor;

public interface ILogConsole
{
    void WriteToLog(string text, LogLevel level = LogLevel.Information);
    public event EventHandler<EventArgs>? WindowClosed;
}

public partial class LogConsole : ILogConsole, ILogConnection
{
    public void WriteToLog(string text, LogLevel level = LogLevel.Information) => 
        LogEventArrived.Write(this, text, level);

    public ValueTask SetDesiredLevel(LogLevel level) => ValueTask.CompletedTask;

    public event EventHandler<LogEventArrivedEventArgs>? LogEventArrived;
    public event EventHandler<EventArgs>? WindowClosed;
    public void StopReading()
    {
        WindowClosed?.Invoke(this, EventArgs.Empty);
    }
}