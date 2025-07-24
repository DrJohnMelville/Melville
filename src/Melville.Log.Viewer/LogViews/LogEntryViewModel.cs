using System;
using System.Windows.Data;
using System.Windows.Media;
using Melville.MVVM.Wpf.Bindings;
using Microsoft.Extensions.Logging;

namespace Melville.Log.Viewer.LogViews;

public class LogEntryViewModel
{
    private readonly LogEvent logEvent;

    public LogEntryViewModel(LogEvent logEvent)
    {
        this.logEvent = logEvent;
    }

    public DateTimeOffset TimeStamp => logEvent.TimeStamp.ToLocalTime();
    public string Message => logEvent.Text;
    public LogLevel Level => logEvent.Level;
  
    public static IValueConverter LevelToBrush = LambdaConverter.Create((LogLevel level) =>
        level switch
        {
            LogLevel.Critical=> Brushes.Black,
            LogLevel.Error => Brushes.Red,
            LogLevel.Warning => Brushes.DeepPink,
            LogLevel.Information => Brushes.DarkOrange,
            LogLevel.Debug => Brushes.LawnGreen,
            _ => Brushes.DarkGreen
        });
}