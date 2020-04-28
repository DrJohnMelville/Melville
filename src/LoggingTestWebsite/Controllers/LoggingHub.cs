using System.IO;
using System.Threading.Tasks;
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
        public Task SetMinimumLogLevel(LogEventLevel minimumLevel)
        {
//            return Clients.All.SendAsync("SendLogEvent", "Log EventData");
            return Task.CompletedTask;
        }
        
    }

    public sealed class HubLogEventSink: ILogEventSink
    {
        private readonly ITextFormatter logEventFormatter = new CompactJsonFormatter();
        private readonly IHubContext<LoggingHub, ILoggingHub> loggingHub;

        public HubLogEventSink(IHubContext<LoggingHub, ILoggingHub> loggingHub)
        {
            this.loggingHub = loggingHub;
        }

        public void Emit(LogEvent logEvent)
        {
            using var write = new StringWriter();
            logEventFormatter.Format(logEvent, write);
            var s = write.ToString();
            loggingHub.Clients.All.SendEvent(s);
        }
    }
}