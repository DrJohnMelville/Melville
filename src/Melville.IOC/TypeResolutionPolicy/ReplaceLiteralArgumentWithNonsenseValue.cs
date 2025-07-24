using System;

namespace Melville.IOC.TypeResolutionPolicy;

/// <summary>
/// When checking if a function based factory can be created we need to check if the function could actually be
/// invoked.  To do this we need to remove the functions arguments from the types that need to be resolved for
/// a successful invocation.  LiteralBindingPolicy special cases this class and allows it to replace any class
/// that CanAssignTo replaces.  Since this class does not actually inherit from fakedType, this will cause a
/// typecast error if ever invoked, but since the CanGet branch never invokes the activation strategy this
/// technique is completely safe.
/// </summary>
public class ReplaceLiteralArgumentWithNonsenseValue
{
    private readonly Type fakedType;

    public ReplaceLiteralArgumentWithNonsenseValue(Type fakedType)
    {
        this.fakedType = fakedType;
    }

    public bool CanAssignTo(Type targetType) => targetType.IsAssignableFrom(fakedType);
}