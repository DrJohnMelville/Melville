using System;
using System.Diagnostics;

namespace Melville.INPC;

/// <summary>
/// Generate a constructor that sets the marked field or property
/// </summary>
[Conditional("ShowCodeGenAttributes")]
[AttributeUsage(AttributeTargets.Field | AttributeTargets.Property | AttributeTargets.Class,
    Inherited = false, AllowMultiple = false)]
public sealed class FromConstructorAttribute : Attribute
{
}