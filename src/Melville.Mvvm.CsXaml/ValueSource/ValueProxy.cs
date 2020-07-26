using System.Collections;
using System.Collections.Generic;
using System.DirectoryServices;
using System.Windows;
using System.Windows.Data;
using Melville.MVVM.USB;

namespace Melville.Mvvm.CsXaml.ValueSource
{
    public interface IValueProxy
    {
        void SetValue(DependencyObject obj, DependencyProperty prop);
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
            switch (otherValue)
            {
                case IValueProxy ivp: ivp.SetValue(obj, prop); break;
                case Binding b: SetBinding(obj, prop, b); break; 
                default: obj.SetValue(prop, otherValue); break;
            }
        }

        private static BindingExpressionBase SetBinding(DependencyObject obj, DependencyProperty prop, Binding b)
        {
            CheckBindingMode(prop, b);
            return BindingOperations.SetBinding(obj, prop, b);
        }

        private static void CheckBindingMode(DependencyProperty prop, Binding b)
        {
            if (prop.ReadOnly && (b.Mode == BindingMode.TwoWay || b.Mode == BindingMode.OneWayToSource))
            {
                b.Mode = BindingMode.OneWay;
            }
        }

        public static implicit operator ValueProxy<T>(T source) => new ValueProxy<T>(source);
   
        public ValueProxy<TNew> As<TNew>() => new ValueProxy<TNew>(otherValue);
    }
}