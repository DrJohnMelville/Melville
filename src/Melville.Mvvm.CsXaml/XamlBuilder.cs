using System;
using System.Linq;
using System.Linq.Expressions;
using System.Windows;
using System.Windows.Data;
using System.Windows.Markup;
using Melville.MVVM.CSharpHacks;

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

        public void Bind(
            DependencyProperty prop,
            Expression<Func<TDataContext, object>> bindingFunc,
            BindingMode mode = BindingMode.TwoWay,
            UpdateSourceTrigger update = UpdateSourceTrigger.PropertyChanged) =>
            Bind(prop, String.Join(", ", GetPropertyNames.FromExpression(bindingFunc).Reverse()),
                mode, update);
        
        public void Bind(
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