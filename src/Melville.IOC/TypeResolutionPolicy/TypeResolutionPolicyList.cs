using System;
using System.Collections.Generic;
using System.Linq;
using Melville.IOC.InjectionPolicies;
using Melville.IOC.IocContainers;
using Melville.IOC.IocContainers.ActivationStrategies;

namespace Melville.IOC.TypeResolutionPolicy
{
    public interface ITypeResolutionPolicy
    {
        IActivationStrategy? ApplyResolutionPolicy(IBindingRequest request);
    }

    public interface ITypeResolutionPolicyList:ITypeResolutionPolicy
    {
        T GetInstantiationPolicy<T>();
        IInterceptionPolicy InterceptionPolicy { get; }
    }
    public class TypeResolutionPolicyList: ITypeResolutionPolicyList
    {
        public List<ITypeResolutionPolicy> Policies { get; } = new List<ITypeResolutionPolicy>();
        public IInterceptionPolicy InterceptionPolicy { get; } = new DefaultInterceptionPolicy();
        public IActivationStrategy? ApplyResolutionPolicy(IBindingRequest request) =>
            Policies
                .Select(i => i.ApplyResolutionPolicy(request))
                .FirstOrDefault(i => i != null && i.ValidForRequest(request));


        public T GetInstantiationPolicy<T>() => Policies
                                       .Select(i=>i is MemorizeResult mr? mr.InnerPolicy:i)
                                       .OfType<T>()
                                       .FirstOrDefault()??
                                   throw new InvalidOperationException("No policy object of type: " + typeof(T).Name);
    }
}