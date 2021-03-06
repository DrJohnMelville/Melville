﻿using System.IO;
using Microsoft.AspNetCore.SignalR;
using Serilog.Core;
using Serilog.Events;
using Serilog.Formatting;
using Serilog.Formatting.Compact;

namespace AspNetCoreLocalLog.HubLog
{
    public sealed class HubLogEventSink: ILogEventSink
    {
        private readonly ITextFormatter logEventFormatter = new CompactJsonFormatter();
        private readonly IHubContext<LoggingHub, ILoggingHub> loggingHub;
        public LoggingLevelSwitch LevelSwitch { get; }= new LoggingLevelSwitch();

        public HubLogEventSink(IHubContext<LoggingHub, ILoggingHub> loggingHub,
            INotifyEvent<LogEventLevel> notifier)
        {
            notifier.Notify += LogEventLevelChanged;
            this.loggingHub = loggingHub;
        }

        private void LogEventLevelChanged(object? sender, NotifyEventArgs<LogEventLevel> e) => 
            LevelSwitch.MinimumLevel = e.EventData;

        public void Emit(LogEvent logEvent) => 
            loggingHub.Clients.All.SendEvent(SerializeLogEvent(logEvent));

        private string SerializeLogEvent(LogEvent logEvent)
        {
            using var write = new StringWriter();
            logEventFormatter.Format(logEvent, write);
            return write.ToString();
        }
    }
}