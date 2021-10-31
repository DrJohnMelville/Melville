using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;

namespace Melville.Linq;

public static class NotifyCollectionChangedOperations
{
    public static void NotifyAllAndMonitorCollection<T>(this IEnumerable<T> list,
        Action<T>? added = null, Action<T>? removed = null, Action? clear = null)
    {
        RunMethod(added, list);
        list.MonitorCollection(added, removed, clear);
    }
    public static void MonitorCollection<T>(this IEnumerable<T> list, 
        Action<T>? added = null, Action<T>? removed = null, Action? clear = null)
    {
        if (list is INotifyCollectionChanged incc)
        {
            incc.CollectionChanged += (s, e) =>
            {
                switch (e.Action)
                {
                    case NotifyCollectionChangedAction.Add:
                        RunMethod(added, e.NewItems);
                        break;
                    case NotifyCollectionChangedAction.Remove:
                        RunMethod(removed, e.OldItems);
                        break;
                    case NotifyCollectionChangedAction.Replace:
                        RunMethod(removed, e.OldItems);
                        RunMethod(added, e.NewItems);
                        break;
                    case NotifyCollectionChangedAction.Move:
                        break;
                    case NotifyCollectionChangedAction.Reset:
                        clear?.Invoke();
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            };
        }
    }

    private static void RunMethod<T>(Action<T>? operation, IEnumerable? list)
    {
        if (operation is null || list is null) return;
        foreach (var item in list.OfType<T>())
        {
            operation(item);
        }
    }
}