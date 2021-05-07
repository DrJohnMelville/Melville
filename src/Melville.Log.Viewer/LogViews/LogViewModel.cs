using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using Melville.INPC;
using Melville.Lists;
using Melville.Log.Viewer.HomeScreens;
using Melville.MVVM.BusinessObjects;
using Serilog.Events;

namespace Melville.Log.Viewer.LogViews
{
    public partial class LogViewModel : NotifyBase, IHomeScreenPage
    {
        private string title;
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

        public void ClearLog() => Events.Clear();


        private readonly ILogConnection logConnection;
        public ICollection<LogEntryViewModel> Events { get; } = 
            new ThreadSafeBindableCollection<LogEntryViewModel>();

        public LogViewModel(ILogConnection logConnection, string title = "<Not Connected>")
        {
            this.logConnection = logConnection;
            this.title = title;
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