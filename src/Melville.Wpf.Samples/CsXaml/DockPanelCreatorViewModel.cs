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
                i.WithChild(Dock.Top, Create.TextBlock("Top").WithBackground(Brushes.Blue));
                i.WithChild(Dock.Bottom, Create.TextBlock("Bottom")
                    .WithBackground(Brushes.Blue));
                i.WithChild(Dock.Left, Create.TextBlock("Left").WithBackground(Brushes.Green));
                i.WithChild(Dock.Right, Create.TextBlock("Right").WithBackground(Brushes.Green));
                i.WithChild(new TextBlock()
                    .WithText("Center")
                    .WithBackground(Brushes.Red)
                    .WithVerticalAlignment(VerticalAlignment.Center)
                    .WithTextBlock_TextAlignment(TextAlignment.Center));
               i.LastChildFill = true;
            }));
        }
    }
}