using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Windows.Data;
using Melville.MVVM.CSharpHacks;
using Melville.Mvvm.CsXaml.ValueSource;
using Melville.MVVM.Wpf.Bindings;

namespace Melville.Mvvm.CsXaml.XamlBuilders
{
    public interface IBindingContext<TDataContext>
    {
        Binding FixBinding(Binding source);
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
        public static ValueProxy<T> Bind<TIntermed, T, TDataContext>(this IBindingContext<TDataContext> receiver ,
            Expression<Func<TDataContext, TIntermed>> bindingFunc,
            Func<TIntermed, T> converter,
            BindingMode mode = BindingMode.OneWay,
            UpdateSourceTrigger update = UpdateSourceTrigger.PropertyChanged) =>
            CreateBinding<T, TDataContext>(ExpressionToBindingString(bindingFunc), LambdaConverter.Create(converter),
                mode, update, receiver);
        
        // bind with a explicit IValueConverter
        public static ValueProxy<T> Bind<T, TDataContext>(this IBindingContext<TDataContext> receiver ,
            Expression<Func<TDataContext, T>> bindingFunc,
            IValueConverter? converter,
            BindingMode mode = BindingMode.Default,
            UpdateSourceTrigger update = UpdateSourceTrigger.PropertyChanged) =>
            CreateBinding<T, TDataContext>(ExpressionToBindingString(bindingFunc), converter, mode, update, receiver);

        // inner actual creator
        private static ValueProxy<T> CreateBinding<T,TDC>(string pathString, IValueConverter? converter, BindingMode mode,
            UpdateSourceTrigger update, IBindingContext<TDC> context) =>
            new ValueProxy<T>( context.FixBinding(new Binding(pathString)
            {
                Mode = mode,
                UpdateSourceTrigger = update,
                Converter = converter
            }));

        private static string ExpressionToBindingString<T, TDataContext>(Expression<Func<TDataContext, T>> bindingFunc)
        {
            return String.Join(".", GetPropertyNames.FromExpression(bindingFunc).Reverse());
        }
    }

    public class BindingContext<TDataContext>: IBindingContext<TDataContext>
    {
        public Binding FixBinding(Binding source) => source;
    }

    public class TemplateBindingContext<TControl>: IBindingContext<TControl>
    {
        public Binding FixBinding(Binding source)
        {
            source.RelativeSource = RelativeSource.TemplatedParent;
            return source;
        }
    }

    public class SourcedBindingContext<T> : IBindingContext<T>
    {
        private T target;

        public SourcedBindingContext(T target)
        {
            this.target = target;
        }

        public Binding FixBinding(Binding source)
        {
            source.Source = target;
            return source;
        }
    }
}