using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Melville.IOC.InjectionPolicies;
using Melville.IOC.IocContainers.ActivationStrategies;
using Melville.IOC.TypeResolutionPolicy;

namespace Melville.IOC.IocContainers
{
    public interface IIocService
    { 
        bool CanGet(IBindingRequest request);
        object? Get(IBindingRequest request);

        IIocService? ParentScope { get; }
        bool IsGlobalScope => ParentScope == null;

    }

    public static class IocServiceOperations
    {
        public static IIocService GlobalScope(this IIocService scope) => scope.ScopeList().Last();

        public static IEnumerable<IIocService> ScopeList(this IIocService? service)
        {
            while (service != null)
            {
                yield return service;
                service = service.ParentScope;
            }
        }
        // make it so we car create scopes on IocContainer without casting
        public static IDisposableIocService CreateScope(this IIocService service) =>
            service.CreateSharingScope().CreateLifetimeScope();
        public static IIocService CreateSharingScope(this IIocService service) => new SharingScopeContainer(service);
        public static IDisposableIocService CreateLifetimeScope(this IIocService service) => 
            new DisposableIocService(service);
        public static bool CanGet<T>(this IIocService ioc) => ioc.CanGet(typeof(T));
        public static bool CanGet(this IIocService ioc, Type type) => ioc.CanGet(new RootBindingRequest(type, ioc));
        public static bool CanGet(this IIocService ioc, IEnumerable<IBindingRequest> requests) => requests.All(ioc.CanGet);
        
        public static T Get<T>(this IIocService ioc) => (T) ioc.Get(typeof(T));
        public static object Get(this IIocService ioc, Type serviceTppe)
        {
            return RecursiveExceptionTracker.BasisCall(ioc.Get, new RootBindingRequest(serviceTppe, ioc)) 
                   ?? throw new IocException("Type resolved to null");
        }

        private static bool IsDisposable([NotNullWhen(true)]object? item) => item is IDisposable || item is IAsyncDisposable;


        public static object?[] Get(this IIocService service, IList<IBindingRequest> requiredParameters)
        {
            object?[] argumentArray = new object[requiredParameters.Count()];
            service.Fill(argumentArray.AsSpan(), requiredParameters);
            return argumentArray;
        }

        public static void Fill(this IIocService service, Span<object?> destination, IEnumerable<IBindingRequest> requests)
        {
            int pos = 0;
            foreach (var request in requests)
            {
                if (pos >= destination.Length) return;
                destination[pos] = service.Get(request);
                pos++;
            }
        }

    }

    public class IocContainer: IBindableIocService, IIocService
    {
        public ITypeResolutionPolicyList TypeResolver { get; }

        public IocContainer(ITypeResolutionPolicyList? typeResolver = null)
        {
            this.TypeResolver = typeResolver ?? new StandardTypeResolutionPolicy();
        }
        
        public IIocService? ParentScope => null;

        public T ConfigurePolicy<T>() => TypeResolver.GetInstantiationPolicy<T>();
        public IInterceptionPolicy InterceptionPolicy => TypeResolver.InterceptionPolicy;
        public void AddTypeResolutionPolicyBefore<T>(ITypeResolutionPolicy policy) => 
            TypeResolver.AddResolutionPolicyBefore<T>(policy);
        public void AddTypeResolutionPolicyAfter<T>(ITypeResolutionPolicy policy) => 
            TypeResolver.AddResolutionPolicyAfter<T>(policy);
        public void RemoveTypeResolutionPolicy<T>() => TypeResolver.RemoveTypeResolutionPolicy<T>();

        #region Get
        
        private object? GetImplementation(IBindingRequest bindingRequest)
        {
            var activator = FindActivationStrategy(bindingRequest);
            return activator.Create(bindingRequest);
        }

        private IActivationStrategy FindActivationStrategy(IBindingRequest bindingRequest) =>
            TypeResolver.ApplyResolutionPolicy(bindingRequest)??
            throw new IocException("Cannot bind type: " + bindingRequest.DesiredType.Name);

        object? IIocService.Get(IBindingRequest requestedType) => 
            RecursiveExceptionTracker.RecursiveCall(GetImplementation, requestedType);

        #endregion

        #region CanGet

        public bool CanGet(IBindingRequest request)
        {
            try
            {
                var activator = TypeResolver.ApplyResolutionPolicy(request);
                if (activator == null) return false;
                return activator.CanCreate(request);
            }
            catch (Exception )
            {
                // if we run into a constructor we cannot automatically resolve, then we cannot get the type.
                return false;
            }
        }
        #endregion

    }
}