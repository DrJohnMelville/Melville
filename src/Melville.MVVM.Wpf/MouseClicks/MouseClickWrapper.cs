using System;
using System.Windows;
using System.Windows.Input;
using Melville.MVVM.Wpf.EventBindings;

namespace Melville.MVVM.Wpf.MouseClicks;

public class MouseClickWrapper: IParameterListExpander
{
        
    public void Push(object? value, Action<object?> target)
    {
        TryWrapMouseEventArgs(value, target);
        target(value);
    }

    private MouseButtonEventArgs? savedMouseEvent;
    private void TryWrapMouseEventArgs(object? value, Action<object?> target)
    {
        if (savedMouseEvent != null && value is FrameworkElement fe)
        {
            target(new MouseClickReport(fe, savedMouseEvent));
            savedMouseEvent = null;
        }
        else
        {
            savedMouseEvent = value as MouseButtonEventArgs ?? savedMouseEvent;
        }
    }
}