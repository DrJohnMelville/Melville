using System;
using System.Diagnostics;

namespace Melville.INPC
{
    [Conditional("ShowCodeGenAttributes")]
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Method | AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Event, Inherited=false, AllowMultiple=true)]
    public sealed class MacroCodeAttribute: Attribute
    {
        public object Prefix {get;set;} = "";
        public object Postfix {get;set;} = "";
        public MacroCodeAttribute(object text){}
    }
}