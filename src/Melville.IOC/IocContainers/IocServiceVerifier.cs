using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text.RegularExpressions;
using Melville.IOC.TypeResolutionPolicy;

namespace Melville.IOC.IocContainers;

public readonly struct IocServiceVerifier(params IEnumerable<Type> typesToTest)
{
    #region Create and add Types
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
    #endregion

    #region Filtering

    public IocServiceVerifier IfNotNamed(string regex) => IfNotNamed(new Regex(regex));
    public IocServiceVerifier IfNotNamed(Regex regex) => If(i => !regex.IsMatch(i.Name));
    public IocServiceVerifier IfNamed(string regex) => IfNamed(new Regex(regex));
    public IocServiceVerifier IfNamed(Regex regex) => If(i => regex.IsMatch(i.Name));

    public IocServiceVerifier If(Func<Type, bool> filter) => new(typesToTest.Where(filter));
    #endregion

    #region  Testing
    public IEnumerable<Type> UncreatableTypes(IIocService ioc, params object[] parameters) => 
        typesToTest.Distinct().Where(type => !ioc.CanGet(type, parameters));

    public void VerifyCreatable(IIocService ioc, params object[] parameters)
    {
        if (UncreatableTypes(ioc, parameters).ToList() is { Count: > 0 } types)
        {
            throw new IocException("Cannot Create the following: " +
                                   string.Join(", ", types.Select(i => i.Name)));
        } 
    }

    [Conditional("DEBUG")]
    public void VerifyCreatableInDebugBuilds(IIocService ioc, params object[] parameters) => 
        VerifyCreatable(ioc, parameters);
    #endregion
}
