using System;
using System.Threading.Tasks;
using Melville.INPC;
using Microsoft.Extensions.Logging;

namespace Melville.Log.Viewer.LogViews;

public abstract partial class LogEvent
{
    [FromConstructor] public DateTimeOffset TimeStamp { get; }
    [FromConstructor]public LogLevel Level {get;}
    public abstract string Text { get; }
}

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
    ValueTask SetDesiredLevel(LogLevel level);
    event EventHandler<LogEventArrivedEventArgs>? LogEventArrived;
    void StopReading();
}