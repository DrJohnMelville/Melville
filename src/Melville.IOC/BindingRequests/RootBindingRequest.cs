using System;
using System.Collections.Generic;
using Melville.IOC.IocContainers;
using Melville.IOC.IocContainers.ActivationStrategies;

namespace Melville.IOC.BindingRequests;

public class RootBindingRequest : IBindingRequest
{
    public RootBindingRequest(Type targetType, IIocService iocService, params IEnumerable<object> arguments)
    {
        DesiredType = targetType;
        IocService = iocService;
        Arguments = arguments;
    }
    public Type DesiredType { get; }
    public string TargetParameterName => "!Root Request!";
    public IIocService IocService { get; set; }
    public Type? TypeBeingConstructed => null;
    public IEnumerable<object> Arguments { get; }


    public bool IsCancelled { get; set; }

    public IBindingRequest? Parent => null;

    public string Trace => this.Print();
    public override string ToString() => Trace;

    public CreateSingletonRequest? SingletonRequestParent => null;
    public IRegisterDispose DisposeScope => IocService.DefaultDisposeRegistration;
    public IScope SharingScope => RootScope.Instance;
}