using  System;
using System.Reflection;
using System.Windows;
using Melville.MVVM.Wpf.DiParameterSources;

namespace Melville.MVVM.Wpf.EventBindings
{
  public interface IFactory
  {
    Type TargetType { get; }
    object? Create(IDIIntegration di,DependencyObject sender, object?[] args);

  }

  public static class Factory
  {
    public static IFactory Unique<T>(Func<DependencyObject, object?[], T> func) where T:notnull => 
      new UniqueFactory<T>(func);
    
    private class UniqueFactory<T> : IFactory where T: notnull
    {
      private Func<DependencyObject, object?[], T> factory;

      public UniqueFactory(Func<DependencyObject, object?[], T> factory)
      {
        this.factory = factory;
      }

      public Type TargetType => typeof(T);

      public object? Create(IDIIntegration di, DependencyObject sender, object?[] args) => factory(sender, args);
    }
  }

  public class DiFactory : IFactory
  {
    private ParameterInfo requestedParameter;

    public DiFactory(ParameterInfo requestedParameter)
    {
      this.requestedParameter = requestedParameter;
    }

    public Type TargetType => requestedParameter.ParameterType;

    public object? Create(IDIIntegration di, DependencyObject sender, object?[] args)
    {
      return di.Get(requestedParameter);
    }
  }
  
  public class ConstantFactory: IFactory
  {
    private object? value;

    public ConstantFactory(object? value)
    {
      this.value = value;
    }

    public Type TargetType => (value?.GetType()) ?? typeof(object);

    public object? Create(IDIIntegration di, DependencyObject sender, object?[] args)
    {
      return value;
    }
  }
}