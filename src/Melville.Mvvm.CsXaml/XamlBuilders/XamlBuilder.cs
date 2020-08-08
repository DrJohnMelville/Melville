using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Windows.Controls;
using System.Windows.Data;
using Melville.MVVM.AdvancedLists.ListMonitors;
using Melville.MVVM.CSharpHacks;
using Melville.Mvvm.CsXaml.ValueSource;
using Melville.MVVM.Wpf.Bindings;
using Expression = System.Windows.Expression;

namespace Melville.Mvvm.CsXaml.XamlBuilders
{
    public interface IBindingContext<TDataContext>
    {
    }

    public static class IBindingContectOperations
    {
        // bind to lists for collections expecting an IEnumerabke
        public static ValueProxy<IEnumerable<T>> BindList<T, TDataContext>(this IBindingContext<TDataContext> receiver ,
            Expression<Func<TDataContext, IEnumerable<T>>> bindingFunc,
            BindingMode mode = BindingMode.Default,
            UpdateSourceTrigger update = UpdateSourceTrigger.PropertyChanged) =>
            Bind<IEnumerable<T>, TDataContext>(receiver, bindingFunc, null, mode, update);
        
        // Direct binding with a function
        public static ValueProxy<T> Bind<T, TDataContext>(this IBindingContext<TDataContext> receiver ,
            Expression<Func<TDataContext, T>> bindingFunc,
            BindingMode mode = BindingMode.Default,
            UpdateSourceTrigger update = UpdateSourceTrigger.PropertyChanged) =>
            Bind<T, TDataContext>(receiver, bindingFunc, null, mode, update);
        
        // bind with a converter
        public static ValueProxy<T> Bind<TIntermed, T, TDataContext>(this IBindingContext<TDataContext> _ ,
            Expression<Func<TDataContext, TIntermed>> bindingFunc,
            Func<TIntermed, T> converter,
            BindingMode mode = BindingMode.OneWay,
            UpdateSourceTrigger update = UpdateSourceTrigger.PropertyChanged) =>
            CreateBinding<T>(ExpressionToBindingString(bindingFunc), LambdaConverter.Create(converter),
                mode, update);
        
        // bind with a explicit IValueConverter
        public static ValueProxy<T> Bind<T, TDataContext>(this IBindingContext<TDataContext> _ ,
            Expression<Func<TDataContext, T>> bindingFunc,
            IValueConverter? converter,
            BindingMode mode = BindingMode.Default,
            UpdateSourceTrigger update = UpdateSourceTrigger.PropertyChanged) =>
            CreateBinding<T>(ExpressionToBindingString(bindingFunc), converter, mode, update);

        // inner actual creator
        private static ValueProxy<T> CreateBinding<T>(string pathString, IValueConverter? converter, BindingMode mode,
            UpdateSourceTrigger update) =>
            new ValueProxy<T>(new Binding(pathString)
            {
                Mode = mode,
                UpdateSourceTrigger = update,
                Converter = converter
            });

        private static string ExpressionToBindingString<T, TDataContext>(Expression<Func<TDataContext, T>> bindingFunc)
        {
            return String.Join(".", GetPropertyNames.FromExpression(bindingFunc).Reverse());
        }
    }

    public readonly struct BindingContext<TDataContext>: IBindingContext<TDataContext>
    {
    }
}