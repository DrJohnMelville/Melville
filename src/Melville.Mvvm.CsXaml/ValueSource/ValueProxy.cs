using System.Diagnostics;
using System.Windows;
using System.Windows.Data;
using System.Windows.Forms.VisualStyles;
using System.Windows.Threading;

namespace Melville.Mvvm.CsXaml.ValueSource
{
    public interface IValueProxy
    {
        public object Value { get; }
    }

    public static class ValueProxyOperations
    {
        public static object InnermostValue(this IValueProxy prox)
        {
            object ret = prox.Value;
            while (ret is IValueProxy inner) ret = inner.Value;
            return ret;
        }
        public static void SetValue(this IValueProxy prox, DependencyObject obj, DependencyProperty prop)
        {
            object value = prox.InnermostValue();
            switch (value)
            {
                case BindingBase b:
                    SetBinding(obj, prop, b);
                    break;
                default:
                    obj.SetValue(prop, value);
                    break;
            }
        }
        
        private static void SetBinding(DependencyObject obj, DependencyProperty prop, BindingBase b)
        {
            ProhibitWriteBindingToReadOnlyProperties(prop, b);
            BindingOperations.SetBinding(obj, prop, b);
        }

        public static void StyleSetter(this IValueProxy prox, Style style, DependencyProperty prop)
        {
            style.Setters.Add(new Setter(prop, prox.InnermostValue()));
        }


        private static void ProhibitWriteBindingToReadOnlyProperties(DependencyProperty prop, BindingBase bb)
        {
            if (!(bb is Binding b)) return;
            if (prop.ReadOnly && (b.Mode == BindingMode.TwoWay || b.Mode == BindingMode.OneWayToSource))
            {
                b.Mode = BindingMode.OneWay;
            }
        }

        public static T ForceValue<T>(this IValueProxy prox) => (T) prox.InnermostValue();
        public static BindingBase ForceBindingBase(this IValueProxy prox)
        {
            var value = prox.InnermostValue();
            return value is BindingBase bb ? bb : new Binding() {Source = value};
        }

        public static T WithTracing<T>(this T binding, PresentationTraceLevel level = PresentationTraceLevel.High) 
            where T : IValueProxy
        {
            PresentationTraceSources.SetTraceLevel(binding.InnermostValue(), level);
            return binding;
        }
    }

    public readonly struct ValueProxy<T> : IValueProxy
    {
        public object Value { get; }

        public ValueProxy(object otherValue) : this()
        {
            Value = otherValue;
        }
        
        public static implicit operator ValueProxy<T>(T source) => new ValueProxy<T>(source);
        public ValueProxy<TNew> As<TNew>() => new ValueProxy<TNew>(Value);
    }

    public struct ThicknessValueProxy : IValueProxy
    {
        public object Value { get; }

        public ThicknessValueProxy(object otherValue) : this()
        {
            Value = otherValue;
        }

        
        public static implicit operator ThicknessValueProxy(Thickness source) => new ThicknessValueProxy(source);
        public static implicit operator ThicknessValueProxy(ValueProxy<Thickness> source) => new ThicknessValueProxy(source);
        public static implicit operator ThicknessValueProxy(double d) => new ThicknessValueProxy(new Thickness(d));
        public static implicit operator ThicknessValueProxy((double lr, double tb) input) => 
            new ThicknessValueProxy(new Thickness(input.lr, input.tb, input.lr, input.tb));
        public static implicit operator ThicknessValueProxy((double l, double t, double r, double b) input) => 
            new ThicknessValueProxy(new Thickness(input.l, input.t, input.r, input.b));
        public ValueProxy<TNew> As<TNew>() => new ValueProxy<TNew>(Value);
    }
}