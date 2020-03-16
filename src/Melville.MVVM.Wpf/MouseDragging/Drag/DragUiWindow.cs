using  System;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Melville.MVVM.Wpf.MouseDragging.Drag
{
  public static class DragUIWindowOperations
  {
    public static IMouseDataSource DragTarget(this IMouseDataSource src, double opacity = 1)
    {
      IDragUIWindow? window = null;

      Point HandleMouseMove(LocalDragEventArgs ea)
      {
        if (window == null)
        {
          if (ea.MessageType != MouseMessageType.Down) return ea.TransformedPoint;
          window = src.Root.ConstructDragWindow(ea.Target, opacity);
          window.Show();
        }

        window.MouseMoved(ea);
        if (ea.MessageType == MouseMessageType.Up)
        {
          window = null;
        }

        return ea.TransformedPoint;
      }

      return LambdaMouseMonitor.FromFull(src, HandleMouseMove);
    }
  }

  public interface IDragUIWindow
  {
    void MouseMoved(LocalDragEventArgs e);
    void Show();
  }

  public sealed class DragUIWindow : Window, IDragUIWindow
  {
    private readonly FrameworkElement target;
    public DragUIWindow(FrameworkElement targetElement, double opacity)
    {
      this.target = targetElement;
      WindowStyle = WindowStyle.None;
      AllowsTransparency = true;
      AllowDrop = false;
      Background = null;
      IsHitTestVisible = false;
      SizeToContent = SizeToContent.WidthAndHeight;
      Topmost = true;
      ShowInTaskbar = false;
      Opacity = opacity;
      Content = new Image()
      {
        Width = targetElement.ActualWidth,
        Height = targetElement.ActualHeight,
        Source = CaptureVisual(targetElement)
      };
      SourceInitialized += MakeWindowInvisibleToMouse;
      target.Visibility = Visibility.Hidden;
    }

    private static BitmapSource CaptureVisual(FrameworkElement target)
    {
      RenderTargetBitmap bitmap = new RenderTargetBitmap((int) Math.Ceiling(target.ActualWidth),
        (int) Math.Ceiling(target.ActualHeight), 96.0, 96.0, PixelFormats.Pbgra32);
      DrawingVisual visual = new DrawingVisual();
      using (DrawingContext context = visual.RenderOpen())
      {
        VisualBrush brush = new VisualBrush(target);
        Rect descendantBounds = VisualTreeHelper.GetDescendantBounds(target);
        Rect rectangle = new Rect(new Point(0.0, 0.0), descendantBounds.Size);
        context.DrawRectangle(brush, null, rectangle);
      }

      bitmap.Render(visual);
      bitmap.Freeze();
      return bitmap;
    }

    /// <summary>
    /// This window is going to be always under the mouse cursor.  If the Mouse could see this window
    /// then drop will never happen because this window does not accept drops.  The window styles below
    /// make it so the drag and drop system ignores this window in picking a drop target.
    /// </summary>
    /// <param name="sender">Ignored, required to call this as an event handler</param>
    /// <param name="args">Ignored, required to call this as an event handler</param>
    private void MakeWindowInvisibleToMouse(object? sender, EventArgs args)
    {
      PresentationSource windowSource = PresentationSource.FromVisual(this);
      IntPtr handle = ((HwndSource) windowSource).Handle;
      Int32 styles = NativeMethods.GetWindowLong(handle, NativeMethods.GWL_EXSTYLE);
      NativeMethods.SetWindowLong(handle, NativeMethods.GWL_EXSTYLE,
        styles | NativeMethods.WS_EX_LAYERED | NativeMethods.WS_EX_TRANSPARENT);
    }

    public void FinishDrag()
    {
      Close();
      target.Visibility = Visibility.Visible;
    }

    public void MouseMoved(LocalDragEventArgs e)
    {
      UpdateWindowLocation();

      switch (e.MessageType)
      {
        case MouseMessageType.Down:
          offset = e.RawPoint;
          break;
        case MouseMessageType.Up:
          FinishDrag();
          break;
      }
    }



    private Point offset;
    private void UpdateWindowLocation()
    {
      if (NativeMethods.GetCursorPos(out var p))
      {
        Left = -30 + ((double) p.X) - offset.X;
        Top = ((double) p.Y) - offset.Y;
      }
    }

    private class NativeMethods
    {
      [StructLayout(LayoutKind.Sequential)]
      public struct Point
      {
        public int X;
        public int Y;

//        public Point(int x, int y)
//        {
//          this.X = x;
//          this.Y = y;
//        }
      }

      public static Int32 GWL_EXSTYLE = -20;
      public static Int32 WS_EX_LAYERED = 0x00080000;
      public static Int32 WS_EX_TRANSPARENT = 0x00000020;


      [DllImport("user32.dll", CharSet = CharSet.Auto)]
      public static extern bool GetCursorPos(out Point pt);
      [DllImport("user32.dll", CharSet = CharSet.Auto)]
      public static extern Int32 GetWindowLong(IntPtr hWnd, Int32 nIndex);

      [DllImport("user32.dll", CharSet = CharSet.Auto)]
      public static extern Int32 SetWindowLong(IntPtr hWnd, Int32 nIndex, Int32 newVal);
    }
  }
}