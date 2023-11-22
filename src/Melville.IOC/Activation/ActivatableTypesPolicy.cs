using System;
using Melville.IOC.IocContainers;

namespace Melville.IOC.Activation;

public static class ActivatableTypesPolicy
{
    public static bool IsActivatable(Type type) => CheckIfTypeIsActivatable(type) == null;

    public static void ThrowIfNotActivatable(Type type)
    {
        if (CheckIfTypeIsActivatable(type) is { } msg)
            throw MakeException(msg, type);
    }

    private static string? CheckIfTypeIsActivatable(Type type) => type switch
    {
        { IsAbstract: true } => "An abstract class cannot be a concrete binding type",
        { IsInterface: true } => "An interface class cannot be a concrete binding type",
        { IsGenericTypeDefinition: true } =>
            "A generic type definition cannot be a concrete binding type.",
        _ => null
    };

    private static Exception MakeException(string message, Type type) =>
        new IocException($"{type.Name}: {message}");
}