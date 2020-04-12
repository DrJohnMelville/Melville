using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Threading;
using Melville.Log.Viewer.HomeScreens;
using Melville.Log.Viewer.NamedPipeServers;
using Melville.MVVM.AdvancedLists;
using Melville.MVVM.BusinessObjects;
using Melville.MVVM.FileSystem;
using Serilog.Events;
using Serilog.Formatting.Compact.Reader;

namespace Melville.Log.Viewer.LogViews
{
    public class LogViewModel : NotifyBase, IHomeScreenPage
    {
        private string title = "<Not Connected>";
        public string Title
        {
            get => title;
            set => AssignAndNotify(ref title, value);
        }

        private CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
        private LogEventLevel minimimLevel = LogEventLevel.Information;
        public LogEventLevel MinimumLevel
        {
            get => minimimLevel;
            set
            {
                if (AssignAndNotify(ref minimimLevel, value))
                {
                    SendDesiredLevelToSink();
                }
            }
        }

        private void SendDesiredLevelToSink() => logConnection.WriteAsync(new byte[] {(byte) MinimumLevel});

        private readonly Stream logConnection;
        public ICollection<LogEntryViewModel> Events { get; } = new ThreadSafeBindableCollection<LogEntryViewModel>();

        public LogViewModel(Stream logConnection)
        {
            this.logConnection = logConnection;
            ClientConnected();
        }

        private async void ClientConnected()
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

        private void HandleEvent(LogEvent logEvent)
        {
            if (IsPrecessNameMessage(logEvent, out var value))
            {
                Title = value.ToString()[1..^1];
            }
            else
            {
                Events.Add(new LogEntryViewModel(logEvent));
            }
        }

        private static bool IsPrecessNameMessage(LogEvent logEvent, out LogEventPropertyValue value) =>
            logEvent.Properties.TryGetValue("AssignProcessName", out value);

        public void Stop()
        {
            cancellationTokenSource.Cancel();
        }
    }
}