﻿using System;
using System.Collections.Generic;
using System.Linq;
using Melville.IOC.Activation;
using Melville.IOC.BindingRequests;
using Melville.IOC.IocContainers;
using Melville.IOC.IocContainers.ActivationStrategies;


namespace Melville.IOC.TypeResolutionPolicy;

public class FunctionActivationStrategy : IActivationStrategy
{
    private readonly Type functionDelegateType;
    private readonly Type resultType;
    private readonly ForwardFuncToMethodCall forwardFuncToMethodCall;
    public FunctionActivationStrategy(Type functionDelegateType)
    {
        resultType = functionDelegateType.GetGenericArguments().Last();
        this.functionDelegateType = functionDelegateType;
        forwardFuncToMethodCall = new ForwardFuncToMethodCall(functionDelegateType, 
            nameof(FunctionFactoryImplementation.CreateTargetObject),
            typeof(FunctionFactoryImplementation));
    }

    public bool CanCreate(IBindingRequest bindingRequest) => 
        bindingRequest.IocService.CanGet(new IBindingRequest[]{InnerRequestForCanCreate(bindingRequest)});

    private IBindingRequest InnerRequestForCanCreate(IBindingRequest bindingRequest)
    {
        var types = functionDelegateType.GetGenericArguments().ToList();
        return bindingRequest.CreateSubRequest(types.Last(),
            CreateFakeObjectsFromFunctionParameters(types).ToArray());
    }

    private static IEnumerable<object> CreateFakeObjectsFromFunctionParameters(List<Type> types) => 
        types.SkipLast(1).Select(i => (object) new ReplaceLiteralArgumentWithNonsenseValue(i));


    public object? Create(IBindingRequest bindingRequest) =>
        forwardFuncToMethodCall.CreateFuncDelegate(new FunctionFactoryImplementation(bindingRequest, resultType));

    public SharingScope SharingScope() => IocContainers.SharingScope.Transient;

    public bool ValidForRequest(IBindingRequest ret) => true;
}

public class FunctionFactoryImplementation
{
    private readonly IBindingRequest bindingRequest;
    private readonly Type desiredType;
    public FunctionFactoryImplementation(IBindingRequest bindingRequest, 
        Type desiredType)
    {
        this.bindingRequest = bindingRequest;
        this.desiredType = desiredType;
    }
        
    public object? CreateTargetObject(object[] parameters) => 
        bindingRequest.IocService.Get(bindingRequest.CreateSubRequest(desiredType, parameters)) 
        ?? throw new IocException("Type resolved to null");
}