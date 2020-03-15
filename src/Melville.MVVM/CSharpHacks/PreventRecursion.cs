using  System;

namespace Melville.MVVM.CSharpHacks
{
  public sealed class PreventRecursion
  {
    private bool closed;

    public void DoNonRecursive(Action action)
    {
      if (closed) return;
      closed = true;
      action();
      closed = false;
    }
  }
}