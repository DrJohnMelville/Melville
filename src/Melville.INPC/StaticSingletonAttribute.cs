using System;
using System.Diagnostics;

namespace Melville.INPC;

[Conditional("ShowCodeGenAttributes")]
[AttributeUsage( AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
public sealed class StaticSingletonAttribute : Attribute
{
    public string? Name { get; }

    public StaticSingletonAttribute(string? name = null)
    {
        Name = name;
    }
}