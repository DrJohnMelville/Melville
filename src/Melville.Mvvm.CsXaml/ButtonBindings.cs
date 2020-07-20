using System;
using System.Windows;
using System.Windows.Controls;

namespace Melville.Mvvm.CsXaml
{
    public static class ButtonBindings
    {
        public static Button ChildButton<TTarget, TDataContext>(
            this XamlBuilder<TTarget, TDataContext> target,
            object content,
            string method,
            string parameters = "",
            int maxCalls = Int32.MaxValue) where TTarget:DependencyObject =>
            target.Child<Button>(i =>
            {
                i.Target.Content = content;
                i.Target.Click += target.DispatchRoutedEvent(method, parameters, maxCalls);
            });
    }
}