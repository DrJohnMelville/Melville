using System.Windows;
using System.Windows.Controls;
using Melville.Mvvm.CsXaml.XamlBuilders;

namespace Melville.Mvvm.CsXaml
{
    public static class DockPanelBindings
    {
        public static DockPanel WithChild(this DockPanel parent, Dock position, UIElement child)
        {
            DockPanel.SetDock(child, position);
            return parent.WithChild(child);
        }
    }
}