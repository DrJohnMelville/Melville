using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Melville.IOC.TypeResolutionPolicy;

namespace Melville.IOC.IocContainers;

public readonly struct IocServiceVerifier(params IEnumerable<Type> typesToTest)
{
    public IocServiceVerifier(IBindableIocService service) : this(
        TypesFrom(service))
    {
    }

    public IocServiceVerifier Add(IBindableIocService service) =>
        new IocServiceVerifier(typesToTest.Concat(TypesFrom(service)));

    private static IEnumerable<Type> TypesFrom(IBindableIocService service) => 
        service.ConfigurePolicy<ISuggestCreatableTypes>().CreatableTypes;

    public IocServiceVerifier Add(params IEnumerable<Type> types) =>
        new IocServiceVerifier(typesToTest.Concat(types));

    public IEnumerable<Type> UncreatableTypes(IIocService ioc) => 
        typesToTest.Distinct().Where(type => !ioc.CanGet(type));

    public void VerifyCreatable(IIocService ioc)
    {
        if (UncreatableTypes(ioc).ToList() is { Count: > 0 } types)
        {
            throw new IocException("Cannot Create the following: " +
                                   string.Join(", ", types.Select(i => i.Name)));
        } 
    }

    [Conditional("DEBUG")]
    public void VerifyCreatableInDebugBuilds(IIocService ioc) => VerifyCreatable(ioc);
}
