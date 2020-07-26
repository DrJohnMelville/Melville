using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Melville.Mvvm.CsXaml;
using Melville.Mvvm.CsXaml.XamlBuilders;

namespace Melville.Wpf.Samples.CsXaml
{
    public class DockPanelCreatorViewModel
    {
        
    }

    public class DockPanelCreatorView : UserControl
    {
        public DockPanelCreatorView()
        {
            AddChild(BuildXaml.Create<DockPanel, DockPanelCreatorViewModel>((i, context) =>
            {
                i.WithChild(Dock.Top, Create.TextBlock("Top")).Background = Brushes.Blue;
                i.WithChild(Dock.Bottom, Create.TextBlock("Bottom")).Background = Brushes.Blue;
                i.WithChild(Dock.Left, Create.TextBlock("Left")).Background = Brushes.Green;
                i.WithChild(Dock.Right, Create.TextBlock("Right")).Background = Brushes.Green;
                i.WithChild(new TextBlock
                {
                    Background = Brushes.Red,
                    TextAlignment = TextAlignment.Center,
                    VerticalAlignment = VerticalAlignment.Center
                });
               i.LastChildFill = true;
            }));
        }
    }
}