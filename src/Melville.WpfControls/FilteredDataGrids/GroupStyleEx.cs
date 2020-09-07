using System.Windows;
using System.Windows.Controls;

namespace Melville.WpfControls.FilteredDataGrids
{
    public static class GroupStyleEx
    {
        public static readonly DependencyProperty AppendProperty
            = DependencyProperty.RegisterAttached("Append", typeof (GroupStyle), typeof (GroupStyleEx),
                new PropertyMetadata(AppendChanged));

        public static GroupStyle GetAppend(DependencyObject obj)
        {
            return (GroupStyle) obj.GetValue(AppendProperty);
        }

        public static void SetAppend(DependencyObject obj, GroupStyle style)
        {
            obj.SetValue(AppendProperty, style);
        }

        private static void AppendChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {  
            if ((d is ItemsControl itemsControl) && e.NewValue is GroupStyle newStyle)
                itemsControl.GroupStyle.Add(newStyle);
        }
    }
}