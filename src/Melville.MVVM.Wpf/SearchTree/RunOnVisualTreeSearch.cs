using  System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Input;
using Melville.MVVM.CSharpHacks;
using Melville.MVVM.Wpf.EventBindings;
using Serilog;
using Serilog.Events;

namespace Melville.MVVM.Wpf.SearchTree
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
          var ret = candidate.Invoke(target, parameters.ToArray());
          MarkReturn(ret as bool?, inputParams);
          {
            o = ret;
            return true;
          }
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

    private static void MarkReturn(bool? funcReturn, object?[] parameters)
    {
      if (!funcReturn.HasValue) return;
      foreach (var rea in parameters.OfType<RoutedEventArgs>())
      {
        rea.Handled = funcReturn.Value;
      }
    }

  }
}