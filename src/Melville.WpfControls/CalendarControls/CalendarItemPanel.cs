using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Melville.INPC;
using Melville.Linq;
using Melville.MVVM.Wpf.EventBindings;
using Melville.WpfControls.Hacks;
using Visibility = System.Windows.Visibility;

namespace Melville.WpfControls.CalendarControls;

public partial class CalendarItemPanel: Panel
{
    [GenerateDP()]
    public static readonly DependencyProperty DisplayDateProperty = DependencyProperty.Register("DisplayDate",
        typeof(DateTime), typeof(CalendarItemPanel), new PropertyMetadata(new DateTime(1975,07,01)));
    private IList<double> rowHeights = Array.Empty<double>();
    public IClickToDayComputer ClickToDayComputer() => 
        new ClickToDayComputer(DisplayDate.FirstDayOnCalendar(),ActualWidth,rowHeights);
        
    protected override void OnRender(DrawingContext dc)
    {
        var size = RenderSize;
        new CalendarGridPainter(dc, DisplayDate.FirstDayOnCalendar(), DisplayDate.Month, size,
            VisualTreeHelper.GetDpi(this).PixelsPerDip, rowHeights).PaintCalendar();
    }


    private MonthSorter<FrameworkElement> CalendarBarSorter(
        Func<FrameworkElement, int, int, double, double> target) =>
        new MonthSorter<FrameworkElement>(DisplayDate.FirstDayOnCalendar(), 22.0,
            i => (CalendarBar) (i.DataContext), target);

    protected override Size MeasureOverride(Size availableSize)
    {
        var singleColSize = new ColumnMeasure(availableSize);
        var sorter = CalendarBarSorter(singleColSize.MeasureElement);
        sorter.SortDates(NonCollapsedChildren());
        return new Size(availableSize.Width, sorter.TotalHeight);
    }

    private IEnumerable<FrameworkElement> NonCollapsedChildren() => InternalChildren
        .OfType<FrameworkElement>()
        .Where(i=>i.Visibility != Visibility.Collapsed);

    protected override Size ArrangeOverride(Size finalSize)
    {
        var singleColSize = new ColumnMeasure(finalSize);
        var sorter = CalendarBarSorter(PlaceSingleItem);
        rowHeights = sorter.SortDates(NonCollapsedChildren());
        return new Size(finalSize.Width, sorter.TotalHeight);

        double PlaceSingleItem(FrameworkElement item, int column, int width, double yOffset)
        {
            item.Arrange(new Rect(singleColSize.ColumnOffset(column), yOffset, 
                singleColSize.MultiColWidth(width), item.DesiredSize.Height));
            return item.DesiredSize.Height;
        }
    }
}
public sealed class CalendarItemsControl : ItemsControl, IAdditionlTargets
{
        
    protected override DependencyObject GetContainerForItemOverride() => 
        new CalendarItemContainer();

    protected override bool IsItemItsOwnContainerOverride(object item) => 
        item is CalendarItemContainer;

    public IEnumerable<object> Targets() =>
        FindCalendarItemPanel() is { } cip
            ? new EnumerateSingle<object>(cip.ClickToDayComputer())
            : Array.Empty<object>();

    private CalendarItemPanel? FindCalendarItemPanel() => 
        this.Descendants().OfType<CalendarItemPanel>().FirstOrDefault();
}