using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using Serilog.Events;

namespace LoggingTestWebsite.Controllers
{
    public class LoggingHub: Hub
    {
        public Task SetMinimumLogLevel(LogEventLevel minimumLevel)
        {
            return Clients.All.SendAsync("SendLogEvent", "Log EventData");
        }
    }
}