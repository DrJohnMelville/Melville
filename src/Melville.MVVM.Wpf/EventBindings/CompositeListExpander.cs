using System;
using System.Collections.Generic;
using System.Linq;

namespace Melville.MVVM.Wpf.EventBindings;

public class CompositeListExpander
{
    private Action<object?> pushAction;
    private List<object> target = new();
    public CompositeListExpander(IEnumerable<IListExpander> expanders)
    {
        pushAction = NestExpanders(expanders.Reverse());
    }

    private Action<object?> NestExpanders(IEnumerable<IListExpander> expanderList) =>
        expanderList.Aggregate<IListExpander, Action<object?>>(PushToList, 
            (current, expander) => s => expander.Push(s, current));

    private void PushToList(object? item)
    {
        if (item == null || target.Contains(item)) return;
        target.Add(item);
    }

    public IList<object> Expand(IEnumerable<object?> source)
    {
        EnsureFreshTarget();
        PushAllItems(source);
        return target;
    }

    private void PushAllItems(IEnumerable<object?> source)
    {
        foreach (var item in source)
        {
            pushAction(item);
        }
    }

    private void EnsureFreshTarget()
    {
        target = target.Count == 0 ? target : new List<object>();
    }
}