using System;
using System.Windows.Data;
using System.Windows.Media;
using Melville.WpfControls.Bindings;
using Serilog.Events;

namespace Melville.Log.Viewer.LogViews
{
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
        public string? Exception => logEvent.Exception?.ToString();

        public static IValueConverter LevelToBrush = LambdaConverter.Create((LogEventLevel level) =>
            level switch
            {
                LogEventLevel.Fatal => Brushes.Black,
                LogEventLevel.Error => Brushes.Red,
                LogEventLevel.Warning => Brushes.DeepPink,
                LogEventLevel.Information => Brushes.DarkOrange,
                LogEventLevel.Debug => Brushes.LawnGreen,
                _ => Brushes.DarkGreen
            });
    }
}