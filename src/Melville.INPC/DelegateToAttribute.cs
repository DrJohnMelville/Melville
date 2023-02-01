using System;
using System.Diagnostics;

namespace Melville.INPC
{
    [Conditional("ShowCodeGenAttributes")]
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property | AttributeTargets.Method,
        Inherited = false, AllowMultiple = false)]
    public sealed class DelegateToAttribute : Attribute
    {
        public DelegateToAttribute(){}
        public DelegateToAttribute(bool explicitImplementation){}
        public DelegateToAttribute(string postProcessName){}
    }
}