using  System;
using System.IO;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;

namespace Melville.WpfControls.TimePickers;

public class TimePicker : TextBox
{
  public string TimeFormat
  {
    get { return (string)GetValue(TimeFormatProperty); }
    set { SetValue(TimeFormatProperty, value); }
  }

  // Using a DependencyProperty as the backing store for TimeFormat.  This enables animation, styling, binding, etc...
  public static readonly DependencyProperty TimeFormatProperty =
    DependencyProperty.Register("TimeFormat", typeof(string), typeof(TimePicker), new PropertyMetadata("h:mm tt"));



  public TimeSpan Value
  {
    get => (TimeSpan)GetValue(ValueProperty);
    set => SetValue(ValueProperty, value);
  }

  public static readonly DependencyProperty ValueProperty =
    DependencyProperty.Register("Value", typeof(TimeSpan), typeof(TimePicker),
      new FrameworkPropertyMetadata(new TimeSpan(), DateChanged));

  private static void DateChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) => 
    ((TimePicker)d).UpdateTimeString();
  private void UpdateTimeString()
  {
    Text = (new DateTime(1975,07,28)+Value).ToString(TimeFormat);
  }


  public TimePicker()
  {
    LostFocus += (s, e) => Value = TimeParser.Parse(Text, DateTime.Now) ?? Value;
  }
}

public static class TimeParser
{
  public static TimeSpan? Parse(string input, DateTime currentTime)
  {
    return ParseShortcutEntry(input, currentTime) ?? ParseDirectTimeEntry(input);
  }

  #region Relative time entry

  private static Regex _shortcutDetector = new Regex(@"^([HhMmSs])([+-])(\d+)$");

  private static TimeSpan? ParseShortcutEntry(string text, DateTime currentTime)
  {
    var match = _shortcutDetector.Match(text);
    return !match.Success
      ? (TimeSpan?)null
      : ComputeNewTime(match.Groups[1].Value, match.Groups[2].Value, match.Groups[3].Value, currentTime);
  }

  private static TimeSpan ComputeNewTime(string prefix, string sign, string offset, DateTime now)
  {
    var delta = int.Parse(offset);
    if (sign == "-")
    {
      delta *= -1;
    }
    switch (prefix)
    {
      case "h":
      case "H":
        return now.AddHours(delta).TimeOfDay;
      case "m":
      case "M":
        return now.AddMinutes(delta).TimeOfDay;
      case "S":
      case "s":
        return now.AddSeconds(delta).TimeOfDay;
    }
    throw new InvalidDataException("This should be impossible.  The RegEx should make sure the case always fires");
  }


  #endregion
  #region Direct Time Entry

  private static TimeSpan? ParseDirectTimeEntry(string input)
  {
    var match = Regex.Match(input, "[aApP]");
    if (match.Success) // an AM or PM is in the string
    {
      return ParseTime(input.Substring(0, match.Index), AmPmFunc(match.Value));
    }

    return ParseTime(input, i => i);
  }

  private static TimeSpan? ParseTime(string input, Func<int, int> adjustHour)
  {
    var match = Regex.Match(input, @"(\d{1,2})\D*(\d{0,2})?\D*(\d{0,2})?");
    if (!match.Success) return null;
    try
    {
      var hours = adjustHour(ParseInt(match.Groups[1].Value));
      if (hours > 23 || hours < 0) return null;
      return new TimeSpan(hours,
        ParseInt(match.Groups[2].Value), ParseInt(match.Groups[3].Value));
    }
    catch (ArgumentOutOfRangeException) // numbers might be an invalid time, no need to check myself
    {
      return null;
    }
  }

  private static int ParseInt(string value) => int.TryParse(value, out var output) ? output : 0;

  private static Func<int, int> AmPmFunc(string value) =>
    (Char.ToLower(value[0]) == 'a') ? (Func<int, int>)(i => Wrap12(i)) : i => Wrap12(i) + 12;

  private static int Wrap12(int input) => input == 12 ? 0 : input;

  #endregion
}