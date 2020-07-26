using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Melville.Mvvm.CsXaml;
using Melville.Mvvm.CsXaml.XamlBuilders;

namespace Melville.Wpf.Samples.CsXaml
{
    public class GridCreatorViewModel
    {

    }

    public class GridCreatorView : UserControl
    {
        private static TextBlock GridBlock(string text, SolidColorBrush color) =>
            new TextBlock()
            {
                Text = text, Padding = new Thickness(15), Background = color
            };
        public GridCreatorView()
        {
            AddChild(
                BuildXaml.Create<StackPanel, GridCreatorViewModel>((i,context) =>
                {
                    i.WithChild(Create.TextBlock("Grid Examples -- Row Major Order"));
                    i.WithChild( new Grid()
                        .SetColumns("auto, 2*, *")
                        .SetRows("auto, auto")
                        .RowMajorChildren()
                            .WithChild(GridBlock("1", Brushes.Red))
                            .WithChild(1, 2, GridBlock("2", Brushes.Green))
                            .WithChild(GridBlock("3", Brushes.Blue))
                            .WithChild(GridBlock("4", Brushes.Purple))
                            .WithChild(GridBlock("5", Brushes.Orange))

                    );
                    i.WithChild(Create.TextBlock("Grid Examples -- Column Major Order"));
                    i.WithChild(new Grid()
                        .SetRows("auto, 2*, *")
                        .SetColumns("auto, auto")
                        .ColumnMajorChildren()
                            .WithChild(GridBlock("1", Brushes.Red))
                            .WithChild(1,2, GridBlock("2", Brushes.Green))
                            .WithChild(GridBlock("3", Brushes.Blue))
                            .WithChild(GridBlock("4", Brushes.Purple))
                            .WithChild(GridBlock("5", Brushes.Orange))

                    );
                    i.WithChild(Create.TextBlock("Additional Grid"));
                    i.WithChild(new Grid()
                        .SetColumns("* * *")
                        .SetRows("auto auto")
                        .RowMajorChildren()
                            .WithChild(GridBlock("1", Brushes.Red))
                            .WithChild(2, GridBlock("2 and 3", Brushes.LightBlue))
                            .WithChild(GridBlock("4", Brushes.Green))
                            .WithChild(GridBlock("5", Brushes.Yellow))
                            .WithChild(GridBlock("6", Brushes.Orange))
                    );
                }));
        }
    }
}