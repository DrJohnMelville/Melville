using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Serilog.Core;
using Serilog.Events;
using Serilog.Formatting;
using Serilog.Formatting.Compact;

namespace LoggingTestWebsite.Controllers
{
    public interface ILoggingHub
    {
        Task SendEvent(string eventData);
    }
    public sealed class LoggingHub: Hub<ILoggingHub>
    {
        private readonly INotifyEvent<LogEventLevel> notifier;

        public LoggingHub(INotifyEvent<LogEventLevel> notifier)
        {
            this.notifier = notifier;
        }

        public void SetMinimumLogLevel(LogEventLevel minimumLevel)
        {
            notifier.Fire(minimumLevel);
        }
    }

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

        public void Emit(LogEvent logEvent)
        {
            using var write = new StringWriter();
            logEventFormatter.Format(logEvent, write);
            var s = write.ToString();
            loggingHub.Clients.All.SendEvent(s);
        }
    }
}