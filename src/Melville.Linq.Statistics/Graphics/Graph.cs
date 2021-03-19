using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Threading;
using LINQPad;
using Melville.Linq.Statistics.Graphics.Gutters;
using Melville.Linq.Statistics.Graphics.Internal.Axes;
using Border = System.Windows.Controls.Border;

namespace Melville.Linq.Statistics.Graphics
{
  public sealed partial class Graph: CanCreateGraph
  {
    private readonly AxisStack xAxes = new AxisStack(()=>new XAxis());
    private readonly AxisStack yAxes = new AxisStack(()=>new YAxis());
    public override Axis CurrentAxisX =>xAxes.Current;
    public override Axis CurrentAxisY => yAxes.Current;
    public Gutter BottomGutter => xAxes.Axes.First().Gutter;
    public Gutter LeftGutter => yAxes.Axes.First().Gutter;
    public Gutter RightGutter => yAxes.Axes.Skip(1).FirstOrDefault()?.Gutter;

    public String Title { get; }
    public Graph(string title = "Untitled Graph")
    {
      Title = title;
    }

    #region LinqPad Integration
    public override object ToDump()
    {
      Console.WriteLine(new Hyperlinq(ShowInWindow, Title, false));
      return GenerateOutputVisual().ToBitmap((int)Width, (int)Height);
    }

    public override void WriteToDisk(string filePath, BitmapEncoder encoder, int width = -1, int height = -1)
    {
      if (width < 0) width = (int)Width;
      if (height < 0) height = (int)(width * Height / Width);
      var renderTargetBitmap = GenerateOutputVisual().CreateOutputBitmap(width, height, Width);
      encoder.Frames.Add(BitmapFrame.Create(renderTargetBitmap));
      using var output = File.Create(filePath);
      encoder.Save(output);
    }

    private Border GenerateOutputVisual()
    {
      var g = new Border
      {
        Background = Brushes.White,
        BorderThickness = new Thickness(1),
        BorderBrush = Brushes.Black,
        Child = PrepareInlineGraph()
      };
      g.Width = Width;
      g.Height = Height;
      var size = new Size(Width, Height);
      g.Measure(size);
      g.Arrange(new Rect(size));
      g.UpdateLayout();
      return g;
    }

    public override BitmapSource ToWpfBitmap() => GenerateOutputVisual()
      .CreateOutputBitmap((int) Width, (int) Height, Width);

    #endregion

    #region Display

    public double Height { get; set; } = 300;

    public double Width { get; set; } = 600;

    private FrameworkElement PrepareInlineGraph()
    {
      var view = CreateView();
      return view;
    }

    private bool delayAxisNotification = false;
    public override void ShowInWindow()
    {
      delayAxisNotification = true;
      var element = CreateView();
      element.Margin = new Thickness(30);
      new Window() {Content = element}.Show();
    }

    #endregion

    public FrameworkElement CreateView()
    {
      AssignColors();
      return new GraphFrame() {DataContext = this};
    }

  }
}