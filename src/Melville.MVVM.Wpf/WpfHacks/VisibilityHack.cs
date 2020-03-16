using  System.Windows;

namespace Melville.MVVM.Wpf.WpfHacks
{
    public static class VisibilityHack
    {
        private static void CollapseIf(DependencyObject d, bool show)
        {
            if (d is UIElement i)
            {
                i.Visibility = show ? Visibility.Collapsed : Visibility.Visible;
            }
        }

        private static void HideIf(DependencyObject d, bool show)
        {
            if (d is UIElement i)
            {
                i.Visibility = show ? Visibility.Hidden : Visibility.Visible;
            }
        }

        public static readonly DependencyProperty CollapseUnlessProperty =
          DependencyProperty.RegisterAttached("CollapseUnless",
            typeof(bool), typeof(VisibilityHack), new FrameworkPropertyMetadata(true, CollapseUnlessChanged));
        public static bool GetCollapseUnless(DependencyObject ob) => (bool)ob.GetValue(CollapseUnlessProperty);
        public static void SetCollapseUnless(DependencyObject ob, bool value) => ob.SetValue(CollapseUnlessProperty, value);
        private static void CollapseUnlessChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            CollapseIf(d, !(bool)e.NewValue);
        }

        public static bool GetCollapseIf(DependencyObject ob) => (bool)ob.GetValue(CollapseIfProperty);
        public static void SetCollapseIf(DependencyObject ob, bool value) => ob.SetValue(CollapseIfProperty, value);
        public static readonly DependencyProperty CollapseIfProperty =
          DependencyProperty.RegisterAttached("CollapseIf",
            typeof(bool), typeof(VisibilityHack), new FrameworkPropertyMetadata(false, CollapseIfChanged));
        private static void CollapseIfChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            CollapseIf(d, (bool)e.NewValue);
        }

        public static bool GetHideUnless(DependencyObject ob) => (bool)ob.GetValue(HideUnlessProperty);
        public static void SetHideUnless(DependencyObject ob, bool value) => ob.SetValue(HideUnlessProperty, value);
        public static readonly DependencyProperty HideUnlessProperty =
          DependencyProperty.RegisterAttached("HideUnless",
            typeof(bool), typeof(VisibilityHack), new FrameworkPropertyMetadata(true, HideUnlessChanged));
        private static void HideUnlessChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            HideIf(d, !(bool)e.NewValue);
        }

        public static bool GetHideIf(DependencyObject ob) => (bool)ob.GetValue(HideIfProperty);
        public static void SetHideIf(DependencyObject ob, bool value) => ob.SetValue(HideIfProperty, value);
        public static readonly DependencyProperty HideIfProperty =
          DependencyProperty.RegisterAttached("HideIf",
            typeof(bool), typeof(VisibilityHack), new FrameworkPropertyMetadata(false, HideIfChanged));
        private static void HideIfChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            HideIf(d, (bool)e.NewValue);
        }

        public static object GetCollapseUnlessNull(DependencyObject ob) => (object)ob.GetValue(CollapseUnlessNullProperty);
        public static void SetCollapseUnlessNull(DependencyObject ob, object value) => ob.SetValue(CollapseUnlessNullProperty, value);
        public static readonly DependencyProperty CollapseUnlessNullProperty =
          DependencyProperty.RegisterAttached("CollapseUnlessNull",
            typeof(object), typeof(VisibilityHack), new FrameworkPropertyMetadata("Unique String cAOPJH SW", CollapseUnlessNullChanged));
        private static void CollapseUnlessNullChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            CollapseIf(d, e.NewValue != null);
        }

        public static object GetCollapseIfNull(DependencyObject ob) => (object)ob.GetValue(CollapseIfNullProperty);
        public static void SetCollapseIfNull(DependencyObject ob, object value) => ob.SetValue(CollapseIfNullProperty, value);
        public static readonly DependencyProperty CollapseIfNullProperty =
          DependencyProperty.RegisterAttached("CollapseIfNull",
            typeof(object), typeof(VisibilityHack), new FrameworkPropertyMetadata("Unique String cAOPJH SW", CollapseIfNullChanged));
        private static void CollapseIfNullChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            CollapseIf(d, e.NewValue == null);
        }

        public static string GetCollapseIfWhitespace(DependencyObject ob) => (string)ob.GetValue(CollapseIfWhitespaceProperty);
        public static void SetCollapseIfWhitespace(DependencyObject ob, string value) => ob.SetValue(CollapseIfWhitespaceProperty, value);
        public static readonly DependencyProperty CollapseIfWhitespaceProperty =
          DependencyProperty.RegisterAttached("CollapseIfWhitespace",
            typeof(string), typeof(VisibilityHack), new FrameworkPropertyMetadata("Unique String cAOPJH SW", CollapseIfWhitespaceChanged));
        private static void CollapseIfWhitespaceChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            CollapseIf(d, string.IsNullOrWhiteSpace(e.NewValue?.ToString()));
        }

        public static string GetCollapseUnlessWhitespace(DependencyObject ob) => (string)ob.GetValue(CollapseUnlessWhitespaceProperty);
        public static void SetCollapseUnlessWhitespace(DependencyObject ob, string value) => ob.SetValue(CollapseUnlessWhitespaceProperty, value);
        public static readonly DependencyProperty CollapseUnlessWhitespaceProperty =
          DependencyProperty.RegisterAttached("CollapseUnlessWhitespace",
            typeof(string), typeof(VisibilityHack), new FrameworkPropertyMetadata("Unique String cAOPJH SW", CollapseUnlessWhitespaceChanged));
        private static void CollapseUnlessWhitespaceChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            CollapseIf(d, !string.IsNullOrWhiteSpace(e.NewValue?.ToString()));
        }

    }
}