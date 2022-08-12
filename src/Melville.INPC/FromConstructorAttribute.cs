using System;
using System.Diagnostics;

namespace Melville.INPC;

[Conditional("ShowCodeGenAttributes")]
[AttributeUsage(AttributeTargets.Field | AttributeTargets.Property | AttributeTargets.Class,
    Inherited = false, AllowMultiple = false)]
public sealed class FromConstructorAttribute : Attribute
{
    public FromConstructorAttribute(){}
}