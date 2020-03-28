using System;
using Melville.IOC.IocContainers;

namespace Melville.IOC.BindingRequests
{
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
}