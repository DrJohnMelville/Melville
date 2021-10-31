using System;
using Melville.IOC.IocContainers;

namespace Melville.IOC.Activation;

public static class ActivatableTypesPolicy
{
    public static bool IsActivatable(Type type) => CheckIfTypeIsActivatable(type) == null;
    public static void ThrowIfNotActivatable(Type type)
    {
        if (CheckIfTypeIsActivatable(type) is {} msg)
            throw MakeException(msg, type);
    }

    private static string? CheckIfTypeIsActivatable(Type type)
    {
        if (type.IsAbstract) return "An abstract class cannot be a concrete binding type";
        if (type.IsInterface) return "An interface class cannot be a concrete binding type";
        if (type.IsGenericTypeDefinition) return "A generic type definition cannot be a concrete binding type.";
        return null;
    }

    private static Exception MakeException(string message, Type type) => 
        new IocException($"{type.Name}: {message}");
}