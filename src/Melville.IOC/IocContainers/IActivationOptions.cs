using System;

namespace Melville.IOC.IocContainers
{
    public interface IActivationOptions<T>
    {
        // sharing scopes
        IActivationOptions<T> AsSingleton();
        IActivationOptions<T> AsScoped();

        // Disposal scopes
        IActivationOptions<T> DoNotDispose();
        IActivationOptions<T> DisposeIfInsideScope();

        // Injection restrictions
        IActivationOptions<T> When(Func<IBindingRequest, bool> predicate);
        IActivationOptions<T> InNamedParameter(string name) =>
            When(p => name.Equals(p.TargetParameterName, StringComparison.Ordinal));
        IActivationOptions<T> WhenConstructingType(Type type) =>
            When(p => type.IsAssignableFrom(p.TypeBeingConstructed));
        IActivationOptions<T> WhenConstructingType<TTarget>() => WhenConstructingType(typeof(TTarget));
        IActivationOptions<T> WhenNotConstructingType(Type type) =>
            When(p => p.TypeBeingConstructed == null || !type.IsAssignableFrom(p.TypeBeingConstructed));
        IActivationOptions<T> WhenNotConstructingType<TTarget>() => WhenNotConstructingType(typeof(TTarget));
        IActivationOptions<T> BlockSelfInjection() => WhenNotConstructingType(typeof(T));

        
        // Additional construction info.
        IActivationOptions<T> WithParameters(params object[] parameters);
    }
}