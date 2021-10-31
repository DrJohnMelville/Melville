using System;
using System.Windows;

namespace Melville.MVVM.Wpf.MouseDragging.LocalDraggers;

public class LambdaDragger<T> : ILocalDragger<T> where T : struct
{
    private readonly Action<MouseMessageType, T> action;
    public LambdaDragger(Action<MouseMessageType, T> action)
    {
        this.action = action;
    }

    public void NewPoint(MouseMessageType type, T point) =>
        action(type, point);
}