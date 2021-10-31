using  System;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Melville.MVVM.Wpf.MouseDragging.LocalDraggers;
using Serilog.Data;

namespace Melville.MVVM.Wpf.MouseDragging.Drag;

public static class DragUIWindowOperations
{
  public static IMouseDataSource DragTarget(this IMouseDataSource src, double opacity = 1)
  {
    if (src.Target is not FrameworkElement target) return src;
    src.BindLocalDragger(
      LocalDragger.MinimumDrag(new DragUiWindow(target, opacity)));
    return src;
  }
}

public sealed class DragUiWindow : Window, ILocalDragger<Point>
{

  private readonly FrameworkElement target;
  public DragUiWindow(FrameworkElement targetElement, double opacity)
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
  }

  private static BitmapSource CaptureVisual(FrameworkElement target)
  {
    var bitmap = new RenderTargetBitmap((int) Math.Ceiling(target.ActualWidth),
      (int) Math.Ceiling(target.ActualHeight), 96.0, 96.0, PixelFormats.Pbgra32);
    var visual = new DrawingVisual();
    using (var context = visual.RenderOpen())
    {
      var brush = new VisualBrush(target);
      var descendantBounds = VisualTreeHelper.GetDescendantBounds(target);
      var rectangle = new Rect(new Point(0.0, 0.0), descendantBounds.Size);
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
  private void MakeWindowInvisibleToMouse(object? _, EventArgs __)
  {
    PresentationSource windowSource = PresentationSource.FromVisual(this);
    IntPtr handle = ((HwndSource) windowSource).Handle;
    Int32 styles = NativeMethods.GetWindowLong(handle, NativeMethods.GWL_EXSTYLE);
    NativeMethods.SetWindowLong(handle, NativeMethods.GWL_EXSTYLE,
      styles | NativeMethods.WS_EX_LAYERED | NativeMethods.WS_EX_TRANSPARENT);
  }

  public void FinishDrag()
  {
    if (IsVisible) Close();
    target.Visibility = Visibility.Visible;
  }

  public void NewPoint(MouseMessageType type, Point point)
  {
    TryShowOrHideWindow(type, point);
    UpdateWindowLocation();
  }

  private void TryShowOrHideWindow(MouseMessageType type, Point point)
  {
    switch (type)
    {
      case MouseMessageType.Down:
        SwapTargetWithProxyWindow(point);
        break;
      case MouseMessageType.Up:
        FinishDrag();
        break;
    }
  }

  private void SwapTargetWithProxyWindow(Point point)
  {
    offset = point;
    target.Visibility = Visibility.Hidden;
    Show();
  }

  private Point offset;
  private void UpdateWindowLocation()
  {
    if (!NativeMethods.GetCursorPos(out var p)) return;
    Left = p.X - offset.X;
    Top = p.Y - offset.Y;
  }

  private class NativeMethods
  {
    [StructLayout(LayoutKind.Sequential)]
    public struct Point
    {
      public int X;
      public int Y;
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