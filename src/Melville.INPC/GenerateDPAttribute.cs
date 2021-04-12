using System;
using System.Diagnostics;

namespace Melville.INPC
{
    /// <summary>
    /// <p>Generate a dependency property.</p>
    /// <p>On a dependency property declaration. -- declares the helper methods</p>
    /// <p>[GenerateDP(typeof(type), "Name", Attached = true, Nullabel= true)]</p>
    /// <p>[GenerateDP]void OnPropertyChanged(int newValue, int oldvalue) = nonattached prop</p>
    /// <p>[GenerateDP]void OnPropertyChanged(int newValue = defaultValue) = nonattached prop</p>
    /// <p>[GenerateDP]static void OnPropertyChanged(DepednencyObject obj, int newValue = defaultValue) = attached prop</p>
    /// <p>[GenerateDP]static void OnPropertyChanged(ObjectType obj, int newValue = defaultValue) = attached prop</p>
    /// </summary>
    [Conditional("ShowCodeGenAttributes")]
    [AttributeUsage(
        AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Property | AttributeTargets.Field |
        AttributeTargets.Event | AttributeTargets.All, Inherited = false, AllowMultiple = true)]
    public sealed class GenerateDPAttribute : Attribute
    {
        public bool Attached { get; set; }
        public bool Nullable { get; set; }
        public object? Default { get; set; }

        public GenerateDPAttribute()
        {
        }

        public GenerateDPAttribute(Type targetType, string propName = "")
        {
        }
    }
}