using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR.Client;
using Serilog.Events;
using Serilog.Formatting.Compact.Reader;

namespace Melville.Log.Viewer.LogViews
{
    public class HubLogConnection : ILogConnection
    {
        private readonly HubConnection connection;
        private readonly IDisposable disposeToStopReadingEvents;
        public event EventHandler<LogEventArrivedEventArgs>? LogEventArrived;

        public HubLogConnection(string url)
        {
            connection = new HubConnectionBuilder()
                .WithUrl(url + "/MelvilleSpecialLoggingHubWellKnownUrl")
                .WithAutomaticReconnect()
                .Build();
            disposeToStopReadingEvents = connection.On("SendEvent", (Action<string>)HandleLogEvent);
            connection.StartAsync();
        }

        private void HandleLogEvent(string serializedEvent)
        {
            var eventReader = new LogEventReader(new StringReader(serializedEvent));
            while (eventReader.TryRead(out var logEvent))
            {
                LogEventArrived?.Invoke(this, new LogEventArrivedEventArgs(logEvent));
            }
        }

        public ValueTask SetDesiredLevel(LogEventLevel level) => 
            new ValueTask(connection.InvokeAsync("SetMinimumLogLevel", level));

        public void StopReading() => disposeToStopReadingEvents.Dispose();
    }
}