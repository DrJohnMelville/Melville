using System;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Controls;
using Melville.MVVM.CSharpHacks;

namespace Melville.Mvvm.CsXaml
{
    public static class BuildXaml
    {
        public static T Create<T, TDataContext>(Action<XamlBuilder<T, TDataContext>> build)
            where T : DependencyObject, new() => Create<T, TDataContext>(new T(), build);

        public static T Create<T, TDataContext>(T target, Action<XamlBuilder<T, TDataContext>> build)
            where T : DependencyObject
        {
            build(new XamlBuilder<T, TDataContext>(target));
            return target;
        }
    }

    public static class GridBindings
    {
        public static void WithRows<TDataContext>(
            this XamlBuilder<Grid, TDataContext> target,
            string RowDeclarations)
        {
            
        }

        public static TChild NextChild<TDataContext, TChild>(
            this XamlBuilder<DockPanel, TDataContext> target,
            TChild child) where TChild : DependencyObject =>
            AssignDockProperty(child, Dock.Top);
    }
}