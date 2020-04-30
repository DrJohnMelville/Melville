using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Diagnostics.Eventing.Reader;
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
    //Note that result may be null even with a successful execution.  The method could execute and return null.
    public static bool Run(DependencyObject root, string targetMethodName, object?[] inputParams,
      out object? result)
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
      out object? o)
    {
      o = null;
      if (string.IsNullOrWhiteSpace(methodName)) return false;

      foreach (var candidate in CandidateMethods(target, methodName))
      {
        if (TryRunSingleMethod(root, inputParams, target, candidate, out o)) return true;
      }

      return false;
    }

    private static bool TryRunSingleMethod(DependencyObject root, object?[] inputParams, object target, 
      MethodInfo candidate, out object? o)
    {
      o = null;
      Log.Debug("Trying Method {methodName} on {Type}", candidate.Name, candidate.DeclaringType?.Name);
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
        Log.Error($"Failed to bind parameters for: {candidate.Name} on {candidate.DeclaringType?.Name}");
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
      return AwaitAndDispose((Task)(ret.Call("AsTask")??
        throw new InvalidOperationException("Method did not return a value.")), scope);
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
    
    // Run arbitrary methods on the visual tree
public static void Run<T1>(DependencyObject root, Action<T1> function)=> 
        RunDelegate(root, function);
    public static TR Run<T1, TR>(DependencyObject root, Func<T1,TR> function) => 
      (TR)RunDelegate(root, function)!;
public static void Run<T1, T2>(DependencyObject root, Action<T1, T2> function)=> 
        RunDelegate(root, function);
    public static TR Run<T1, T2, TR>(DependencyObject root, Func<T1, T2,TR> function) => 
      (TR)RunDelegate(root, function)!;
public static void Run<T1, T2, T3>(DependencyObject root, Action<T1, T2, T3> function)=> 
        RunDelegate(root, function);
    public static TR Run<T1, T2, T3, TR>(DependencyObject root, Func<T1, T2, T3,TR> function) => 
      (TR)RunDelegate(root, function)!;
public static void Run<T1, T2, T3, T4>(DependencyObject root, Action<T1, T2, T3, T4> function)=> 
        RunDelegate(root, function);
    public static TR Run<T1, T2, T3, T4, TR>(DependencyObject root, Func<T1, T2, T3, T4,TR> function) => 
      (TR)RunDelegate(root, function)!;
public static void Run<T1, T2, T3, T4, T5>(DependencyObject root, Action<T1, T2, T3, T4, T5> function)=> 
        RunDelegate(root, function);
    public static TR Run<T1, T2, T3, T4, T5, TR>(DependencyObject root, Func<T1, T2, T3, T4, T5,TR> function) => 
      (TR)RunDelegate(root, function)!;
public static void Run<T1, T2, T3, T4, T5, T6>(DependencyObject root, Action<T1, T2, T3, T4, T5, T6> function)=> 
        RunDelegate(root, function);
    public static TR Run<T1, T2, T3, T4, T5, T6, TR>(DependencyObject root, Func<T1, T2, T3, T4, T5, T6,TR> function) => 
      (TR)RunDelegate(root, function)!;
public static void Run<T1, T2, T3, T4, T5, T6, T7>(DependencyObject root, Action<T1, T2, T3, T4, T5, T6, T7> function)=> 
        RunDelegate(root, function);
    public static TR Run<T1, T2, T3, T4, T5, T6, T7, TR>(DependencyObject root, Func<T1, T2, T3, T4, T5, T6, T7,TR> function) => 
      (TR)RunDelegate(root, function)!;
public static void Run<T1, T2, T3, T4, T5, T6, T7, T8>(DependencyObject root, Action<T1, T2, T3, T4, T5, T6, T7, T8> function)=> 
        RunDelegate(root, function);
    public static TR Run<T1, T2, T3, T4, T5, T6, T7, T8, TR>(DependencyObject root, Func<T1, T2, T3, T4, T5, T6, T7, T8,TR> function) => 
      (TR)RunDelegate(root, function)!;
public static void Run<T1, T2, T3, T4, T5, T6, T7, T8, T9>(DependencyObject root, Action<T1, T2, T3, T4, T5, T6, T7, T8, T9> function)=> 
        RunDelegate(root, function);
    public static TR Run<T1, T2, T3, T4, T5, T6, T7, T8, T9, TR>(DependencyObject root, Func<T1, T2, T3, T4, T5, T6, T7, T8, T9,TR> function) => 
      (TR)RunDelegate(root, function)!;
public static void Run<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(DependencyObject root, Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> function)=> 
        RunDelegate(root, function);
    public static TR Run<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TR>(DependencyObject root, Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10,TR> function) => 
      (TR)RunDelegate(root, function)!;
public static void Run<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(DependencyObject root, Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11> function)=> 
        RunDelegate(root, function);
    public static TR Run<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TR>(DependencyObject root, Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11,TR> function) => 
      (TR)RunDelegate(root, function)!;
public static void Run<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(DependencyObject root, Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12> function)=> 
        RunDelegate(root, function);
    public static TR Run<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TR>(DependencyObject root, Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12,TR> function) => 
      (TR)RunDelegate(root, function)!;
public static void Run<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>(DependencyObject root, Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13> function)=> 
        RunDelegate(root, function);
    public static TR Run<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TR>(DependencyObject root, Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13,TR> function) => 
      (TR)RunDelegate(root, function)!;
public static void Run<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>(DependencyObject root, Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14> function)=> 
        RunDelegate(root, function);
    public static TR Run<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TR>(DependencyObject root, Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14,TR> function) => 
      (TR)RunDelegate(root, function)!;
public static void Run<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>(DependencyObject root, Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15> function)=> 
        RunDelegate(root, function);
    public static TR Run<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TR>(DependencyObject root, Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15,TR> function) => 
      (TR)RunDelegate(root, function)!;
public static void Run<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16>(DependencyObject root, Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16> function)=> 
        RunDelegate(root, function);
    public static TR Run<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, TR>(DependencyObject root, Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16,TR> function) => 
      (TR)RunDelegate(root, function)!;
    private static object? RunDelegate(DependencyObject root, Delegate function)
    {
      var decl = function.GetInvocationList().First();
      if (decl.Target == null || decl.Method == null)
        throw new InvalidOperationException("Must be a Func or Action to execute.");
      TryRunSingleMethod(root, Array.Empty<object>(), decl.Target, decl.Method,  out var result);
      return result;
    }


  }
}