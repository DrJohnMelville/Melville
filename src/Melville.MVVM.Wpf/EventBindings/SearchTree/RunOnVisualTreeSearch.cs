using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using Melville.MVVM.CSharpHacks;
using Melville.MVVM.Wpf.EventBindings.ParameterResolution;
using Serilog;

namespace Melville.MVVM.Wpf.EventBindings.SearchTree
{
  public static class RunOnVisualTreeSearch
  {
    public static bool Run(DependencyObject root, string targetMethodName, object?[] inputParams,
      [NotNullWhen(true)]out object? result)
    {
      Log.Information("Search Tree Run Method {MethodName}", targetMethodName);
      var pathComponents = ReflectionHelper.SplitPathString(targetMethodName).ToList();
      var methodName = pathComponents.LastOrDefault() ?? "";
      var targets = AddPathFollowing(TargetSelector.ResolveTarget(root), pathComponents);

      foreach (var target in targets)
      {
        if (target == null) continue;
        Log.Debug("Searching Object: {target}", target.ToString());
        if (TryInvokeMethod(root, inputParams, target, methodName, out result)) return true;

        if (TryInvokeCommand(inputParams, methodName, target)) return true;
      }

      Log.Error("Failed to bind method: {MethodName}", targetMethodName);
      result = null;
      return false;
    }

    private static bool TryInvokeCommand(object?[] inputParams, string methodName, object target)
    {
      var commandCandidate = string.IsNullOrWhiteSpace(methodName) ? target : target.GetPath(methodName, true);
      if (commandCandidate is ICommand command)
      {
        InvokeCommand(inputParams.OfType<object>().ToArray(), command);

        return true;
      }

      return false;
    }

    private static void InvokeCommand(object[] inputParams, ICommand command)
    {
      var param = inputParams.Length == 2 ? inputParams[0] : inputParams;
      if (command.CanExecute(param))
      {
        command.Execute(param);
        MarkReturn(true, inputParams);
      }
    }

    public static bool TryInvokeMethod(DependencyObject root, object?[] inputParams, object target, string methodName,
      [NotNullWhen(true)]out object? o)
    {
      o = null;
      if (string.IsNullOrWhiteSpace(methodName)) return false;

      foreach (var candidate in CandidateMethods(target, methodName))
      {
        Log.Debug("Trying Method {methodName} on {Type}", methodName, target.GetType().Name );
        var parameters = ParameterResolver.Resolve(candidate.GetParameters(), root, inputParams);
        if (parameters != null)
        {
          var scope = parameters.GetValues(out var arguments);
          var ret = candidate.Invoke(target, arguments);
          o = ProcessMethodReturn(ret, inputParams, scope);
          return true;
        }
        else
        {
          Log.Error("Failed to bind parameters for: {methodName} on {Type}", methodName, target.GetType().Name );
          
        }
      }

      return false;
    }


    private static IEnumerable<MethodInfo> CandidateMethods(object sender,string methodName) => 
      sender
      .GetType()
      .GetMethods()
      .Where(j => j.Name.Equals(methodName, StringComparison.Ordinal));

    private static IEnumerable<object?> AddPathFollowing(IEnumerable<object> targets, List<string> pathComponents)
    {
      if (pathComponents.Count < 2)
      {
        return targets;
      }
      return targets.Select(i => i.
          FollowPathComponents(pathComponents.Take(pathComponents.Count - 1), true))
        .Where(i => i != null);
    }

    private static object? ProcessMethodReturn(object? ret, object?[] inputParams, IDisposable scope)
    {
      switch (ret)
      {
        case null: 
          scope.Dispose();
          return null;
        case bool b: 
          scope.Dispose();
          return MarkReturn(b, inputParams);
        case Task t: return AwaitAndDispose(t, scope);
        case ValueTask t : return AwaitAndDispose(t, scope);
        case var _ when IsValueTaskOfT(ret.GetType()): return DisposeOfValueTask(ret, scope);
        default: 
          scope.Dispose();
          return ret;
      }
    }

    private static object? DisposeOfValueTask(object ret, IDisposable scope)
    {
      return AwaitAndDispose((Task)ret.Call("AsTask"), scope);
    }

    private static bool IsValueTaskOfT(Type type) => type.IsConstructedGenericType && type.GetGenericTypeDefinition() == typeof(ValueTask<>);

    private static Task AwaitAndDispose(ValueTask task, IDisposable scope) => AwaitAndDispose(task.AsTask(), scope);

    private static Task AwaitAndDispose(Task task, IDisposable scope)
    {
      task.ContinueWith(i => scope.Dispose());
      return task;
    }

    private static bool MarkReturn(bool funcReturn, object?[] parameters)
    {
      foreach (var rea in parameters.OfType<RoutedEventArgs>())
      {
        rea.Handled = funcReturn;
      }
      return funcReturn;
    }

  }
}