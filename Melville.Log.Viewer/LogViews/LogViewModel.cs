using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Melville.Log.Viewer.HomeScreens;
using Melville.Log.Viewer.NamedPipeServers;
using Melville.MVVM.AdvancedLists;
using Melville.MVVM.BusinessObjects;
using Serilog.Events;
using Serilog.Formatting.Compact.Reader;

namespace Melville.Log.Viewer.LogViews
{
    public class LogViewModel: NotifyBase, IHomeScreenPage
    {
        private string title = "<Not Connected>";
        public string Title
        {
            get => title;
            set => AssignAndNotify(ref title, value);
        }

        private readonly Stream logConnection;
        public ICollection<LogEntryViewModel> Events { get; } = new ThreadSafeBindableCollection<LogEntryViewModel>();

        public LogViewModel(Stream logConnection)
        {
            this.logConnection = logConnection;
            ClientConnected();
        }

        private void ClientConnected()
        {
            Task.Run(() =>
            {
                var reader = new LogEventReader(new StreamReader(logConnection));
                while (reader.TryRead(out var logEvent))
                {
                    if (logEvent.Properties.TryGetValue("AssignProcessName", out var value))
                    {
                        Title = value.ToString()[1..^1];
                    }
                    else
                    {
                        Events.Add(new LogEntryViewModel(logEvent));
                    }
                }
            });
        }
    }

    public class LogEntryViewModel
    {
        private readonly LogEvent logEvent;
        public LogEntryViewModel(LogEvent logEvent)
        {
            this.logEvent = logEvent;
        }

        public DateTimeOffset TimeStamp => logEvent.Timestamp.ToLocalTime().DateTime;
        public string Message => logEvent.MessageTemplate.Render(logEvent.Properties);
        public LogEventLevel Level => logEvent.Level;
        public string Exception => logEvent.Exception?.ToString() ?? "";
    }
}