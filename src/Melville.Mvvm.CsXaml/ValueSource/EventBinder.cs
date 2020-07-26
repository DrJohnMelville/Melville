using System;
using System.Windows;
using System.Windows.Controls;
using Melville.MVVM.Wpf.EventBindings.SearchTree;

namespace Melville.Mvvm.CsXaml.ValueSource
{
    public readonly struct EventBinder
    {
        private readonly EventDispatcher dispatcher;

        public EventBinder(string method, string parameters, int maxCalls)
        { 
            dispatcher = new EventDispatcher(method, parameters, maxCalls);
        }
        
        public static implicit operator EventBinder(string method) => new EventBinder(method, "", Int32.MaxValue); 
        public static implicit operator EventBinder((string Method, string Parameters) p) => 
            new EventBinder(p.Method, p.Parameters, Int32.MaxValue); 
        public static implicit operator EventBinder((string Method, string Parameters, int MaxCalls) p) => 
            new EventBinder(p.Method, p.Parameters, p.MaxCalls); 
        public static implicit operator EventBinder((string Method, int MaxCalls) p) => 
            new EventBinder(p.Method, "", p.MaxCalls);

        public void Bind(UIElement target, RoutedEvent eventDecl) =>
            target.AddHandler(eventDecl, (RoutedEventHandler)dispatcher.Execute);

        private sealed class EventDispatcher : SearchTreeTarget
        {
            public EventDispatcher(string method, string parameters, int callsRemaining) : 
                base(method, parameters, callsRemaining)
            {
            }

            public void Execute(object? sender, RoutedEventArgs e)
            {
                if (!(sender is DependencyObject depObj)) return;
                InvokeTarget(depObj, e);
            }
        }
    }
}