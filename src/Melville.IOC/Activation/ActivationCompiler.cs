using System;
using System.Collections.Generic;
using System.Reflection;
using Melville.IOC.IocContainers;

namespace Melville.IOC.Activation;

public static class ActivationCompiler
{
    public static ConstructorInvoker Compile(Type targetType, params Type[] parameters) =>
        ConstructorInvoker.Create(
            targetType.GetConstructor(parameters)??
                throw new IocException("No constructor found for these parameters."));
}