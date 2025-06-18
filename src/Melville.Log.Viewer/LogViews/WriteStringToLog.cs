using System;
using Melville.INPC;
using Microsoft.Extensions.Logging;

namespace Melville.Log.Viewer.LogViews;

public static class WriteStringToLog
{
    public static void Write(this EventHandler<LogEventArrivedEventArgs>? handler,
        object? sender, string content, LogLevel level = LogLevel.Information) =>
        handler?.Invoke(sender, new LogEventArrivedEventArgs(new StringLogEvent(
            DateTimeOffset.Now, level, content)));
}

public partial class StringLogEvent : LogEvent
{
    [FromConstructor] public override string Text => "";
}