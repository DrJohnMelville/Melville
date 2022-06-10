using System;
using System.Windows;
using Melville.INPC;

namespace Melville.MVVM.Wpf.MouseDragging.LocalDraggers;

public partial class LambdaDragger<T> : ILocalDragger<T> where T : struct
{
    [FromConstructor] private readonly Action<MouseMessageType, T> action;

    public void NewPoint(MouseMessageType type, T point) =>
        action(type, point);
}