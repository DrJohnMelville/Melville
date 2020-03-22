using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using Castle.DynamicProxy;
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
    public class TypeInterceptorStrategy : ForwardingActivationStrategy, IInterceptorTypeDeclaration
    {
        private readonly List<Type> interceptorTypes = new List<Type>();

        public TypeInterceptorStrategy(IActivationStrategy activationStrategy) : 
            base(activationStrategy) {}

        public override object? Create(IBindingRequest bindingRequest)
        {
            var item = base.Create(bindingRequest) ?? throw new IOException("Cannot wrap null");
            var interceptors = bindingRequest.IocService.Get(interceptorTypes
                    .Select(i => bindingRequest.CreateSubRequest(i)).ToList())
                .OfType<IInterceptor>()
                .ToArray();
            return ProxyGeneratorSource.CreateInterceptor(bindingRequest.IocService, bindingRequest.DesiredType,
                item, interceptors);
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