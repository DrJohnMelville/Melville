using System;
using System.Diagnostics;

namespace Melville.INPC
{
    [Conditional("ShowCodeGenAttributes")]
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Method | AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Event, Inherited=false, AllowMultiple=true)]
    public sealed class MacroItemAttribute: Attribute
    {
        public MacroItemAttribute(params object[] text){}
    }
}