using System.Windows;
using Melville.Mvvm.CsXaml.ValueSource;

namespace Melville.Mvvm.CsXaml
{
    public static class StyleBuilder
    {
        public static Style WithSetter(this Style style, DependencyProperty prop, 
            ValueProxy<object> value)
        {
            style.Setters.Add(new Setter(prop, value.InnermostValue()));;
            return style;
        }
        
    }
}