using System;
using System.Collections;
using System.Collections.Generic;
using Melville.IOC.Activation;
using Melville.IOC.IocContainers;
using Melville.IOC.IocContainers.ActivationStrategies;


namespace Melville.IOC.TypeResolutionPolicy
{
    public class MultipleValueActivator : IActivationStrategy
    {
        private readonly ITypeResolutionPolicy innerResolver;
        private readonly Type typeToResolve;
        private readonly Func<object[], object> typedListCreator;

        public MultipleValueActivator(ITypeResolutionPolicy innerResolver, Type typeToResolve)
        {
            this.innerResolver = innerResolver;
            this.typeToResolve = typeToResolve;
            typedListCreator = ActivationCompiler.Compile(typeof(List<>).MakeGenericType(typeToResolve),
                Array.Empty<Type>());
        }

        // we can always create a list, even if it is an empty list.
        public bool CanCreate(IBindingRequest bindingRequest) => true;

        public object? Create(IBindingRequest bindingRequest)
        {
            var ret = CreateResultList();
            ResolveInnerType(bindingRequest)?.CreateMany(bindingRequest, ret.Add);
            return ret;
        }

        public bool ValidForRequest(IBindingRequest request) => true;

        private IList CreateResultList() => (IList)typedListCreator(Array.Empty<object>());
        private IActivationStrategy? ResolveInnerType(IBindingRequest bindingRequest) => 
            innerResolver.ApplyResolutionPolicy(bindingRequest.CreateSubRequest(typeToResolve));

        public SharingScope SharingScope() =>  IocContainers.SharingScope.Transient;
    }
}