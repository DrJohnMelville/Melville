﻿using System.Linq;
using Melville.Generators.INPC.CodeWriters;
using Melville.Generators.INPC.PartialTypeGenerators;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Melville.Generators.INPC.Macros
{
      [Generator]
    public class MacroGenerator:PartialTypeGenerator
    {
        public MacroGenerator() : base("MacroGen", 
            "Melville.MacroGen.MacroCodeAttribute", "Melville.MacroGen.MacroCodeAttribute")
        {
        }

        protected override bool GlobalDeclarations(CodeWriter cw)
        {
            cw.Append("namespace Melville.MacroGen");
            using var ns = cw.CurlyBlock();
            AttributeDeclarations(cw);
            return true;
        }

        private static void AttributeDeclarations(CodeWriter cw)
        {
            cw.AddPrefixLine("using System;");
            cw.AppendLine(
                @"[AttributeUsage(AttributeTargets.Field | AttributeTargets.Property | AttributeTargets.Class | AttributeTargets.Method | AttributeTargets.Field | AttributeTargets.Event | AttributeTargets.Struct, Inherited = false, AllowMultiple = true)]");
            cw.AppendLine("internal sealed class MacroCodeAttribute : Attribute");
            using (cw.CurlyBlock())
            {
                cw.AppendLine(@"public object Prefix {get;set;} = """";");
                cw.AppendLine(@"public object Postfix {get;set;} = """";");
                cw.AppendLine("public MacroCodeAttribute(object text){}");
            }

            cw.AppendLine(
                @"[AttributeUsage(AttributeTargets.Field | AttributeTargets.Property | AttributeTargets.Class | AttributeTargets.Method | AttributeTargets.Field | AttributeTargets.Event | AttributeTargets.Struct, Inherited = false, AllowMultiple = true)]");
            cw.AppendLine("internal sealed class MacroItemAttribute : Attribute");
            using (cw.CurlyBlock())
            {
                cw.AppendLine("public MacroItemAttribute(params object[] text){}");
            }
        }

        protected override bool GenerateClassContents(
            IGrouping<TypeDeclarationSyntax, MemberDeclarationSyntax> input, CodeWriter cw)
        {
            foreach (var node in input)
            {
                MacroSyntaxInterpreter.ExpandSingleMacroSet(node, cw);
            }
            return true;
        }
    }
}