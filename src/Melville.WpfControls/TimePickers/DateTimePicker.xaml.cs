using  System;
using System.Windows;
using System.Windows.Controls;
using Melville.MVVM.BusinessObjects;

namespace Melville.WpfControls.TimePickers;

/// <summary>
/// Interaction logic for DateTimePicker.xaml
/// </summary>
public partial class DateTimePicker : UserControl
{



  public DateTime DateTime
  {
    get { return (DateTime) GetValue(DateTimeProperty); }
    set { SetValue(DateTimeProperty, value); }
  }

  // Using a DependencyProperty as the backing store for DateTime.  This enables animation, styling, binding, etc...
  public static readonly DependencyProperty DateTimeProperty =
    DependencyProperty.Register("DateTime", typeof(DateTime), typeof(DateTimePicker),
      new PropertyMetadata(new DateTime(), PropChanged));

  private static void PropChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
  {
    ((DateTimePicker) d).ViewModel.SetDateTime((DateTime) e.NewValue);
  }


  public CompositeDateTime ViewModel
  {
    get { return (CompositeDateTime) GetValue(ViewModelProperty); }
    set { SetValue(ViewModelProperty, value); }
  }

  // Using a DependencyProperty as the backing store for ViewModel.  This enables animation, styling, binding, etc...
  public static readonly DependencyProperty ViewModelProperty =
    DependencyProperty.Register("ViewModel", typeof(CompositeDateTime), typeof(DateTimePicker),
      new PropertyMetadata(null));




  public DateTimePicker()
  {
    ViewModel = new CompositeDateTime(this);
    InitializeComponent();
  }
}

public class CompositeDateTime : NotifyBase
{
  private readonly DateTimePicker dateTimePicker;
  private DateTime date;
  public DateTime Date
  {
    get => date;
    set
    {
      if (AssignAndNotify(ref date, value))
      {
        UpdateSource();
      }
    }
  }

  private void UpdateSource()
  {
    dateTimePicker.DateTime = Date + Time;
  }

  private TimeSpan time;

  public CompositeDateTime(DateTimePicker dateTimePicker)
  {
    this.dateTimePicker = dateTimePicker;
  }

  public TimeSpan Time
  {
    get => time;
    set
    {
      if (AssignAndNotify(ref time, value))
      {
        UpdateSource();
      }
    }
  }

  public void SetDateTime(DateTime eNewValue)
  {
    Date = eNewValue.Date;
    Time = eNewValue.TimeOfDay;
  }
}