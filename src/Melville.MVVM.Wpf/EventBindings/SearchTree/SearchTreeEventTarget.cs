using System;
using System.Reflection;
using System.Windows;
using System.Windows.Input;

namespace Melville.MVVM.Wpf.EventBindings.SearchTree
{
  public abstract class SearchTreeTarget
  {
    protected string Method { get; }
    protected string Parameters { get; }
    private int callsRemaining;

    protected SearchTreeTarget(string method, string parameters, int callsRemaining)
    {
      Method = method;
      Parameters = parameters;
      this.callsRemaining = callsRemaining;
    }

    protected void InvokeTarget(DependencyObject target, object ea)
    {
      if (!DecrementAndQueryCallCount()) return;
      RunOnVisualTreeSearch.Run(target, Method, SearchTreeParameterParser.EvalParameters(Parameters, target, ea), out var _);
    }

    private bool DecrementAndQueryCallCount()
    {
      if (callsRemaining == int.MaxValue) return true;
      if (callsRemaining < 1) return false;
      callsRemaining--;
      return true;
    }
  }

  public sealed class SearchTreeEventTarget: SearchTreeTarget 
  {
    public SearchTreeEventTarget(string method, string parameters, int callsRemaining) : base(method, parameters, callsRemaining)
    {
    }

    private MethodInfo TargetMethodInfo() =>
      GetType().GetMethod(nameof(RunMethod), BindingFlags.NonPublic | BindingFlags.Instance) ??
      throw new InvalidProgramException("Cannot find RunMethod");

    private void RunMethod(object sender, EventArgs e) => InvokeTarget((DependencyObject)sender, e);

    public Delegate CreateDelegate(Type? returnType)
    {
      if (returnType == null) throw new InvalidOperationException("Must specify return type of the delegate.");
      return Delegate.CreateDelegate(returnType, this, TargetMethodInfo());
    }
  }

  public sealed class SearchTreeCommandTarget: SearchTreeTarget, ICommand
  {
    private DependencyObject target;
    public SearchTreeCommandTarget(DependencyObject target, string method, string parameters, int callsRemaining) : base(method, parameters, callsRemaining)
    {
      this.target = target;
    }

    public bool CanExecute(object parameter) => true;

    public void Execute(object parameter)
    {
      InvokeTarget(target, parameter);
    }

    public event EventHandler CanExecuteChanged
    {
      add {}
      remove{}
    }
  }
}
