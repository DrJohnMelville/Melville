using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Input;
using Melville.Hacks.Reflection;
using Melville.INPC;
using Melville.MVVM.Wpf.DiParameterSources;
using Melville.MVVM.Wpf.EventBindings.ParameterResolution;
using Melville.MVVM.Wpf.VisualTreeLocations;
using Microsoft.Extensions.Logging;

namespace Melville.MVVM.Wpf.EventBindings.SearchTree;

public interface IVisualTreeRunner: IVisualTreeLocation<IVisualTreeRunner, DependencyObject>
{
    public bool RunTreeSearch(string targetMethodName, object?[] inputParams, out object? result);
    public bool RunOnTarget(object target, string targetMethodName, object?[] inputParams, out object? result);
    public bool RunMethod(Delegate function, object?[] parameters, out object? result);
}

public partial class VisualTreeRunner : IVisualTreeRunner
{
    [FromConstructor] private readonly DependencyObject root;
    
    DependencyObject IVisualTreeLocation<IVisualTreeRunner, DependencyObject>.Target => root;
    IVisualTreeRunner IVisualTreeLocation<IVisualTreeRunner, DependencyObject>.CreateNewChild(
        DependencyObject? target) => 
        target == null? this:new VisualTreeRunner(target);

    public static event EventHandler<VisualTreeRunContext>? RunStarted;
    public static event EventHandler<VisualTreeRunContext>? RunFailed;

    public bool RunTreeSearch(string targetMethodName, object?[] inputParams, out object? result)
    {
        var pathComponents = ReflectionHelper.SplitPathString(targetMethodName).ToList();
        var methodName = pathComponents.LastOrDefault() ?? "";
        var context = CreateContext(inputParams, methodName);
        RunStarted?.Invoke(this, context);
        var targets = AddPathFollowing(context.Targets, pathComponents);

        foreach (var target in targets)
        {
            if (target == null) continue;
            if (RunOnTarget(target, ref context, out result)) return true;
        }

        RunFailed?.Invoke(this, context);
        result = null;
        return false;
    }

    private VisualTreeRunContext CreateContext(object?[] inputParams, string methodName) =>
        new(DiIntegration.SearchForContainer(root), root, methodName, 
            inputParams.Append(this));

    public bool RunOnTarget(object? target, string targetMethodName, object?[] inputParams, out object? result)
    {
        var context = CreateContext(inputParams, targetMethodName);
        return RunOnTarget(target, ref context, out result);
    }

    private bool RunOnTarget(object? target, ref VisualTreeRunContext context, out object? result)
    {
        result = null;
        if (target == null) return false;
        return 
            TryInvokeMethod(ref context, target, out result) || 
            TryInvokeCommand(ref context, target);
    }

    public object? Run(Delegate target, params object?[] parameters)
    {
        RunMethod(target, parameters, out var result);
        return result;
    }
    
    public bool RunMethod(Delegate function, object?[] parameters, out object? result)
    {
        var context = CreateContext(parameters, "");
        return InnerRunSingleMethod(ref context, function.Target, function.Method, out result);
    }

    private bool TryInvokeCommand(ref VisualTreeRunContext context, object target)
    {
        var commandCandidate = string.IsNullOrWhiteSpace(context.TargetMethodName) ? target : 
            target.GetPath(context.TargetMethodName, true);
        if (commandCandidate is ICommand command)
        {
            InvokeCommand(context.InputParameters.ToArray(), command);
            return true;
        }
        return false;
    }

    private void InvokeCommand(object?[] inputParams, ICommand command)
    {
        var param = inputParams.Length >= 2 ? inputParams[0] : inputParams;
        if (command.CanExecute(param))
        {
            command.Execute(param);
            MarkReturn(true, inputParams);
        }
    }

    private  bool TryInvokeMethod(ref VisualTreeRunContext context, object target, out object? o)
    {
        o = null;
        if (string.IsNullOrWhiteSpace(context.TargetMethodName)) return false;

        foreach (var candidate in CandidateMethods(target, context.TargetMethodName))
        {
            if (InnerRunSingleMethod(ref context, target, candidate, out o)) return true;
        }

        return false;
    }

    private bool InnerRunSingleMethod(ref VisualTreeRunContext context, object? target, MethodInfo candidate,
        out object? o)
    {
        o = null;
        var log = context.DIIntegration.Get(typeof(Microsoft.Extensions.Logging.ILogger<VisualTreeRunner>))
            as Microsoft.Extensions.Logging.ILogger<VisualTreeRunner>;
        log?.LogDebug("Trying Method {Candidate} on {Name}",
            candidate.Name, candidate.DeclaringType?.Name);
        var parameters = ParameterResolver.Resolve(candidate.GetParameters(), ref context);
        if (parameters != null)
        {
            // we do not use the using syntax on scope because we are in a immediate context
            // but we might be calling an async handler.  If so, CleanupAfterCompletion uses a
            // continuation passing style to defer the disposal until after the async
            // task completes.
            var scope = parameters.Value.GetValues(ref context, out var arguments);
            var ret = candidate.Invoke(target, arguments);
            o = CleanUpAfterCompletion(ret, context.InputParameters, scope);
            return true;
        }
        else
        {
            log?.LogError("Failed to bind parameters for: {Candidate} on {CandidateType}",
            candidate.Name, candidate.DeclaringType?.Name);
        }

        return false;
    }


    private IEnumerable<MethodInfo> CandidateMethods(object sender, string methodName) =>
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

        return targets.Select(i => i.FollowPathComponents(pathComponents.Take(pathComponents.Count - 1), true))
            .Where(i => i != null);
    }

    private object? CleanUpAfterCompletion(object? ret, object?[] inputParams, IDisposable scope)
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
            case ValueTask t: return AwaitAndDispose(t, scope);
            case var _ when IsValueTaskOfT(ret.GetType()): return DisposeOfValueTask(ret, scope);
            default:
                scope.Dispose();
                return ret;
        }
    }

    private object DisposeOfValueTask(object ret, IDisposable scope)
    {
        return AwaitAndDispose((Task) (ret.Call("AsTask") ??
                                       throw new InvalidOperationException("Method did not return a value.")),
            scope);
    }

    private bool IsValueTaskOfT(Type type) =>
        type.IsConstructedGenericType && type.GetGenericTypeDefinition() == typeof(ValueTask<>);

    private static Task AwaitAndDispose(ValueTask task, IDisposable scope) => AwaitAndDispose(task.AsTask(), scope);

    private static Task AwaitAndDispose(Task task, IDisposable scope)
    {
        task.ContinueWith(i => scope.Dispose());
        return task;
    }

    private bool MarkReturn(bool funcReturn, object?[] parameters)
    {
        foreach (var rea in parameters.OfType<RoutedEventArgs>())
        {
            rea.Handled = funcReturn;
        }
        return funcReturn;
    }

    private object? RunDelegate(Delegate function)
    {
        var decl = function.GetInvocationList().First();
        if (decl.Target == null || decl.Method == null)
            throw new InvalidOperationException("Must be a Func or Action to execute.");
        var context = CreateContext(Array.Empty<object>(), "");
        InnerRunSingleMethod(ref context, decl.Target, decl.Method, out var result);
        return result;
    }
}