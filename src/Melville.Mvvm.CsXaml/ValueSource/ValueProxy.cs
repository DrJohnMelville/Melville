using System.Windows;
using System.Windows.Data;

namespace Melville.Mvvm.CsXaml.ValueSource
{
    public interface IValueProxy
    {
        void SetValue(DependencyObject obj, DependencyProperty prop);
    }

    public static class ValueProxyOperations
    {
        public static void SetValue(DependencyObject obj, DependencyProperty prop, object? value)
        {
            switch (value)
            {
                case IValueProxy ivp:
                    ivp.SetValue(obj, prop);
                    break;
                case Binding b:
                    SetBinding(obj, prop, b);
                    break;
                default:
                    obj.SetValue(prop, value);
                    break;
            }
        }

        private static void SetBinding(DependencyObject obj, DependencyProperty prop, Binding b)
        {
            CheckBindingMode(prop, b);
            BindingOperations.SetBinding(obj, prop, b);
        }

        private static void CheckBindingMode(DependencyProperty prop, Binding b)
        {
            if (prop.ReadOnly && (b.Mode == BindingMode.TwoWay || b.Mode == BindingMode.OneWayToSource))
            {
                b.Mode = BindingMode.OneWay;
            }
        }
        
    }

    public struct ValueProxy<T> : IValueProxy
    {
        private object? otherValue;

        public ValueProxy(object? otherValue) : this()
        {
            this.otherValue = otherValue;
        }

        public void SetValue(DependencyObject obj, DependencyProperty prop)
        {
            ValueProxyOperations.SetValue(obj, prop, otherValue);
        }
        
        public static implicit operator ValueProxy<T>(T source) => new ValueProxy<T>(source);
   
        public ValueProxy<TNew> As<TNew>() => new ValueProxy<TNew>(otherValue);
    }
}