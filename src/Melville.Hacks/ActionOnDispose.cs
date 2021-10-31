using System;

namespace Melville.Hacks;

public sealed class ActionOnDispose : IDisposable
{
  private Action? action;

  public ActionOnDispose(Action action)
  {
    this.action = action;
  }

  public void Dispose()
  {
      
    var localAction = action;
    action = null;
    if (localAction == null) return;
    localAction();
  }
}