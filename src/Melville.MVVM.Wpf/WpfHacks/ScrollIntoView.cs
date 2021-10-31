using System.Windows;

namespace Melville.MVVM.Wpf.WpfHacks;

public static class ScrollIntoView
{
    public static DependencyProperty OnChangeProperty = DependencyProperty.RegisterAttached(
        "OnChange", typeof(object), typeof(ScrollIntoView),
        new FrameworkPropertyMetadata(null,
            OnChangeChanged));

    private static void OnChangeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is FrameworkElement source)
        {
            source.BringIntoView();
        }
    }

    public static object GetOnChange(DependencyObject obj) => obj.GetValue(OnChangeProperty);

    public static void SetOnChange(DependencyObject obj, object value) =>
        obj.SetValue(OnChangeProperty, value);
}