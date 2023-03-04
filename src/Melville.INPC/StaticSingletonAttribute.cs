using System;
using System.Diagnostics;

namespace Melville.INPC;

/// <summary>
/// Generate a static member holding a singleton instance of this class and add a private
/// default constructor to prevent others from instantiating the class.
/// </summary>
[Conditional("ShowCodeGenAttributes")]
[AttributeUsage( AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
public sealed class StaticSingletonAttribute : Attribute
{
    /// <summary>
    /// The desired name of the Instance field.
    /// </summary>
    public string? Name { get; }

    /// <summary>
    /// Construct a StaticSingletonObject
    /// </summary>
    /// <param name="name">Desired name for the Instance field</param>
    public StaticSingletonAttribute(string? name = null)
    {
        Name = name;
    }
}