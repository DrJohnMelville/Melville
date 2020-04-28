using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Serilog.Events;
using Serilog.Formatting.Compact.Reader;

namespace Melville.Log.Viewer.LogViews
{
    public class StreamLogConnection : ILogConnection
    {
        private CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
        private readonly Stream logConnection;
        public event EventHandler<LogEventArrivedEventArgs>? LogEventArrived;

        public StreamLogConnection(Stream logConnection)
        {
            this.logConnection = logConnection;
            ReadEventsLoop();
        }

        public void StopReading() => cancellationTokenSource.Cancel();
        private async void ReadEventsLoop()
        {
            var waitStream = new AsyncToSyncProgressStream(logConnection, cancellationTokenSource.Token);
            var reader = new LogEventReader(new StreamReader(waitStream));
            while (await waitStream.WaitForData())
            {
                if (reader.TryRead(out var logEvent))
                {
                    HandleEvent(logEvent);
                }
            }
        }
        private void HandleEvent(LogEvent logEvent) => 
            LogEventArrived?.Invoke(this, new LogEventArrivedEventArgs(logEvent));


        public ValueTask SetDesiredLevel(LogEventLevel level) => 
            logConnection.WriteAsync(new byte[] {(byte) level});
    }
}