using System;
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

    public class Style<T> : Style
    {
        public Style(Type targetType) : base(targetType)
        {
        }

        public Style(Type targetType, Style basedOn) : base(targetType, basedOn)
        {
        }
    }
}