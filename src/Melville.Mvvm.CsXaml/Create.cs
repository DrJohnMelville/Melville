using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using Melville.Mvvm.CsXaml.ValueSource;
using Melville.Mvvm.CsXaml.XamlBuilders;

namespace Melville.Mvvm.CsXaml
{
    public static class Create
    {
        public static Button Button(
            ValueProxy<object> content,
            EventBinder clickBinder)
        {
            var ret = new Button();
            content.SetValue(ret, ContentControl.ContentProperty);
            clickBinder.Bind(ret, ButtonBase.ClickEvent);
            return ret;
        }
        public static TextBlock TextBlock(
            ValueProxy<string> text, 
            ValueProxy<double>? fontSize = null,
            ValueProxy<HorizontalAlignment>? horizontalAlignment = null)
        {
            var ret = new TextBlock().WithText(text).WithTextBlock_FontSize(fontSize);
            horizontalAlignment?.SetValue(ret, FrameworkElement.HorizontalAlignmentProperty);
            return ret;
        }

    }
}