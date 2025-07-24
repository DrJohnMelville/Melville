using System;
using System.Collections.Generic;
using Melville.IOC.IocContainers;
using Melville.IOC.IocContainers.ActivationStrategies;

namespace Melville.IOC.BindingRequests;

public class ForwardingRequest(IBindingRequest inner) : IBindingRequest
{
    public IBindingRequest Parent { get; } = inner;
    public virtual Type DesiredType => Parent.DesiredType;
    public virtual string TargetParameterName => Parent.TargetParameterName;
    public virtual Type? TypeBeingConstructed => Parent.TypeBeingConstructed;

    public virtual IIocService IocService => Parent.IocService;

    public virtual IEnumerable<object> Arguments => Parent.Arguments;

    public virtual bool HasDefaultValue(out object? value)
    {
        value = null;
        return false;
    }

    public bool IsCancelled
    {
        get => Parent.IsCancelled;
        set => Parent.IsCancelled = value;
    }

    public string Trace => this.Print();
    public override string ToString() => Trace;
    public virtual IRegisterDispose DisposeScope => inner.DisposeScope;
    public virtual IScope SharingScope => inner.SharingScope;
}