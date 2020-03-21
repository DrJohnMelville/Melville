using System;

namespace Melville.IOC.IocContainers
{
    public interface IActivationOptions
    {
        IActivationOptions AsSingleton();
        IActivationOptions AsScoped();
        IActivationOptions InNamedParamemter(string name);
        IActivationOptions WhenConstructingType(Type? type);
        IActivationOptions WhenConstructingType<T>() => WhenConstructingType(typeof(T));
        IActivationOptions WithParameters(params object[] parameters);
        IActivationOptions DoNotDispose();
        IActivationOptions DisposeIfInsideScope();
    }
}