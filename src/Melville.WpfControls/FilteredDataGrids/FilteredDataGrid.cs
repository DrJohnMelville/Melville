using System.Windows;
using System.Windows.Controls;

namespace Melville.WpfControls.FilteredDataGrids
{
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