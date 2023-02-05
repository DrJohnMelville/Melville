using System;
using System.Diagnostics;

namespace Melville.INPC;

[Conditional("ShowCodeGenAttributes")]
[AttributeUsage(AttributeTargets.Field | AttributeTargets.Property | AttributeTargets.Method,
    Inherited = false, AllowMultiple = true)]
public sealed class DelegateToAttribute : Attribute
{

    /// <summary>
    /// Sets the desired accessibility of the generated members,
    /// </summary>
    public Visibility Visibility { get; set; }
    public string WrapWith { get; set; } = "";
    public bool ExplicitImplementation { get; set; }
    public string? Filter { get; set; }
    public string? Exclude { get; set; }
    public string? Rename { get; set; }

    public DelegateToAttribute(){}
    public DelegateToAttribute(bool explicitImplementation)
    {
        ExplicitImplementation = explicitImplementation;
    }
}