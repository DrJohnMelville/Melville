using  System;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Media;

namespace Melville.MVVM.Wpf.Clipboards
{
  public interface IClipboardNotification
  {
    /// <summary>
    /// Occurs when the contents of the clipboard is updated.
    /// </summary>
    event EventHandler ClipboardUpdate;
  }

  public class ClipboardNotification : IClipboardNotification
  {
    /// <summary>
    /// Occurs when the contents of the clipboard is updated.
    /// </summary>
    public event EventHandler? ClipboardUpdate;

    /// <summary>
    /// Raises the <see cref="ClipboardUpdate"/> event.
    /// </summary>
    /// <param name="e">Event arguments for the event.</param>
    private void OnClipboardUpdate(EventArgs e) => ClipboardUpdate?.Invoke(null, e);

    public void SetHook(Visual win)
    {
      if (PresentationSource.FromVisual(win) is HwndSource src)
      {
        src.AddHook(WindowProc);
        NativeMethods.AddClipboardFormatListener(src.Handle);
      }
    }

    private IntPtr WindowProc(IntPtr hwnd, int msg, IntPtr wparam, IntPtr lparam, ref bool handled)
    {
      if (msg == NativeMethods.WM_CLIPBOARDUPDATE)
      {
        OnClipboardUpdate(EventArgs.Empty);
      }

      return IntPtr.Zero;

    }
  }

  static class NativeMethods
  {
    // See http://msdn.microsoft.com/en-us/library/ms649021%28v=vs.85%29.aspx
    public const int WM_CLIPBOARDUPDATE = 0x031D;
    public static IntPtr HWND_MESSAGE = new IntPtr(-3);
    // See http://msdn.microsoft.com/en-us/library/ms632599%28VS.85%29.aspx#message_only
    [DllImport("user32.dll", SetLastError = true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    public static extern bool AddClipboardFormatListener(IntPtr hwnd);

    // See http://msdn.microsoft.com/en-us/library/ms633541%28v=vs.85%29.aspx
    // See http://msdn.microsoft.com/en-us/library/ms649033%28VS.85%29.aspx
    [DllImport("user32.dll", SetLastError = true)]
    public static extern IntPtr SetParent(IntPtr hWndChild, IntPtr hWndNewParent);
  }
}