using  System;
using System.ComponentModel;
using System.Reflection;
using System.Windows;
using Melville.MVVM.Wpf.EventBindings.SearchTree;

namespace Melville.MVVM.Wpf.EventBindings;

public class EventIocSetter : EventSetter, ISupportInitialize
{
  public string MethodName { get; set; } = "";
  public string Parameters { get; set; } = "";

  private MethodInfo TargetMethodInfo() =>
    GetType().GetMethod(nameof(HandleEvent), BindingFlags.NonPublic | BindingFlags.Instance) ??
    throw new InvalidOperationException("Could not find the method to call");
    
  private void HandleEvent(object o, EventArgs e)
  {
    if(o is DependencyObject dependencyObject)
    {
      object?[] inputParams = SearchTreeParameterParser.EvalParameters(Parameters, dependencyObject, e);
      new VisualTreeRunner(dependencyObject).RunTreeSearch(MethodName, inputParams, out var result);
    }
  }

  public void BeginInit()
  {
  }

  public void EndInit()
  {
    if (Handler != null)
    {
      throw new InvalidOperationException("Do not set Handler of EventIocSetter, use MethodName instead");
    }
    Handler = Delegate.CreateDelegate(Event.HandlerType, this, TargetMethodInfo());
  }
}