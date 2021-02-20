using System;
using System.Diagnostics;

namespace Melville.INPC
{
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