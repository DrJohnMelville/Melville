using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR.Client;
using Serilog.Events;
using Serilog.Parsing;

namespace Melville.Log.Viewer.LogViews
{
    public class LogEventArrivedEventArgs : EventArgs
    {
        public LogEvent LogEvent { get; }

        public LogEventArrivedEventArgs(LogEvent logEvent)
        {
            LogEvent = logEvent;
        }
    }
    public interface ILogConnection
    {
        ValueTask SetDesiredLevel(LogEventLevel level);
        event EventHandler<LogEventArrivedEventArgs>? LogEventArrived;
        void StopReading();
    }

    public class HubLogConnection : ILogConnection
    {
        private readonly HubConnection connection;
        private readonly IDisposable disposeToStopReadingEvents;
        public event EventHandler<LogEventArrivedEventArgs>? LogEventArrived;

        public HubLogConnection(string url)
        {
            connection = new HubConnectionBuilder()
                .WithUrl(url+"/Logging")
                .WithAutomaticReconnect()
                .Build();
            disposeToStopReadingEvents = connection.On("SendLogEvent", (Action<string>)HandleLogEvent);
            connection.StartAsync();
        }

        private void HandleLogEvent(string serializedEvent)
        {
            LogEventArrived?.Invoke(this, new LogEventArrivedEventArgs(
                new LogEvent(DateTimeOffset.Now, LogEventLevel.Information, null,
                    new MessageTemplate(serializedEvent, new MessageTemplateToken[0]),
                    new LogEventProperty[0])
                ));
        }

        public ValueTask SetDesiredLevel(LogEventLevel level) => 
            new ValueTask(connection.InvokeAsync("SetMinimumLogLevel", level));

        public void StopReading() => disposeToStopReadingEvents.Dispose();
    }
}