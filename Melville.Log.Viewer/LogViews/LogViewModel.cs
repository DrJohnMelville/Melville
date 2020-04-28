using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
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


        private readonly ILogConnection logConnection;
        public ICollection<LogEntryViewModel> Events { get; } = 
            new ThreadSafeBindableCollection<LogEntryViewModel>();

        public LogViewModel(ILogConnection logConnection)
        {
            this.logConnection = logConnection;
            logConnection.LogEventArrived += HandleEvent;
        }
        
        private void HandleEvent(object? _, LogEventArrivedEventArgs args) =>
            HandleEvent(args.LogEvent);
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

        private static bool IsPrecessNameMessage(LogEvent logEvent, 
            [NotNullWhen(true)]out LogEventPropertyValue? value) =>
            logEvent.Properties.TryGetValue("AssignProcessName", out value);

        private void SendDesiredLevelToSink() => logConnection.SetDesiredLevel(MinimumLevel);
        public void Stop() => logConnection.StopReading();
    }
}