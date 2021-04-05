using System;
using System.Collections.Generic;
using Castle.DynamicProxy;
using Melville.IOC.BindingRequests;
using Melville.IOC.InjectionPolicies;
using Melville.IOC.IocContainers;

namespace Melville.Ioc.Interception
{
    public static class BindableIocServiceExtensions
    {
        public static void Intercept<T>(this IBindableIocService service,
            params IInterceptor[] interceptors) =>
            service.Intercept(new SingletonInterceptorRule<T,T>(interceptors));
        public static void Intercept<T>(this IBindableIocService service,
            Action<IInterceptorTypeDeclaration> types) =>
            service.Intercept(new TypedInterceptionRule<T,T>(types));
        public static void Intercept<TSource,TDest>(this IBindableIocService service,
            params IInterceptor[] interceptors) where TSource:TDest =>
            service.Intercept(new SingletonInterceptorRule<TSource,TDest>(interceptors));
        public static void Intercept<TSource,TDest>(this IBindableIocService service,
            Action<IInterceptorTypeDeclaration> types) where TSource:TDest =>
            service.Intercept(new TypedInterceptionRule<TSource,TDest>(types));
    }

    public class TypedInterceptionRule<TSource,TDest> : InterceptorRuleBase<TSource,TDest>, IInterceptorTypeDeclaration
        where TSource:TDest
    {
        private List<Type> interceptorTypes = new List<Type>();

        public TypedInterceptionRule(Action<IInterceptorTypeDeclaration> types)
        {
            types(this);
        }

        protected override TDest DoInterception(IBindingRequest request, TSource source)
        {
            return (TDest)TypedInterceptorImplementation.InterceptFromTypes(request, source!,
                interceptorTypes);
        }

        public IInterceptorTypeDeclaration Add(Type type)
        {
            if (!typeof(IInterceptor).IsAssignableFrom(type))
            {
                throw new InvalidOperationException("Attempt to inject a not IInterceptor by type");
            }
            interceptorTypes.Add(type);
            return this;
        }
    }

    public class SingletonInterceptorRule<TSource, TDest> : InterceptorRuleBase<TSource,TDest> where TSource:TDest
    {
        private readonly IInterceptor[] interceptors;

        public SingletonInterceptorRule(IInterceptor[] interceptors) => this.interceptors = interceptors;

        protected override TDest DoInterception(IBindingRequest request, TSource source) => 
            (TDest)ProxyGeneratorSource.CreateInterceptor(request.IocService, typeof(TDest), source!, interceptors);
    }
}