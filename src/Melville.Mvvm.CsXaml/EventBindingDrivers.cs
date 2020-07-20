using System;
using System.Windows;
using Melville.MVVM.Wpf.EventBindings.SearchTree;

namespace Melville.Mvvm.CsXaml
{
    public static class EventBindingDrivers
    {
        public static EventHandler<EventArgs> DispatchEvent<TTarget, TDataContext>(
            this in XamlBuilder<TTarget, TDataContext> target, string method,
            string parameters = "", int maxCalls = Int32.MaxValue) where TTarget: DependencyObject =>
            new EventDispatcher(method, parameters, maxCalls).Execute;
        public static RoutedEventHandler DispatchRoutedEvent<TTarget, TDataContext>(
            this in XamlBuilder<TTarget, TDataContext> target, string method,
            string parameters = "", int maxCalls = Int32.MaxValue) where TTarget: DependencyObject =>
            new EventDispatcher(method, parameters, maxCalls).Execute;

        private sealed class EventDispatcher : SearchTreeTarget
        {
            public EventDispatcher(string method, string parameters, int callsRemaining) : 
                base(method, parameters, callsRemaining)
            {
            }

            public void Execute(object? sender, EventArgs e)
            {
                if (!(sender is DependencyObject depObj)) return;
                InvokeTarget(depObj, e);
            }
        } 
    }
}