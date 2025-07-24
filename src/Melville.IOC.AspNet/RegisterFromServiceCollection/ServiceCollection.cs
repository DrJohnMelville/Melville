using System.Collections;
using System.Collections.Generic;
using Melville.INPC;
using Microsoft.Extensions.DependencyInjection;

namespace Melville.IOC.AspNet.RegisterFromServiceCollection;

public partial class ServiceCollection : IServiceCollection
{
    [DelegateTo] private IList<ServiceDescriptor> descriptors = new List<ServiceDescriptor>();
    public IEnumerator<ServiceDescriptor> GetEnumerator()
    {
        return descriptors.GetEnumerator();
    }
    
    IEnumerator IEnumerable.GetEnumerator()
    {
        return ((IEnumerable) descriptors).GetEnumerator();
    }
}