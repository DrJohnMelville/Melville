using System;
using System.Windows;

namespace Melville.Mvvm.CsXaml.XamlBuilders
{
    public static class CodeStyles
    {
        public static readonly DependencyProperty CodeStyleProperty = DependencyProperty.RegisterAttached(
            "CodeStyle", typeof(Action<DependencyObject>), typeof(CodeStyles),
            new FrameworkPropertyMetadata(null, CSPChanged));

        public static Action<DependencyObject> GetCodeStyle(DependencyObject obj) =>
            (Action<DependencyObject>)obj.GetValue(CodeStyleProperty);

        public static void SetCodeStyle(DependencyObject obj, Action<DependencyObject> value) =>
            obj.SetValue(CodeStyleProperty, value);

        private static void CSPChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d != null && e.NewValue is Action<DependencyObject> operation)
                operation(d);
        }
    }
}