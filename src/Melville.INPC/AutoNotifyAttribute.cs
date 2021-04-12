using System;
using System.Diagnostics;

namespace Melville.INPC
{
    /// <summary>
    /// <p>Generate a property from a field.</p>
    /// <p>{PropertyName}SetFilter = filter on the input of the set method</p>
    /// <p>{PropertyName}GetFilter = Filter on output of the get member</p>
    /// <p>When{PropertyName}Changes({type} oldValue, {type} newValue) == called when a change happens</p>
    /// </summary>
    [Conditional("ShowCodeGenAttributes")]
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property | AttributeTargets.Class,
        Inherited = false, AllowMultiple = false)]
    public sealed class AutoNotifyAttribute : Attribute
    {
    }
}