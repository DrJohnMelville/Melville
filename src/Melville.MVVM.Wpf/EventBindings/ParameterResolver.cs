using  System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Reflection;
using System.Windows;
using Melville.MVVM.Functional;
using Melville.MVVM.Wpf.DiParameterSources;
using Serilog;

namespace Melville.MVVM.Wpf.EventBindings
{
  public static class ParameterResolver
  {
    public static IList<object>? Resolve(ParameterInfo[] parameters, DependencyObject sender, 
      object?[] inputParam)
    {
      var ret = new List<object>();
      var scope = DiIntegration.SearchForContainer(sender).CreateScope();
      foreach (var parameter in parameters)
      {
        if (!ActualOrDefaultParamValue(parameter, sender, inputParam, scope, out var actualVal)) return null;
        ret.Add(actualVal);        
      }

      return ret;
    }

    public static bool ActualOrDefaultParamValue(ParameterInfo parameter, DependencyObject sender, object?[] inputParam,
      IDIIntegration scope, [NotNullWhen(true)] out object? result)
    {
      result = parameter switch
      {
        var p when HasFromServiceAttribute(p) => scope.Get(p),
        var p when ResolveParameter(p.ParameterType, sender, inputParam, out var r) => r,
        var p when p.HasDefaultValue => p.DefaultValue,
        _ => parameter
      };
      return result != parameter;
    }

    private static bool HasFromServiceAttribute(ParameterInfo p) => Attribute.IsDefined(p, typeof(FromServicesAttribute));

    public static bool ResolveParameter(Type parameterType, DependencyObject sender, object?[] inputParam,
      [NotNullWhen(true)]out object? result)
      {
      foreach (var candidate in AllParameterPossibilities(sender, inputParam))
      {
        if (candidate is IFactory fact && parameterType.IsAssignableFrom(fact.TargetType))
        {
          result = fact.Create(sender, inputParam);
          return true;
        }

        if (parameterType.IsInstanceOfType(candidate))
        {
          result = candidate;
          return true;
        }
      }
      result = null;
      return false;
    }

    private static IEnumerable<object> AllParameterPossibilities(DependencyObject sender, object?[] inputParam) =>
      inputParam.Concat(sender.AllSources()).OfType<object>().Distinct();


  }
}