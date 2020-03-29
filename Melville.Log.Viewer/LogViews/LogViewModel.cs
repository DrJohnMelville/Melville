using System.IO;
using System.Threading.Tasks;
using Melville.Log.Viewer.HomeScreens;
using Melville.Log.Viewer.NamedPipeServers;
using Serilog.Events;
using Serilog.Formatting.Compact.Reader;

namespace Melville.Log.Viewer.LogViews
{
    public class LogViewModel: IHomeScreenPage
    {
        public string Title { get; set; } = "Log Window";
        private readonly Stream logConnection;

        public LogViewModel(Stream logConnection)
        {
            this.logConnection = logConnection;
        }
        
        public void Emit(LogEvent logEvent)
        {
//            LastLogEntry = logEvent.MessageTemplate.Render(logEvent.Properties);
        }

        private void ClientConnected(object? sender, NewPipeConnectionEventArgs e)
        {
            Task.Run(() =>
            {
                var reader = new LogEventReader(new StreamReader(e.ClientConnection));
                while (reader.TryRead(out var logEvent))
                {
                    Emit(logEvent);
                }
            });
        }

    }
}