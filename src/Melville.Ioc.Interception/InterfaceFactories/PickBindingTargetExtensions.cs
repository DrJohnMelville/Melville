using System;
using Melville.IOC.BindingRequests;
using Melville.IOC.IocContainers;
using Melville.IOC.IocContainers.ActivationStrategies;
using Melville.IOC.IocContainers.BindingSources;

namespace Melville.Ioc.Interception.InterfaceFactories
{
    public static class PickBindingTargetExtensions
    {
        public static IActivationOptions<T> ToFactory<T>(this IPickBindingTarget<T> target) => 
            target.DoBinding(new GenerateFactoryStrategy(typeof(T)));
    }

    public class GenerateFactoryStrategy : IActivationStrategy
    {
        private readonly Type targetType;
        public GenerateFactoryStrategy(Type targetType)
        {
            this.targetType = targetType;
            VerifyTypeIsValidFactoryTemplate();
        }

        private void VerifyTypeIsValidFactoryTemplate()
        {
            if (!targetType.IsInterface)
            {
                throw new IocException("Only interfaces can become dynamic factories.");
            }
        }

        public object? Create(IBindingRequest bindingRequest)
        {
            var implementation = new FactoryInterceptor(bindingRequest);
            return ProxyGeneratorSource.CreateSyntheticObject(bindingRequest.IocService, targetType, implementation);
        }

        public bool CanCreate(IBindingRequest bindingRequest) => true;
        public SharingScope SharingScope() => IOC.IocContainers.SharingScope.Transient;
        public bool ValidForRequest(IBindingRequest request) => true;
    }
}