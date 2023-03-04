using System;
using System.Diagnostics;

namespace Melville.INPC
{
    /// <summary>
    /// This implementa a C style macro functionality.  Macros are generated for the cartesian product to
    /// all the MacroCode and MAcroItem attributes on a single code element.
    /// Each MacroItem attribute has multiple parameters which are accessed using ~0~, ~1~, or similar notation.
    /// </summary>
    [Conditional("ShowCodeGenAttributes")]
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Method | 
                    AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Event|
        AttributeTargets.Interface, Inherited=false, AllowMultiple=true)]
    public sealed class MacroCodeAttribute: Attribute
    {
        /// <summary>
        /// Code to print before the generated items.
        /// </summary>
        public object Prefix {get;set;} = "";
        /// <summary>
        /// Code to print after the generated items.
        /// </summary>
        public object Postfix {get;set;} = "";

        /// <summary>
        /// Code to generate in a macro operation
        /// </summary>
        /// <param name="text">The code to generate with ~0~, ~1~, or similar tags for the paramaters of the MacroItem</param>
        public MacroCodeAttribute(object text){}
    }
}