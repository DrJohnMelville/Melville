using System;
using Serilog.Events;
using Serilog.Parsing;

namespace Melville.Log.Viewer.LogViews;

public static class WriteStringToLog
{
    public static void Write(this EventHandler<LogEventArrivedEventArgs>? handler,
        object? sender, string content, LogEventLevel level = LogEventLevel.Information) =>
        handler?.Invoke(sender, new LogEventArrivedEventArgs(new LogEvent(
            DateTimeOffset.Now, level, null, new MessageTemplateParser().Parse(content), [])));
}