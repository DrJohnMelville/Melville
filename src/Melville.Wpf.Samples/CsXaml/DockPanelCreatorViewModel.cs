using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Melville.Mvvm.CsXaml;

namespace Melville.Wpf.Samples.CsXaml
{
    public class DockPanelCreatorViewModel
    {
        
    }

    public class DockPanelCreatorView : UserControl
    {
        public DockPanelCreatorView()
        {
            AddChild(BuildXaml.Create<DockPanel, DockPanelCreatorViewModel>(i =>
            {
                i.TopChild(i.ChildTextBlock("Top")).Background = Brushes.Blue;
                i.BottomChild(i.ChildTextBlock("Bottom")).Background = Brushes.Blue;
                i.LeftChild(i.ChildTextBlock("Left")).Background = Brushes.Green;
                i.RightChild(i.ChildTextBlock("Right")).Background = Brushes.Green;
                var center = i.ChildTextBlock("Center");
                center.Background = Brushes.Red;
                center.TextAlignment = TextAlignment.Center;
                center.VerticalAlignment = VerticalAlignment.Center;
                i.Target.LastChildFill = true;
            }));
        }
    }
}