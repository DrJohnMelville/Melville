using System;
using System.Diagnostics;

namespace Melville.INPC;

[Conditional("ShowCodeGenAttributes")]
[AttributeUsage(AttributeTargets.Field | AttributeTargets.Property | AttributeTargets.Method,
    Inherited = false, AllowMultiple = false)]
public sealed class DelegateToAttribute : Attribute
{
    /// <summary>
    /// Sets the desired accessibility of the generated members,
    /// </summary>
    public Accessibility Accessibility { get; set; }
    public string WrapWith { get; set; }

    public DelegateToAttribute(){}
    public DelegateToAttribute(bool explicitImplementation){}
}