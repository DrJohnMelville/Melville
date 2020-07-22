using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Melville.Mvvm.CsXaml;

namespace Melville.Wpf.Samples.CsXaml
{
    public class GridCreatorViewModel
    {

    }

    public class GridCreatorView : UserControl
    {
        public GridCreatorView()
        {
            AddChild(
                BuildXaml.Create<StackPanel, GridCreatorViewModel>(i =>
                {
                    i.ChildTextBlock("Grid Examples -- Row Major Order");
                    i.Child<Grid>(j =>
                    {
                        j.SetColumns("auto, 2*, *");
                        j.SetRows("auto, auto");
                        j.RowMajorChildren()
                            .AddToGrid(GridBlock(j, "1", Brushes.Red))
                            .AddToGrid(GridBlock(j, "2", Brushes.Green), 1, 2)
                            .AddToGrid(GridBlock(j, "3", Brushes.Blue))
                            .AddToGrid(GridBlock(j, "4", Brushes.Purple))
                            .AddToGrid(GridBlock(j, "5", Brushes.Orange));

                    });
                    i.ChildTextBlock("Grid Examples -- Column Major Order");
                    i.Child<Grid>(j =>
                    {
                        j.SetRows("auto, 2*, *");
                        j.SetColumns("auto, auto");
                        j.ColumnMajorChildren()
                            .AddToGrid(GridBlock(j, "1", Brushes.Red))
                            .AddToGrid(GridBlock(j, "2", Brushes.Green), 1, 2)
                            .AddToGrid(GridBlock(j, "3", Brushes.Blue))
                            .AddToGrid(GridBlock(j, "4", Brushes.Purple))
                            .AddToGrid(GridBlock(j, "5", Brushes.Orange));

                    });
                    i.ChildTextBlock("Additional Grid");
                    i.Child<Grid>(j =>
                    {
                        j.SetColumns("* * *");
                        j.SetRows("auto auto");
                        j.RowMajorChildren()
                            .AddToGrid(GridBlock(j, "1", Brushes.Red))
                            .AddToGrid(GridBlock(j, "2 and 3", Brushes.LightBlue), 2)
                            .AddToGrid(GridBlock(j, "4", Brushes.Green))
                            .AddToGrid(GridBlock(j, "5", Brushes.Yellow))
                            .AddToGrid(GridBlock(j, "6", Brushes.Orange));
                    });
                }));
        }

        private static TextBlock GridBlock(XamlBuilder<Grid, GridCreatorViewModel> j, string text, SolidColorBrush color)
        {
            var childTextBlock = j.ChildTextBlock(text);
            childTextBlock.Background = color;
            childTextBlock.Padding = new Thickness(15);
            return childTextBlock;
        }
    }


}