using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using Melville.IOC.Activation;
using Melville.IOC.BindingRequests;
using Melville.IOC.IocContainers;
using Melville.IOC.IocContainers.ActivationStrategies;


namespace Melville.IOC.TypeResolutionPolicy;

public class MultipleValueActivator : IActivationStrategy
{
    private readonly ITypeResolutionPolicy innerResolver;
    private readonly Type typeToResolve;
    private readonly ConstructorInvoker typedListCreator;
    private IList? emptyResult;

    public MultipleValueActivator(ITypeResolutionPolicy innerResolver, Type typeToResolve)
    {
        this.innerResolver = innerResolver;
        this.typeToResolve = typeToResolve;
        typedListCreator = 
            ActivationCompiler.Compile(typeof(List<>)
                .MakeGenericType(typeToResolve));
    }

    // we can always create a list, even if it is an empty list.
    public bool CanCreate(IBindingRequest bindingRequest) => true;

    public object? Create(IBindingRequest bindingRequest)
    {
        if (bindingRequest.IsCancelled) return null;

        var subRequest = bindingRequest.CreateSubRequest(typeToResolve);
        var resolver = innerResolver.ApplyResolutionPolicy(subRequest);
        if (resolver == null || !resolver.CanCreate(subRequest))
        {
            bindingRequest.IsCancelled = false; // failure to bind an item successfully returns an empty list.
            return GetEmptyList();
        }
        var ret = CreateResultList();
        resolver?.CreateMany(subRequest, ret);
        return ret;
    }

    public bool ValidForRequest(IBindingRequest request) => true;

    private IList CreateResultList() => (IList)typedListCreator.Invoke();

    public SharingScope SharingScope() =>  IocContainers.SharingScope.Transient;

    private IList GetEmptyList() => emptyResult ??= CreateEmptyList();
    private IList CreateEmptyList()
    {
        var type = typeof(Array);
        var meth = type.GetMethod("Empty", BindingFlags.Static | BindingFlags.Public);
        return (IList) meth!.MakeGenericMethod(typeToResolve).Invoke(null,[])!;
    }
}