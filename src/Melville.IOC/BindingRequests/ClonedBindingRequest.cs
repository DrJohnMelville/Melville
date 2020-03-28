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
            ArgumentsFromParent = rootRequest.ArgumentsFromParent.ToArray(); 
            ArgumentsFormChild = rootRequest.ArgumentsFormChild.ToArray();
        }

        public Type DesiredType => rootRequest.DesiredType;
        public Type? TypeBeingConstructed => rootRequest.TypeBeingConstructed;
        public string TargetParameterName => rootRequest.TargetParameterName;
        public IIocService IocService { get; set; }
        public object?[] ArgumentsFormChild { get; set; }
        public object?[] ArgumentsFromParent { get; }
    }
}