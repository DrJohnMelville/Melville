using System;

namespace Melville.IOC.IocContainers
{
    public interface IActivationOptions
    {
        // sharing scopes
        IActivationOptions AsSingleton();
        IActivationOptions AsScoped();

        // Disposal scopes
        IActivationOptions DoNotDispose();
        IActivationOptions DisposeIfInsideScope();

        // Injection restrictions
        IActivationOptions When(Func<IBindingRequest, bool> predicate);
        IActivationOptions InNamedParameter(string name) =>
            When(p => name.Equals(p.TargetParameterName, StringComparison.Ordinal));
        IActivationOptions WhenConstructingType(Type type) =>
            When(p => type.IsAssignableFrom(p.TypeBeingConstructed));
        IActivationOptions WhenConstructingType<T>() => WhenConstructingType(typeof(T));
        IActivationOptions WhenNotConstructingType(Type type) =>
            When(p => p.TypeBeingConstructed == null || !type.IsAssignableFrom(p.TypeBeingConstructed));
        IActivationOptions WhenNotConstructingType<T>() => WhenNotConstructingType(typeof(T));
        
        // Additional construction info.
        IActivationOptions WithParameters(params object[] parameters);
    }
}