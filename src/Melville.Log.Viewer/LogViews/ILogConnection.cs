using System;
using System.Threading.Tasks;
using Serilog.Events;

namespace Melville.Log.Viewer.LogViews;

public class LogEventArrivedEventArgs : EventArgs
{
    public LogEvent LogEvent { get; }

    public LogEventArrivedEventArgs(LogEvent logEvent)
    {
        LogEvent = logEvent;
    }
}
public interface ILogConnection
{
    ValueTask SetDesiredLevel(LogEventLevel level);
    event EventHandler<LogEventArrivedEventArgs>? LogEventArrived;
    void StopReading();
}