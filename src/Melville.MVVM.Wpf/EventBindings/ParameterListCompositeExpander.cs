using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Melville.MVVM.Wpf.ContextMenus;
using Melville.MVVM.Wpf.KeyboardFacade;
using Melville.MVVM.Wpf.MouseClicks;

namespace Melville.MVVM.Wpf.EventBindings;

public sealed class ParameterListCompositeExpander: CompositeListExpander
{
    private static IParameterListExpander[] builtInExpanders =
    {
        new MouseClickWrapper(),
        new KeyEventWrapper(),
        new EventBindingParameterExpander(),
        new ListExpander<ContextMenuEventArgs>((e,target)=>target.Invoke(new ContextMenuWrapper(e)))
    };
    public ParameterListCompositeExpander(IEnumerable<IParameterListExpander> expanders) : base(
        builtInExpanders.Concat(expanders))
    {
    }
}

internal class EventBindingParameterExpander : IParameterListExpander
{
    public void Push(object? value, Action<object?> target)
    {
        if (value is DependencyObject depObj &&
            EventBinding.GetParameter(depObj) is { } item)
                target(item);
        target(value);
    }
}