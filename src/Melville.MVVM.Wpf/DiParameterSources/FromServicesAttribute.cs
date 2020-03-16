using System;
using System.Reflection;

namespace Melville.MVVM.Wpf.DiParameterSources
{
    /// <summary>
    /// Indicates that a parameter should be sought from the DI system rather than the tree.
    /// </summary>
    [AttributeUsage(AttributeTargets.Parameter)]
    public class FromServicesAttribute:Attribute
    {
    }

    public interface IDIIntegration: IDisposable
    {
        public IDIIntegration CreateScope();
        public object? Get(ParameterInfo info);
        public object? Get(Type type);
    }
    
    public class EmptyScopeFactory: IDIIntegration
    {
        public void Dispose() {}
        public object? Get(ParameterInfo info) => throw new NotSupportedException("Null DI container cannot create objects.");
        public object? Get(Type type) => throw new NotSupportedException("Null DI container cannot create objects.");
        public IDIIntegration CreateScope() => this;
    }
}