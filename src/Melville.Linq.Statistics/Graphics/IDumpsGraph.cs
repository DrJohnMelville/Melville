using System.Windows.Media.Imaging;

namespace Melville.Linq.Statistics.Graphics
{
  public interface IDumpsGraph
  {
    object ToDump();
    BitmapSource ToWpfBitmap();
    void ShowInWindow();
  }
}