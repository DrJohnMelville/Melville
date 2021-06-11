using System.Windows;
using System.Windows.Controls;

namespace Melville.WpfControls.CalendarControls
{
    public sealed class CalendarItemContainer : ContentControl
    {
        static CalendarItemContainer()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(CalendarItemContainer),
                new FrameworkPropertyMetadata(typeof(CalendarItemContainer)));
        }
    }
}