using System.Windows;
using System.Windows.Markup;
using Melville.Mvvm.CsXaml.ValueSource;

namespace Melville.Mvvm.CsXaml.XamlBuilders
{
    public static class XamlBuilderOps
    {
        public static T WithChild<T>(this T parent, UIElement child) where T : IAddChild
        {
            parent.AddChild(child);
            return parent;
        }

        public static T BindEvent<T>(this T source, RoutedEvent routedEvent, EventBinder? binding)
            where T:UIElement
        {
            binding?.Bind(source, routedEvent);
            return source;
        }
    }
}