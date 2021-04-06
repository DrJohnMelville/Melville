using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using Melville.Hacks.Reflection;
using Melville.MVVM.Wpf.EventBindings.ParameterResolution;
using Serilog;

namespace Melville.MVVM.Wpf.EventBindings.SearchTree
{
    public interface IVisualTreeRunner
    {
        public bool RunTreeSearch(string targetMethodName, object?[] inputParams, out object? result);
        public bool RunOnTarget(object target, string targetMethodName, object?[] inputParams, out object? result);
        public bool RunMethod(Delegate function, object?[] parameters, out object? result);
    }

    public class VisualTreeRunner : IVisualTreeRunner
    {
        private readonly DependencyObject root;

        public VisualTreeRunner(DependencyObject root)
        {
            this.root = root;
        }

        public bool RunTreeSearch(string targetMethodName, object?[] inputParams, out object? result)
        {
            Log.Information("Search Tree Run Method {MethodName}", targetMethodName);
            var pathComponents = ReflectionHelper.SplitPathString(targetMethodName).ToList();
            var methodName = pathComponents.LastOrDefault() ?? "";
            var targets = AddPathFollowing(TargetSelector.ResolveTarget(root), pathComponents);

            foreach (var target in targets)
            {
                if (target == null) continue;
                Log.Debug("Searching Object: {target}", target.ToString());
                if (RunOnTarget(target, methodName, inputParams, out result)) return true;
            }

            Log.Error("Failed to bind method: {MethodName}", targetMethodName);
            result = null;
            return false;
        }

        public bool RunOnTarget(object? target, string targetMethodName, object?[] inputParams, out object? result)
        {
            result = null;
            if (target == null) return false;
            Log.Debug("Searching Object: {target}", target.ToString());
            return 
                TryInvokeMethod(inputParams, target, targetMethodName, out result) || 
                TryInvokeCommand(inputParams, targetMethodName, target);
        }

        public bool RunMethod(Delegate function, object?[] parameters, out object? result) => 
            TryRunSingleMethod(parameters, function.Target, function.Method, out result);

        private bool TryInvokeCommand(object?[] inputParams, string methodName, object target)
        {
            var commandCandidate = string.IsNullOrWhiteSpace(methodName) ? target : target.GetPath(methodName, true);
            if (commandCandidate is ICommand command)
            {
                InvokeCommand(inputParams.OfType<object>().ToArray(), command);

                return true;
            }

            return false;
        }

        private void InvokeCommand(object[] inputParams, ICommand command)
        {
            var param = inputParams.Length == 2 ? inputParams[0] : inputParams;
            if (command.CanExecute(param))
            {
                command.Execute(param);
                MarkReturn(true, inputParams);
            }
        }

        private  bool TryInvokeMethod(object?[] inputParams, object target,
            string methodName,
            out object? o)
        {
            o = null;
            if (string.IsNullOrWhiteSpace(methodName)) return false;

            foreach (var candidate in CandidateMethods(target, methodName))
            {
                if (TryRunSingleMethod(inputParams, target, candidate, out o)) return true;
            }

            return false;
        }

        private bool TryRunSingleMethod(object?[] inputParams, object? target,
            MethodInfo candidate, out object? o)
        {
            o = null;
            Log.Debug("Trying Method {methodName} on {Type}", candidate.Name, candidate.DeclaringType?.Name);
            var parameters = ParameterResolver.Resolve(candidate.GetParameters(), root, 
                inputParams.Append(this).ToArray());
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

        private object? ProcessMethodReturn(object? ret, object?[] inputParams, IDisposable scope)
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
            TryRunSingleMethod(Array.Empty<object>(), decl.Target, decl.Method, out var result);
            return result;
        }
    }
}