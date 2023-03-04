using System;
using System.Diagnostics;

namespace Melville.INPC
{
    /// <summary>
    /// This implements a C style macro functionality.  Macros are generated for the cartesian product to
    /// all the MacroCode and MacroItem attributes on a single code element.
    /// Each MacroItem attribute has multiple parameters which are accessed using ~0~, ~1~, or similar notation.
    /// </summary>
    [Conditional("ShowCodeGenAttributes")]
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Method | 
                    AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Event |
        AttributeTargets.Interface, Inherited=false, AllowMultiple=true)]

    public sealed class MacroItemAttribute: Attribute
    {
        /// <summary>
        /// Construct a MacroItemAttribute
        /// </summary>
        /// <param name="text">The parameters to pass into the generator for this item.</param>
        public MacroItemAttribute(params object[] text){}
    }
}