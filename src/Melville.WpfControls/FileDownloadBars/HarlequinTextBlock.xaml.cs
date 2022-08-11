using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;
using Melville.INPC;
using Melville.MVVM.Wpf.Bindings;

namespace Melville.WpfControls.FileDownloadBars;

[GenerateDP(typeof(Brush), "BarBrush", Default = "@Brushes.Blue")]
[GenerateDP(typeof(Brush), "NonBarBrush", Default = "@Brushes.White")]
[GenerateDP(typeof(double), "Offset")]
[GenerateDP(typeof(string), "Text", Default = "Default Text")]
public partial class HarlequinTextBlock : UserControl
{
    public HarlequinTextBlock()
    { 
        InitializeComponent();
    }

    public static readonly IMultiValueConverter ClipConverter = LambdaConverter.Create(
        (double width, double height, double fraction) =>
            new RectangleGeometry(new Rect(0, 0, width * fraction, height))
    );
}