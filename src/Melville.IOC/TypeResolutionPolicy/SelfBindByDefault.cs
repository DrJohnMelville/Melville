using System;
using System.Collections.Generic;
using Melville.IOC.BindingRequests;
using Melville.IOC.IocContainers.ActivationStrategies;
using Melville.IOC.IocContainers.ActivationStrategies.TypeActivation;

namespace Melville.IOC.TypeResolutionPolicy
{
    public class SelfBindByDefault : ITypeResolutionPolicy
    {
        public List<Type> ForbiddenTypes { get; } = new List<Type>()
        {
            typeof(bool),
            typeof(string),
            typeof(char),
            typeof(int),
            typeof(uint),
            typeof(ulong),
            typeof(long),
            typeof(float),
            typeof(double),
            typeof(DateTime),
            typeof(DateTimeOffset),
        };
        public IActivationStrategy? ApplyResolutionPolicy(IBindingRequest request) => 
            IsCreatable(request.DesiredType) ? 
                TypeActivatorFactory.CreateTypeActivator(request.DesiredType, 
                    ConstructorSelectors.MaximumArgumentCount) : null;

        private bool IsCreatable(Type type) => (type.IsClass || type.IsValueType) 
           && type.GetConstructors().Length > 0 && ! ForbiddenTypes.Contains(type);
    }
}