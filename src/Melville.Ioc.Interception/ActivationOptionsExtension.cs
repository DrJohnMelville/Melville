using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Castle.DynamicProxy;
using Melville.IOC.BindingRequests;
using Melville.IOC.IocContainers;
using Melville.IOC.IocContainers.ActivationStrategies;

namespace Melville.Ioc.Interception
{
    public static class ActivationOptionsExtension
    {
        public static IActivationOptions<T> InterceptWith<T>(this IActivationOptions<T> parent,
            params IInterceptor[] interceptors) =>
            parent.AddActivationStrategy(i => new InterceptorStrategy(i, interceptors));

        public static IActivationOptions<T> InterceptWith<T>(this IActivationOptions<T> parent,
            Action<IInterceptorTypeDeclaration> types) =>
            parent.AddActivationStrategy(i =>
            {
                var ret = new TypeInterceptorStrategy(i);
                types(ret);
                return ret;
            });
    }

    public interface IInterceptorTypeDeclaration
    {
        IInterceptorTypeDeclaration Add(Type type);
        IInterceptorTypeDeclaration Add<T>() where T:IInterceptor => Add(typeof(T));
    }

    public static class TypedInterceptorImplementation
    {
        public static object InterceptFromTypes(IBindingRequest bindingRequest, object item, 
            IEnumerable<Type> interceptorTypes)
        {
            var interceptors = bindingRequest.IocService.Get(interceptorTypes
                    .Select(bindingRequest.CreateSubRequest).ToList())
                .OfType<IInterceptor>()
                .ToArray();
            return ProxyGeneratorSource.CreateInterceptor(bindingRequest.IocService, bindingRequest.DesiredType,
                item, interceptors);
        }
    }
    public class TypeInterceptorStrategy : ForwardingActivationStrategy, IInterceptorTypeDeclaration
    {
        private readonly List<Type> interceptorTypes = new List<Type>();

        public TypeInterceptorStrategy(IActivationStrategy activationStrategy) : 
            base(activationStrategy) {}

        public override object? Create(IBindingRequest bindingRequest)
        {
            var item = base.Create(bindingRequest) ?? throw new IOException("Cannot wrap null");
            return TypedInterceptorImplementation.InterceptFromTypes(bindingRequest, item, interceptorTypes);
        }

        private bool IsInterceptor(Type i)
        {
            return typeof(IInterceptor).IsAssignableFrom(i);
        }

        public IInterceptorTypeDeclaration Add(Type type)
        {
            if (IsInterceptor(type))
            {
                interceptorTypes.Add(type);
            };
            return this;
        }
    }


    public class InterceptorStrategy : ForwardingActivationStrategy
    {
        private readonly IInterceptor[] interceptors;
        public InterceptorStrategy(IActivationStrategy activationStrategy, IInterceptor[] interceptors) : 
            base(activationStrategy) =>
            this.interceptors = interceptors;

        public override object? Create(IBindingRequest bindingRequest)
        {
            var item = base.Create(bindingRequest) ?? throw new IOException("Cannot wrap null");
            return ProxyGeneratorSource.CreateInterceptor(bindingRequest.IocService, bindingRequest.DesiredType,
                item, interceptors);
        }
    }
}