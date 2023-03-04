using System;
using System.Diagnostics;

namespace Melville.INPC;

/// <summary>
/// Generate code to delegate calls to a specified member
/// </summary>
[Conditional("ShowCodeGenAttributes")]
[AttributeUsage(AttributeTargets.Field | AttributeTargets.Property | AttributeTargets.Method,
    Inherited = false, AllowMultiple = true)]
public sealed class DelegateToAttribute : Attribute
{

    /// <summary>
    /// Sets the desired accessibility of the generated members,
    /// </summary>
    public Visibility Visibility { get; set; }
    /// <summary>
    /// Sets the name of a method that will attempt to wrap the delegating call in another method if types are correct.
    /// </summary>
    public string WrapWith { get; set; } = "";
    /// <summary>
    /// Implement interface methods explicitly.
    /// </summary>
    public bool ExplicitImplementation { get; set; }
    /// <summary>
    /// A regex expression that will cause only methods matching the regex to be forwarded.
    /// </summary>
    public string? Filter { get; set; }
    /// <summary>
    /// A regex expression.  MEmber names matching this regex will not be forwarded.
    /// </summary>
    public string? Exclude { get; set; }
    /// <summary>
    /// This string works with the filter expression.  Regex replacement using the filter expression
    /// and this as the replace expression will determine the final name of the generated method.
    /// </summary>
    public string? Rename { get; set; }

    /// <summary>
    /// Create a DelegateTo attribute
    /// </summary>
    public DelegateToAttribute(){}

    /// <summary>
    /// Create a DelegateTo attribute
    /// </summary>
    /// <param name="explicitImplementation">If true will implement interface methods explicitly.</param>
    public DelegateToAttribute(bool explicitImplementation)
    {
        ExplicitImplementation = explicitImplementation;
    }
}