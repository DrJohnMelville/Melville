using System.Windows;
using System.Windows.Controls;

namespace Melville.Mvvm.CsXaml
{
    public static class DockPanelBindings
    {
        public static TChild TopChild<TDataContext, TChild>(
            this XamlBuilder<DockPanel, TDataContext> target,
            TChild child) where TChild : DependencyObject =>
            AssignDockProperty(child, Dock.Top);
        public static TChild BottomChild<TDataContext, TChild>(
            this XamlBuilder<DockPanel, TDataContext> target,
            TChild child) where TChild : DependencyObject =>
            AssignDockProperty(child, Dock.Bottom);
        public static TChild LeftChild<TDataContext, TChild>(
            this XamlBuilder<DockPanel, TDataContext> target,
            TChild child) where TChild : DependencyObject =>
            AssignDockProperty(child, Dock.Left);
        public static TChild RightChild<TDataContext, TChild>(
            this XamlBuilder<DockPanel, TDataContext> target,
            TChild child) where TChild : DependencyObject =>
            AssignDockProperty(child, Dock.Right);

        private static TChild AssignDockProperty<TChild>(TChild child, Dock dockPosition)
            where TChild : DependencyObject
        {
            child.SetValue(DockPanel.DockProperty, dockPosition);
            return child;
        }
    }
}