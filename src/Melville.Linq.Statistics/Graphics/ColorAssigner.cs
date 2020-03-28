using System.Windows.Media;

namespace Melville.Linq.Statistics.Graphics
{
  public class ColorAssigner
  {
    public static Color[] DefaultColors = new Color[]
      {Colors.Blue, Colors.Green, Colors.Red, Colors.Brown, Colors.Purple, Colors.Gold, Colors.Fuchsia, Colors.Aqua};
    private int nextColor = 0;
    public Color Color() => DefaultColors[nextColor++ % DefaultColors.Length];
    public Pen Pen() => new Pen(new SolidColorBrush(Color()), 1.0);
    public Brush Brush() => new SolidColorBrush(Color());

  }
}