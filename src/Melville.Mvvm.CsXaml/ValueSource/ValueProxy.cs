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
                case Binding b: BindingOperations.SetBinding(obj, prop, b); break; 
                default: obj.SetValue(prop, otherValue); break;
            }
        }
        
        public static implicit operator ValueProxy<T>(T source) => new ValueProxy<T>(source);
        
        public ValueProxy<TNew> As<TNew>() => new ValueProxy<TNew>(otherValue);
    }
}