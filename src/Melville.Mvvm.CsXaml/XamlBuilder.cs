using System;
using System.Linq;
using System.Linq.Expressions;
using System.Windows;
using System.Windows.Data;
using System.Windows.Markup;
using Melville.MVVM.CSharpHacks;
using Melville.Mvvm.CsXaml.ValueSource;
using Melville.MVVM.Wpf.Bindings;
using Expression = System.Windows.Expression;

namespace Melville.Mvvm.CsXaml
{
    public readonly struct XamlBuilder<TTarget, TDataContext>
        where TTarget: DependencyObject
    {
        public TTarget Target { get; }

        public XamlBuilder(TTarget target)
        {
            Target = target;
        }

        public TChild Child<TChild>(Action<XamlBuilder<TChild, TDataContext>> build) 
            where TChild : DependencyObject, new()
        {
            var child = BuildXaml.Create<TChild, TDataContext>(build);
            return Child(child);
        }

        public TChild Child<TChild>(TChild child) where TChild : new()
        {
            if (!(Target is IAddChild parentAsAddChild))
                throw new InvalidOperationException($"Cannot add child to {nameof(TTarget)}.");
            parentAsAddChild.AddChild(child);
            return child;
        }

        public ValueProxy<T> Bind<T>(Expression<Func<TDataContext, T>> bindingFunc,
            BindingMode mode = BindingMode.TwoWay,
            UpdateSourceTrigger update = UpdateSourceTrigger.PropertyChanged) =>
            Bind<T>(bindingFunc, null, mode, update);

        public ValueProxy<T> Bind<TIntermed, T>(Expression<Func<TDataContext, TIntermed>> bindingFunc,
            Func<TIntermed, T> converter,
            BindingMode mode = BindingMode.OneWay,
            UpdateSourceTrigger update = UpdateSourceTrigger.PropertyChanged) =>
            CreateBinding<T>(ExpressionToBindingString(bindingFunc), LambdaConverter.Create(converter),
                mode, update);

        public ValueProxy<T> Bind<T>(Expression<Func<TDataContext, T>> bindingFunc,
            IValueConverter? converter,
            BindingMode mode = BindingMode.TwoWay,
            UpdateSourceTrigger update = UpdateSourceTrigger.PropertyChanged) =>
            CreateBinding<T>(ExpressionToBindingString(bindingFunc), converter, mode, update);

        private static ValueProxy<T> CreateBinding<T>(string pathString, IValueConverter? converter, BindingMode mode,
            UpdateSourceTrigger update) =>
            new ValueProxy<T>(new Binding(pathString)
            {
                Mode = mode,
                UpdateSourceTrigger = update,
                Converter = converter
            });

        private static string ExpressionToBindingString<T>(Expression<Func<TDataContext, T>> bindingFunc)
        {
            return String.Join(", ", GetPropertyNames.FromExpression(bindingFunc).Reverse());
        }

        public void BindTo(
            DependencyProperty prop, 
            string bindingExpression, 
            BindingMode mode = BindingMode.TwoWay, 
            UpdateSourceTrigger update = UpdateSourceTrigger.PropertyChanged) =>
            BindingOperations.SetBinding(Target, prop, new Binding(bindingExpression)
            {
                Mode = mode,
                UpdateSourceTrigger = update
            });
    }
}