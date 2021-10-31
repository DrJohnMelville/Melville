﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Melville.IOC.BindingRequests;

namespace Melville.IOC.IocContainers.ActivationStrategies;

public class TypeActivationStrategy: IActivationStrategy
{
    private readonly ParameterInfo[] paramTypes;
    private readonly Func<object?[], object> activator;

    public SharingScope SharingScope() => IocContainers.SharingScope.Transient;
    public bool ValidForRequest(IBindingRequest request) => true;
    public bool CanCreate(IBindingRequest bindingRequest) => 
        bindingRequest.IocService.CanGet(ComputeDependencies(bindingRequest));

    public object? Create(IBindingRequest bindingRequest) => 
        activator(bindingRequest.IocService.Get(ComputeDependencies(bindingRequest)));

    private List<IBindingRequest> ComputeDependencies(IBindingRequest bindingRequest) =>
        paramTypes
            .Select(bindingRequest.CreateSubRequest)
            .ToList();

    public TypeActivationStrategy(Func<object?
        [], object> activator, ParameterInfo[] paramTypes)
    {
        this.activator = activator;
        this.paramTypes = paramTypes;
    }
}