using System;
using System.Collections.Generic;
using System.Linq;
using Melville.IOC.TypeResolutionPolicy;

namespace Melville.IOC.IocContainers
{
    public interface IIocService
    { 
        bool CanGet(IBindingRequest request);
        object? Get(IBindingRequest request);

        IIocService GlobalScope { get; }
        bool IsGlobalScope => GlobalScope == this;

        IDisposableIocService CreateScope() => this.Get<Scope>();
    }

    public static class IocServiceOperations
    {
        // make it so we car create scopes on IocContainer without casting
        public static IDisposableIocService CreateScope(IIocService service) => service.CreateScope();
        public static bool CanGet<T>(this IIocService ioc) => ioc.CanGet(typeof(T));
        public static bool CanGet(this IIocService ioc, Type type) => ioc.CanGet(new RootBindingRequest(type, ioc));
        public static bool CanGet(this IIocService ioc, IEnumerable<IBindingRequest> requests) => requests.All(ioc.CanGet);
        
        public static T Get<T>(this IIocService ioc) => (T) ioc.Get(typeof(T));
        public static object Get(this IIocService ioc, Type serviceTppe) =>
            RecursiveExceptionTracker.BasisCall(ioc.Get, new RootBindingRequest(serviceTppe, ioc))
            ?? throw new InvalidOperationException("Type resolved to null");
        
        public static object?[] Get(this IIocService service, IList<IBindingRequest> requiredParameters)
        {
            object?[] argumentArray = new object[requiredParameters.Count()];
            for (int i = 0; i < argumentArray.Length && i <requiredParameters.Count() ; i++)
            {
                argumentArray[i] = service.Get(requiredParameters[i]);
            }
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

        public IIocService GlobalScope => this;

        public T ConfigurePolicy<T>() => TypeResolver.GetPolicy<T>();


        #region Get

        public IDisposableIocService CreateScope() => new Scope(this);
        
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
            var activator = TypeResolver.ApplyResolutionPolicy(request);
            if (activator == null) return false;
            return activator.CanCreate(request);
        }
        #endregion

    }
}