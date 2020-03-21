using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Melville.IOC.IocContainers.ActivationStrategies;
using Melville.IOC.TypeResolutionPolicy;

namespace Melville.IOC.IocContainers
{
    public interface IIocService
    { 
        bool CanGet(IBindingRequest request);
        (object? Result, DisposalState DisposalState) Get(IBindingRequest request);

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
            return UnwrapCheckNullAndDispose(RecursiveExceptionTracker.BasisCall(ioc.Get, new RootBindingRequest(serviceTppe, ioc)));
        }

        public static object UnwrapCheckNullAndDispose(this (object? Result, DisposalState DisposalState) value)
        {
            return UnwrapAndCheckDispose(value) ?? throw new IocException("Type resolved to null");
        }

        public static object? UnwrapAndCheckDispose(this (object? Result, DisposalState DisposalState) item)
        {
            // if (item.DisposalState == DisposalState.DisposalRequired && IsDisposable(item.Result))
            //     throw new IocException($"{item.Result.GetType()} requires disposal but was created in global context.");
            return item.Result;
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
                destination[pos] = UnwrapAndCheckDispose(service.Get(request));
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

        public T ConfigurePolicy<T>() => TypeResolver.GetPolicy<T>();


        #region Get
        
        private (object? Result, DisposalState DisposalState) GetImplementation(IBindingRequest bindingRequest)
        {
            var activator = FindActivationStrategy(bindingRequest);
            return activator.Create(bindingRequest);
        }

        private IActivationStrategy FindActivationStrategy(IBindingRequest bindingRequest) =>
            TypeResolver.ApplyResolutionPolicy(bindingRequest)??
            throw new IocException("Cannot bind type: " + bindingRequest.DesiredType.Name);

        (object? Result, DisposalState DisposalState) IIocService.Get(IBindingRequest requestedType) => 
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