using Melville.IOC.IocContainers;

namespace Melville.IOC.BindingRequests;

public class ChangeIocServiceRequest(IBindingRequest inner, IIocService newScope) : ForwardingRequest(inner)
{
    public override IIocService IocService => newScope;

    public static IBindingRequest TryBindToService(IBindingRequest br, IIocService service) =>
        br.IocService == service ? br : new ChangeIocServiceRequest(br, service);
}