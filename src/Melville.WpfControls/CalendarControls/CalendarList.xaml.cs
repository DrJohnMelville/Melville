using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using Melville.INPC;

namespace Melville.WpfControls.CalendarControls;

public partial class CalendarList
{
    [GenerateDP(typeof(CalendarBarCollection), "Items")]
    [GenerateDP(typeof(DataTemplate), "ItemTemplate")]
    [GenerateDP(typeof(DataTemplateSelector), "ItemTemplateSelector")]
    [GenerateDP(typeof(Style), "ItemContainerStyle")]
    public CalendarList()
    {
        InitializeComponent();
    }

    [GenerateDP]
    private void OnItemsSourceChanged(IEnumerable<ICalendarItem> items) => BuildItems();
    [GenerateDP]
    public static readonly DependencyProperty DisplayMonthProperty = DependencyProperty.Register(
        "DisplayMonth",
        typeof(DateTime), typeof(CalendarList), new PropertyMetadata(new DateTime(1975,07,01)));

    private void OnDisplayMonthChanged(DateTime month) => BuildItems();
    private void BuildItems()
    {
        if (ItemsSource is not { } source) return;
        if (DisplayMonth == default) return;
        Items = new CalendarBarCollection(source,DisplayMonth.FirstDayOnCalendar(), DisplayMonth.LastDayOnCalendar());
    }
}