using System.Collections.Generic;

namespace Melville.MVVM.Wpf.RootWindows;

public interface INavigationHistory
{
    object? Pop();
    void Push(object content);
}

public class NoNavigationHistory : INavigationHistory
{
    public object? Pop() => null;
    public void Push(object content)
    {
    }
}

public class StackNavigationHistory : INavigationHistory
{
    private readonly Stack<object> stack = new Stack<object>();
    public object? Pop() => stack.TryPeek(out var ret) ? ret : null;
    public void Push(object content) => stack.Push(content);
}