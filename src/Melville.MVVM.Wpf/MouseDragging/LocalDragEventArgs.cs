using  System;
using System.Windows;
using System.Windows.Input;

namespace Melville.MVVM.Wpf.MouseDragging
{
  public class LocalDragEventArgs : EventArgs
  {
    public Point RawPoint { get; }
    public MouseMessageType MessageType { get; }
    public Size TargetSize { get; }
    public Point TransformedPoint { get; set; }
    public MouseButton ButtonUsed { get; }
    public FrameworkElement Target { get; }

    public LocalDragEventArgs(Point rawPoint, MouseMessageType messageType, Size targetSize, MouseButton buttonUsed,
      FrameworkElement target)
    { 
      RawPoint = rawPoint;
      MessageType = messageType;
      TargetSize = targetSize;
      ButtonUsed = buttonUsed;
      Target = target;
      TransformedPoint = rawPoint;
    }
  }
}