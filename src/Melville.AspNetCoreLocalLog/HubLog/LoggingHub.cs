using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using Serilog.Events;

namespace AspNetCoreLocalLog.HubLog
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
        // this method is called dynamically by the client.
        public void SetMinimumLogLevel(LogEventLevel minimumLevel)
        {
            notifier.Fire(minimumLevel);
        }
    }
}