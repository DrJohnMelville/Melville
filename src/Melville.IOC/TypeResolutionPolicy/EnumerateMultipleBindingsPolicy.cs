using System;
using System.Collections.Generic;
using Melville.IOC.BindingRequests;
using Melville.IOC.IocContainers;
using Melville.IOC.IocContainers.ActivationStrategies;

namespace Melville.IOC.TypeResolutionPolicy
{
    public class EnumerateMultipleBindingsPolicy: ITypeResolutionPolicy
    {
        private readonly ITypeResolutionPolicy innerPolicy;

        public EnumerateMultipleBindingsPolicy(ITypeResolutionPolicy innerPolicy)
        {
            this.innerPolicy = innerPolicy;
        }

        public IActivationStrategy? ApplyResolutionPolicy(IBindingRequest request)
        {
            if (!IsClosedEnumerable(request.DesiredType)) return null;
            return new MultipleValueActivator(innerPolicy, request.DesiredType.GetGenericArguments()[0]);
        }

        private bool IsClosedEnumerable(Type requestTargetType) =>
            requestTargetType.IsConstructedGenericType &&
            IsListType(requestTargetType.GetGenericTypeDefinition());

        private static bool IsListType(Type typeToCheck)
        {
            return typeToCheck == typeof(IEnumerable<>) ||
                   typeToCheck == typeof(ICollection<>) ||
                   typeToCheck == typeof(IList<>);
        }
    }
}