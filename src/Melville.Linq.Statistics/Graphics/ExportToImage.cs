using System;
using System.Drawing;
using System.IO;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Melville.Linq.Statistics.Graphics
{
  public static class ExportToImageImplementation
  {
    public static Bitmap ToBitmap(this Visual visual, int width, int height)
    {
      var rtb = CreateOutputBitmap(visual, width, height, width);
      var dest = new MemoryStream();
      WriteBitmapToDisk(rtb, dest);
      var src = new MemoryStream(dest.ToArray());
      return new Bitmap(src);
    }
    public static void WriteBitmapToDisk(BitmapSource bitmap,
      Stream outputStream)
    {
          var encoder = CreateEncoder();
          encoder.Frames.Add(BitmapFrame.Create(bitmap));
          encoder.Save(outputStream);
    }

    private static BitmapEncoder CreateEncoder()
    {
      return new BmpBitmapEncoder();
    }
    public static RenderTargetBitmap CreateOutputBitmap(this Visual snapshotHost, int width, int height, double srcwidth)
    {
      var zoom = srcwidth / width;
      var bitmap = new RenderTargetBitmap(width, height, 96 / zoom, 96 / zoom, PixelFormats.Pbgra32);
      bitmap.Render(snapshotHost);
      bitmap.Freeze();
      return bitmap;
    }
  }
}
