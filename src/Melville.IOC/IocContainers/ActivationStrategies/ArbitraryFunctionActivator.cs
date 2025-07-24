using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Melville.Hacks;
using Melville.IOC.BindingRequests;

namespace Melville.IOC.IocContainers.ActivationStrategies;

public class ArbitraryFunctionActivator(
    Delegate targetMethod): IActivationStrategy
{
    public bool CanCreate(IBindingRequest bindingRequest) =>
        targetMethod.Method.GetParameters().All(i =>
            bindingRequest.IocService.CanGet(i.ParameterType));

    public object? Create(IBindingRequest bindingRequest) => 
        targetMethod.Method.Invoke(targetMethod.Target, ComputeArguments(bindingRequest))
            .TryRegisterDisposeAndReturn(bindingRequest);

    private object?[] ComputeArguments(IBindingRequest bindingRequest)
    {
        var parameters = targetMethod.Method.GetParameters();
        if (parameters.Length == 0) return [];
        
        var arguments = new object?[parameters.Length];
        bindingRequest.IocService.Fill(arguments, ArgumentRequests(bindingRequest, parameters));
        return arguments;
    }

    private static IEnumerable<IBindingRequest> ArgumentRequests(
        IBindingRequest bindingRequest, ParameterInfo[] parameters) =>
        parameters.Select(i=>bindingRequest.CreateSubRequest(i.ParameterType));

    public SharingScope SharingScope() => IocContainers.SharingScope.Transient;

    public bool ValidForRequest(IBindingRequest request) => true;
}