using System;

namespace Melville.MVVM.Wpf.EventBindings;

public interface IListExpander
{
    void Push(object? value, Action<object?> target);
}

public interface ITargetListExpander : IListExpander { }
public interface IParameterListExpander : IListExpander { }
public class ListExpander<T> : ITargetListExpander, IParameterListExpander
{
    private Action<T, Action<object?>> method;

    public ListExpander(Action<T, Action<object?>> method)
    {
        this.method = method;
    }

    public void Push(object? value, Action<object?> target)
    {
        if (value is T valueAsT) method(valueAsT, target);
        target(value);
    }
}