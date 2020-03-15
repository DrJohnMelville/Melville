using System;
using System.Reflection;
using Melville.IOC.IocContainers;
using Melville.MVVM.Wpf.DiParameterSources;

namespace WebDashboard.Startup
{
    public class DiBridge:IDIIntegration
    {
        private IIocService service;

        public DiBridge(IIocService service) => this.service = service;

        public IDIIntegration CreateScope() => new DiBridge(service.CreateScope());
    
        public void Dispose()
        {
            (service as IDisposable)?.Dispose();
        }

        public object? Get(ParameterInfo info) => service.Get(ParameterInfoToRequest(info));

        private IBindingRequest ParameterInfoToRequest(ParameterInfo info) => 
            ((IBindingRequest) new RootBindingRequest(info.ParameterType, service)).CreateSubRequest(info);

        public object? Get(Type type) => service.Get(type);
    }
}