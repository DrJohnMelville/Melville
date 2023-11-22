using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Melville.Hacks;
using Melville.IOC.Activation;
using Melville.IOC.BindingRequests;
using Melville.IOC.IocContainers;
using Melville.IOC.IocContainers.ActivationStrategies;


namespace Melville.IOC.TypeResolutionPolicy;

public sealed class TuplesToScopeResolutionPolicy: ITypeResolutionPolicy
{
    public IActivationStrategy? ApplyResolutionPolicy(IBindingRequest request) => 
        IsTupleScopePattern(request.DesiredType) ?
            new ScopedTupleActivationStrategy(request.DesiredType) : 
            null;

    private bool IsTupleScopePattern(Type type)
    {
        return type.IsConstructedGenericType &&
               ValueTupleTypes.Contains(type.GetGenericTypeDefinition()) &&
               IsDisposableType(type.GetGenericArguments()[0]);
    }

    private static bool IsDisposableType(Type type)
    {
        return type == typeof(IDisposable) || type == typeof(IAsyncDisposable);
    }

    private static readonly Type[] ValueTupleTypes =
    {
        typeof(ValueTuple<,>),
        typeof(ValueTuple<,,>),
        typeof(ValueTuple<,,,>),
        typeof(ValueTuple<,,,,>),
        typeof(ValueTuple<,,,,,>),
        typeof(ValueTuple<,,,,,,>),
        typeof(ValueTuple<,,,,,,,>),
        typeof(Tuple<,>),
        typeof(Tuple<,,>),
        typeof(Tuple<,,,>),
        typeof(Tuple<,,,,>),
        typeof(Tuple<,,,,,>),
        typeof(Tuple<,,,,,,>),
        typeof(Tuple<,,,,,,,>),
    };
}
    
public sealed class ScopedTupleActivationStrategy : IActivationStrategy  
{
    private readonly Type desiredType;
    private readonly ConstructorInvoker funcCreator;

    public ScopedTupleActivationStrategy(Type desiredType)
    {
        this.desiredType = desiredType;
        funcCreator = ActivationCompiler.Compile(desiredType, desiredType.GetGenericArguments().ToArray());
    }
    
    public bool CanCreate(IBindingRequest bindingRequest) =>
        bindingRequest.IocService.CanGet(RequestsForInnerItems(bindingRequest));
    
    private IEnumerable<IBindingRequest> RequestsForInnerItems(IBindingRequest bindingRequest) => 
        desiredType.GetGenericArguments().Skip(1).Select(bindingRequest.CreateSubRequest);
    
    public object? Create(IBindingRequest bindingRequest)
    {
        using var buffer = new RentedBuffer<object?>(desiredType.GetGenericArguments().Length);
        var values = buffer.Span;
        var scope = bindingRequest.IocService.CreateScope();
        values[0] = scope;
        scope.Fill(values[1..], RequestsForInnerItems(bindingRequest));
        return funcCreator.Invoke(values);
    }
    
    public SharingScope SharingScope() => IocContainers.SharingScope.Transient;
    public bool ValidForRequest(IBindingRequest request) => true;
}