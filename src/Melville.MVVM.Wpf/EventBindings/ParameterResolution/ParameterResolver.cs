using System;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using Melville.MVVM.Wpf.DiParameterSources;
using Melville.MVVM.Wpf.EventBindings.SearchTree;

namespace Melville.MVVM.Wpf.EventBindings.ParameterResolution;

public static class ParameterResolver
{
  public static ResolvedParameters? Resolve(ParameterInfo[] parameters, ref VisualTreeRunContext context)
  {
    var ret = new ResolvedParameters(parameters.Length);
    for (int i = 0; i < parameters.Length; i++)
    {
      if (!ActualOrDefaultParamValue(parameters[i], ref context, out var actualVal))
        return null;
      ret.Put(i, actualVal);        
        
    }
    return ret;
  }

  private static bool ActualOrDefaultParamValue(
    ParameterInfo parameter, ref VisualTreeRunContext context, [NotNullWhen(true)] out IFactory? result)
  {
    result = parameter switch
    {
      var p when HasFromServiceAttribute(p) => new DiFactory(parameter),
      var p when ResolveParameter(p.ParameterType, ref context, out var r) => r,
      var p when p.HasDefaultValue => new ConstantFactory(p.DefaultValue),
      _ => null      
    };
    return result != null;
  }

  private static bool HasFromServiceAttribute(ParameterInfo p) => 
    Attribute.IsDefined(p, typeof(FromServicesAttribute));

  public static bool ResolveParameter(
    Type parameterType, ref VisualTreeRunContext context,
    [NotNullWhen(true)]out IFactory? result)
  {
    foreach (var candidate in context.AllParameterPossibilities())
    {
      if (candidate is IFactory fact && parameterType.IsAssignableFrom(fact.TargetType))
      {
        result = fact;
        return true;
      }

      if (parameterType.IsInstanceOfType(candidate))
      {
        result = new ConstantFactory(candidate);
        return true;
      }
    }
    result = null;
    return false;
  }
}