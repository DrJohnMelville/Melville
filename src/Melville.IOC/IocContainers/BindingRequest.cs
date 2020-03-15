using System;
using System.Linq;
using System.Reflection;
using Melville.IOC.TypeResolutionPolicy;

namespace Melville.IOC.IocContainers
{
    public interface IBindingRequest
    {
        Type DesiredType { get; }
        Type? TypeBeingConstructed { get; }
        string TargetParameterName { get; }
        IIocService IocService { get; set; }
        IBindingRequest CreateSubRequest(ParameterInfo info)=> new ParameterBindingRequest(info, this);
        IBindingRequest CreateSubRequest(Type type)=> new TypeChangeBindingRequest(this, type);
        IBindingRequest CreateSubRequest(Type type, params object[] parameters)=> 
            new ParameterizedRequest(this, type, parameters);
        IBindingRequest Clone() => new ClonedBindingRequest(this);

        bool HasDefaultValue(out object? value)
        {
            value = null;
            return false;
        }

        object?[] ExtraParamsForChild { get; set; }
        object?[] ExtraParamsFromParent { get; }
    }
    
    public class RootBindingRequest : IBindingRequest
    {
        public RootBindingRequest(Type targetType, IIocService iocService)
        {
            DesiredType = targetType;
            IocService = iocService;
        }
        public Type DesiredType { get; }
        public string TargetParameterName => "!Root Request!";
        public IIocService IocService { get; set; }
        public Type? TypeBeingConstructed => null;
        public object?[] ExtraParamsForChild { get; set;} = Array.Empty<object>();
        public object?[] ExtraParamsFromParent => Array.Empty<object>();
    }
    
    public class ClonedBindingRequest: IBindingRequest
    {
        private IBindingRequest rootRequest;

        public ClonedBindingRequest(IBindingRequest rootRequest)
        {
            this.rootRequest = rootRequest;
            IocService = rootRequest.IocService;
            // copy the array because it gets destroyed
            ExtraParamsFromParent = rootRequest.ExtraParamsFromParent.ToArray(); 
            ExtraParamsForChild = rootRequest.ExtraParamsForChild.ToArray();
        }

        public Type DesiredType => rootRequest.DesiredType;
        public Type? TypeBeingConstructed => rootRequest.TypeBeingConstructed;
        public string TargetParameterName => rootRequest.TargetParameterName;
        public IIocService IocService { get; set; }
        public object?[] ExtraParamsForChild { get; set; }
        public object?[] ExtraParamsFromParent { get; }
    }

    public class ForwardingRequest : IBindingRequest
    {
        private readonly IBindingRequest inner;

        public ForwardingRequest(IBindingRequest inner)
        {
            this.inner = inner;
        }

        public virtual Type DesiredType => inner.DesiredType;
        public virtual string TargetParameterName => inner.TargetParameterName;
        public virtual Type? TypeBeingConstructed => inner.TypeBeingConstructed;

        public virtual IIocService IocService
        {
            get => inner.IocService;
            set => inner.IocService = value;
        }

        public object?[] ExtraParamsFromParent => inner.ExtraParamsForChild;
        public object?[] ExtraParamsForChild { get; set;} = Array.Empty<object>();
        public virtual bool HasDefaultValue(out object? value)
        {
            value = null;
            return false;
        }
    }
    public class ParameterBindingRequest : ForwardingRequest
    {
        private readonly ParameterInfo parameter;
        public ParameterBindingRequest(ParameterInfo parameter, IBindingRequest parentRequest): base(parentRequest)
        {
            this.parameter = parameter;
        }
        public override Type DesiredType => parameter.ParameterType;
        public override string TargetParameterName => parameter.Name ?? "";
        public override Type? TypeBeingConstructed => parameter.Member.DeclaringType;

        public override bool HasDefaultValue(out object? value)
        {
            value = parameter.DefaultValue;
            return parameter.HasDefaultValue;
        }
    }
    public class TypeChangeBindingRequest : ForwardingRequest
    {
        public TypeChangeBindingRequest(IBindingRequest inner, Type targetType): base(inner)
        {
            this.DesiredType = targetType;
        }
        public override Type DesiredType { get; }
    }

}