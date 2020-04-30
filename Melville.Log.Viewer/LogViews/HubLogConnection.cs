using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR.Client;
using Serilog.Events;
using Serilog.Formatting.Compact.Reader;
using TokenServiceClient.Native;
using TokenServiceClient.Native.PersistentToken;

namespace Melville.Log.Viewer.LogViews
{
    public class FakeAccessToken: IPersistentAccessToken
    {
        public ValueTask<AccessTokenHolder> CurrentAccessToken()
        {
            return new ValueTask<AccessTokenHolder>(new AccessTokenHolder("", DateTime.Now.AddDays(10),""));
        }
    }
    public class HubLogConnection : ILogConnection
    {
        private readonly HubConnection connection;
        private readonly IDisposable disposeToStopReadingEvents;
        public event EventHandler<LogEventArrivedEventArgs>? LogEventArrived;
        private IPersistentAccessToken token = new FakeAccessToken();
         
        public HubLogConnection(string url)
        {
            connection = new HubConnectionBuilder()
                .WithUrl(url.TrimEnd('/','\\') + "/MelvilleSpecialLoggingHubWellKnownUrl",
                    o=> o.AccessTokenProvider = GetAccessToken)
                .WithAutomaticReconnect()
                .Build();
            disposeToStopReadingEvents = connection.On("SendEvent", (Action<string>)HandleLogEvent);
            ConnectToHub();
        }

        private async Task<string> GetAccessToken() => 
            (await token.CurrentAccessToken()).AccessToken;

        private async void ConnectToHub()
        {
            for (int i = 0; i < 2; i++)
            {
                try
                {
                    await connection.StartAsync();
                    return;
                 }
                catch (InvalidDataException)
                {
                    token = CapWebTokenFactory.CreateCapWebClient("CapWeb",
                        "7v0ehQkQOsWuzx9bT7hcQludASvUFcD5l5JEdkNDPaM");
                }
                
            }
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