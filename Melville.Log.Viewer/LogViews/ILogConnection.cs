using System.Collections.Generic;
using System.Threading.Tasks;
using Serilog.Events;

namespace Melville.Log.Viewer.LogViews
{
    public interface ILogConnection
    {
        Task SetDesiredLevel(LogEventLevel level);
        IAsyncEnumerable<LogEvent> ReadEvents();
    }
    public class StreamLogConnection : ILogConnection
    {
        
    }
}