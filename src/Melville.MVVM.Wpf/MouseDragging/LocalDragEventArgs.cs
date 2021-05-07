using  System;
using System.Windows;

namespace Melville.MVVM.Wpf.MouseDragging
{
  [Obsolete]
  public class LocalDragEventArgs : EventArgs
  {
    public Point RawPoint { get; }
    public MouseMessageType MessageType { get; }
    public Point TransformedPoint { get; }

    public LocalDragEventArgs(Point rawPoint, MouseMessageType messageType)
    { 
      RawPoint = rawPoint;
      MessageType = messageType;
      TransformedPoint = rawPoint;
    }
  }
}