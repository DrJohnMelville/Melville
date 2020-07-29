using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Windows;
using System.Windows.Data;
using System.Windows.Markup;
using Melville.MVVM.AdvancedLists.ListMonitors;
using Melville.MVVM.CSharpHacks;
using Melville.Mvvm.CsXaml.ValueSource;
using Melville.MVVM.Wpf.Bindings;
using Expression = System.Windows.Expression;

namespace Melville.Mvvm.CsXaml.XamlBuilders
{
    public readonly struct BindingContext<TDataContext>
    {
        public ValueProxy<IEnumerable<T>> BindList<T>(Expression<Func<TDataContext, IEnumerable<T>>> bindingFunc,
            BindingMode mode = BindingMode.Default,
            UpdateSourceTrigger update = UpdateSourceTrigger.PropertyChanged) =>
            Bind<IEnumerable<T>>(bindingFunc, null, mode, update);
        public ValueProxy<T> Bind<T>(Expression<Func<TDataContext, T>> bindingFunc,
            BindingMode mode = BindingMode.Default,
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
            BindingMode mode = BindingMode.Default,
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
            return String.Join(".", GetPropertyNames.FromExpression(bindingFunc).Reverse());
        }
    }

    public static class XamlBuilderOps
    {
        public static T WithChild<T>(this T parent, UIElement child) where T : IAddChild
        {
            parent.AddChild(child);
            return parent;
        }

        public static T Bind<T>(this T source, RoutedEvent routedEvent, EventBinder? binding)
          where T:UIElement
        {
            binding?.Bind(source, routedEvent);
            return source;
        }
    }


}