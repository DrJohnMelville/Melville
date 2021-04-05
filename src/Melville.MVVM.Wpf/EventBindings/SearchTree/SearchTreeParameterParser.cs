using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using Melville.Linq;
using Melville.MVVM.CSharpHacks;

namespace Melville.MVVM.Wpf.EventBindings.SearchTree
{
  public static class SearchTreeParameterParser
  {
    public static object?[] EvalParameters(string paramString, DependencyObject target, object? arg)
    {
      return EnumerableExtensions.Concat(NamedParameters(paramString, target, arg), arg)
        .OfType<object>()
        .ToArray();
    }

    private static IEnumerable<object?> NamedParameters(string paramString, DependencyObject target, object? arg)
    {
      if (String.IsNullOrWhiteSpace(paramString)) return new object[0];
      return paramString.Split(new[] {','}, StringSplitOptions.RemoveEmptyEntries)
        .Select(i => EvalParameter(i, target, arg));
    }

    private static object? EvalParameter(string paramString, DependencyObject target, object? arg)
    {
      var path = ReflectionHelper.SplitPathString(paramString);
      object root = paramString;
      switch (path[0])
      {
        case "$arg":
          root = arg ?? new object();
          break;
        case "$this":
          root = target;
          break;
      }

      return path.Length > 1 ? root.FollowPathComponents(path.Skip(1), true) : root;
    }
  }
}