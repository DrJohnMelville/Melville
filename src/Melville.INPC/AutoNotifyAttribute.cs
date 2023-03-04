using System;
using System.Diagnostics;

namespace Melville.INPC
{
    /// <summary>
    /// <p>Generate a property from a field.</p>
    /// <p>{PropertyName}SetFilter = filter on the input of the set method</p>
    /// <p>{PropertyName}GetFilter = Filter on output of the get member</p>
    /// <p>On{PropertyName}Changed({type} oldValue, {type} newValue) == called when a change happens</p>
    /// <p>On{PropertyName}Changed({type} newValue) == called when a change happens</p>
    /// <p>On{PropertyName}Changed() == called when a change happens</p>
    /// </summary>
    [Conditional("ShowCodeGenAttributes")]
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property | AttributeTargets.Class,
        Inherited = false, AllowMultiple = false)]
    public sealed class AutoNotifyAttribute : Attribute
    {
        /// <summary>
        /// Set the desired visibility of the generated property -- defaults to public
        /// </summary>
        public string PropertyModifier { get; set; } = "";
    }
}