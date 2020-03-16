using  System;
using System.Windows;

namespace Melville.MVVM.Wpf.EventBindings
{
  public interface IFactory
  {
    Type TargetType { get; }
    object Create(DependencyObject sender, object?[] args);

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

      public object Create(DependencyObject sender, object?[] args) => factory(sender, args);
    }
  }
}