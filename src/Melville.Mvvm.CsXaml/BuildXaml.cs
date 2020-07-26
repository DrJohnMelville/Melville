using System;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Controls;
using Melville.MVVM.CSharpHacks;
using Melville.Mvvm.CsXaml.XamlBuilders;

namespace Melville.Mvvm.CsXaml
{
    public static class BuildXaml
    {
        public static T Create<T, TDataContext>(Action<T, BindingContext<TDataContext>> build)
            where T : DependencyObject, new() => Create<T, TDataContext>(new T(), build);

        public static T Create<T, TDataContext>(T target, Action<T, BindingContext<TDataContext>> build)
            where T : DependencyObject
        {
            build(target, new BindingContext<TDataContext>());
            return target;
        }
    }
}