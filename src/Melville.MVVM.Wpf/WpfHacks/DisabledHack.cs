using  System.Windows;

namespace Melville.MVVM.Wpf.WpfHacks
{
    public static class DisabledHack
    {
        private static void DisableIf(DependencyObject d, bool show)
        {
            if (d is UIElement i)
            {
                i.IsEnabled = !show;
            }
        }

        public static readonly DependencyProperty DisableUnlessProperty =
            DependencyProperty.RegisterAttached("DisableUnless",
                typeof(bool), typeof(DisabledHack), new FrameworkPropertyMetadata(true, DisableUnlessChanged));
        public static bool GetDisableUnless(DependencyObject ob) => (bool)ob.GetValue(DisableUnlessProperty);
        public static void SetDisableUnless(DependencyObject ob, bool value) => ob.SetValue(DisableUnlessProperty, value);
        private static void DisableUnlessChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) =>
            DisableIf(d, !(bool)e.NewValue);

        public static bool GetDisableIf(DependencyObject ob) => (bool)ob.GetValue(DisableIfProperty);
        public static void SetDisableIf(DependencyObject ob, bool value) => ob.SetValue(DisableIfProperty, value);
        public static readonly DependencyProperty DisableIfProperty =
            DependencyProperty.RegisterAttached("DisableIf",
                typeof(bool), typeof(DisabledHack), new FrameworkPropertyMetadata(true, DisableIfChanged));
        private static void DisableIfChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            DisableIf(d, (bool)e.NewValue);
        }

        public static object GetDisableUnlessNull(DependencyObject ob) => (object)ob.GetValue(DisableUnlessNullProperty);
        public static void SetDisableUnlessNull(DependencyObject ob, object value) => ob.SetValue(DisableUnlessNullProperty, value);
        public static readonly DependencyProperty DisableUnlessNullProperty =
            DependencyProperty.RegisterAttached("DisableUnlessNull",
                typeof(object), typeof(DisabledHack), new FrameworkPropertyMetadata("Unique String cAOPJH SW", DisableUnlessNullChanged));
        private static void DisableUnlessNullChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            DisableIf(d, e.NewValue != null);
        }

        public static object GetDisableIfNull(DependencyObject ob) => (object)ob.GetValue(DisableIfNullProperty);
        public static void SetDisableIfNull(DependencyObject ob, object value) => ob.SetValue(DisableIfNullProperty, value);
        public static readonly DependencyProperty DisableIfNullProperty =
            DependencyProperty.RegisterAttached("DisableIfNull",
                typeof(object), typeof(DisabledHack), new FrameworkPropertyMetadata("Unique String cAOPJH SW", DisableIfNullChanged));
        private static void DisableIfNullChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            DisableIf(d, e.NewValue == null);
        }

        public static string GetDisableIfWhitespace(DependencyObject ob) => (string)ob.GetValue(DisableIfWhitespaceProperty);
        public static void SetDisableIfWhitespace(DependencyObject ob, string value) => ob.SetValue(DisableIfWhitespaceProperty, value);
        public static readonly DependencyProperty DisableIfWhitespaceProperty =
            DependencyProperty.RegisterAttached("DisableIfWhitespace",
                typeof(string), typeof(DisabledHack), new FrameworkPropertyMetadata("Unique String cAOPJH SW", DisableIfWhitespaceChanged));
        private static void DisableIfWhitespaceChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            DisableIf(d, string.IsNullOrWhiteSpace(e.NewValue?.ToString()));
        }

        public static string GetDisableUnlessWhitespace(DependencyObject ob) => (string)ob.GetValue(DisableUnlessWhitespaceProperty);
        public static void SetDisableUnlessWhitespace(DependencyObject ob, string value) => ob.SetValue(DisableUnlessWhitespaceProperty, value);
        public static readonly DependencyProperty DisableUnlessWhitespaceProperty =
            DependencyProperty.RegisterAttached("DisableUnlessWhitespace",
                typeof(string), typeof(DisabledHack), new FrameworkPropertyMetadata("Unique String cAOPJH SW", DisableUnlessWhitespaceChanged));
        private static void DisableUnlessWhitespaceChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            DisableIf(d, !string.IsNullOrWhiteSpace(e.NewValue?.ToString()));
        }

    }
}