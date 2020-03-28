using System;
using System.Linq;
using Melville.IOC.IocContainers;

namespace Melville.IOC.BindingRequests
{
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
}