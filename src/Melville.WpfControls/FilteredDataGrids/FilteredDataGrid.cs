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
    public static class FilteredDataGrid
    {
        public static readonly DependencyProperty UseFilterProperty = DependencyProperty.RegisterAttached(
            "UseFilter", typeof(bool), typeof(FilteredDataGrid),
            new FrameworkPropertyMetadata(false, OnFilterPropertyChanged));

        public static bool GetUseFilter(DependencyObject obj) => (bool) (obj.GetValue(UseFilterProperty));
        public static void SetUseFilter(DependencyObject obj, bool value) =>
            obj.SetValue(UseFilterProperty, value);

        private static void OnFilterPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (e.NewValue is bool b && b && d is DataGrid dg)
            {
                dg.Style = (Style) new GridParts()["FilteredDataGrid"];
            }
        }
    }
}