using System;
using System.Diagnostics;

namespace Melville.INPC;

/// <summary>
/// <p>Generate a dependency property.</p>
/// <p>On a dependency property declaration. -- declares the helper methods</p>
/// <p>[GenerateDP(typeof(type), "Name", Attached = true, Nullabel= true)]</p>
/// <p>[GenerateDP]void OnPropertyChanged(int oldValue, int newvalue) = nonattached prop</p>
/// <p>[GenerateDP]void OnPropertyChanged(int newValue = defaultValue) = nonattached prop</p>
/// <p>[GenerateDP]static void OnPropertyChanged(DepednencyObject obj, int newValue = defaultValue) = attached prop</p>
/// <p>[GenerateDP]static void OnPropertyChanged(ObjectType obj, int newValue = defaultValue) = attached prop</p>
/// </summary>
[Conditional("ShowCodeGenAttributes")]
[AttributeUsage(
    AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Property | AttributeTargets.Field |
    AttributeTargets.Event | AttributeTargets.All, Inherited = false, AllowMultiple = true)]
public sealed class GenerateBPAttribute : Attribute
{
    /// <summary>
    /// Generate an attached property.
    /// </summary>
    public bool Attached { get; set; }
    /// <summary>
    /// Generate a nullable property
    /// </summary>
    public bool Nullable { get; set; }
    /// <summary>
    /// C# string for the default value of the property.
    /// </summary>
    public object? Default { get; set; }
    /// <summary>
    /// Xml Documentation to attach to the property.
    /// </summary>
    public string XmlDocumentation {get; set; } = "";

    /// <summary>
    /// Default constructor for GenerateDpAttribute
    /// </summary>
    public GenerateBPAttribute()
    {
    }

    /// <summary>
    /// Create a GenerateDPAttribute
    /// </summary>
    /// <param name="targetType">Type of the desired dependency property.</param>
    /// <param name="propName">Name of the desired dependency property.</param>
    public GenerateBPAttribute(Type targetType, string propName = "")
    {
    }
}