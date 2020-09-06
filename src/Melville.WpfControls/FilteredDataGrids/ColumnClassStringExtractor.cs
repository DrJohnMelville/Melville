using System;
using System.Diagnostics.CodeAnalysis;
using System.Windows;
using System.Windows.Controls;
using Melville.MVVM.CSharpHacks;

namespace Melville.WpfControls.FilteredDataGrids
{
    public static class ColumnClassStringExtractor 
    {
        public static readonly DependencyProperty FilterStringMethodProperty = 
            DependencyProperty.RegisterAttached("FilterStringMethod", typeof(Func<object, string>), 
                typeof(ColumnClassStringExtractor), new PropertyMetadata(null));

        public static Func<object, string>? GetFilterStringMethod(DependencyObject obj) =>
            (Func<object, string>?) obj.GetValue(FilterStringMethodProperty);

        public static void SetFilterStringMethod(DependencyObject obj, Func<object, string>? value) =>
            obj.SetValue(FilterStringMethodProperty, value);

        public static readonly DependencyProperty FilterStringPathProperty = 
            DependencyProperty.RegisterAttached("FilterStringPath", typeof(string), 
                typeof(ColumnClassStringExtractor), new PropertyMetadata(null));

        public static string? GetFilterStringPath(DependencyObject obj) =>
            (string) obj.GetValue(FilterStringPathProperty);

        public static void SetFilterStringPath(DependencyObject obj, string? value) =>
            obj.SetValue(FilterStringPathProperty, value);
        
        public static Func<object, string>? ConstructFilterStringMethod(this DataGridColumn column) =>
            GetFilterStringMethod(column) ??
            ConstructPathMethod(GetFilterStringPath(column)) ??
            ConstructPathMethod(column.SortMemberPath);

        [return:NotNullIfNotNull("path")]
        private static Func<object, string>? ConstructPathMethod(string? path)
        {
            if (path == null) return null;
            return o => ReflectionHelper.GetPath(o, path)?.ToString() ?? "<Null>";
        }
    }
}