using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Reflection;
using System.Windows;
using Melville.MVVM.Wpf.DiParameterSources;

namespace Melville.MVVM.Wpf.EventBindings.ParameterResolution
{
  public sealed class ResolvedParameters
  {
    private readonly IDIIntegration di;
    private readonly DependencyObject sender;
    private readonly object?[] inputParam;
    private readonly IFactory[] factories;

    public ResolvedParameters(in int numberOfParameters, IDIIntegration dIContainer, DependencyObject sender, 
      object?[] inputParam)
    {
      this.di = dIContainer;
      this.sender = sender;
      this.inputParam = inputParam;
      factories = new IFactory[numberOfParameters];
    }

    public void Put(int position, IFactory parameter)
    {
      factories[position] = parameter;
    }
    
    public IDisposable GetValues(out object?[] values)
    {
      var scope = di.CreateScope();
      values = new object?[factories.Length];
      for (int i = 0; i < factories.Length; i++)
      {
        values[i] = factories[i].Create(scope, sender, inputParam);
      }
      return scope;
    }
  }
  public static class ParameterResolver
  {
    public static ResolvedParameters? Resolve(ParameterInfo[] parameters, DependencyObject sender, 
      object?[] inputParam)
    {
      var dIContainer = DiIntegration.SearchForContainer(sender);
      var ret = new ResolvedParameters(parameters.Length, dIContainer, sender, inputParam);
      for (int i = 0; i < parameters.Length; i++)
      {
        if (!ActualOrDefaultParamValue(parameters[i], sender, inputParam, out var actualVal))
          return null;
        ret.Put(i, actualVal);        
        
      }
      return ret;
    }

    private static bool ActualOrDefaultParamValue(
      ParameterInfo parameter, DependencyObject sender, IEnumerable<object?> inputParam, [NotNullWhen(true)] out IFactory? result)
    {
      result = parameter switch
      {
        var p when HasFromServiceAttribute(p) => new DiFactory(parameter),
        var p when ResolveParameter(p.ParameterType, sender, inputParam, out var r) => r,
        var p when p.HasDefaultValue => new ConstantFactory(p.DefaultValue),
        _ => null      
      };
      return result != null;
    }

    private static bool HasFromServiceAttribute(ParameterInfo p) => Attribute.IsDefined(p, typeof(FromServicesAttribute));

    public static bool ResolveParameter(
      Type parameterType, DependencyObject sender, IEnumerable<object?> inputParam,
      [NotNullWhen(true)]out IFactory? result)
      {
      foreach (var candidate in AllParameterPossibilities(sender, inputParam))
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

    private static IEnumerable<object> AllParameterPossibilities(
      DependencyObject sender, IEnumerable<object?> inputParam) =>
      inputParam.Concat(sender.AllSources()).OfType<object>().Distinct();
  }
}