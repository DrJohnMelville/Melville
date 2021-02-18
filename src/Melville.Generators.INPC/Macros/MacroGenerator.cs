using System;
using System.Linq;
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
            AttributeDeclarations(cw);
            return true;
        }

        private static void AttributeDeclarations(CodeWriter cw)
        {
            using (cw.ComplexAttribute("MacroCode",
                AttributeTargets.Field | AttributeTargets.Property | AttributeTargets.Class | AttributeTargets.Field |
                AttributeTargets.Event | AttributeTargets.Struct | AttributeTargets.Method, true))
            {
                cw.AppendLine(@"public object Prefix {get;set;} = """";");
                cw.AppendLine(@"public object Postfix {get;set;} = """";");
                cw.AppendLine("public MacroCodeAttribute(object text){}");
            }

            using (cw.ComplexAttribute("MacroItem",
                AttributeTargets.Field | AttributeTargets.Property | AttributeTargets.Class | AttributeTargets.Field |
                AttributeTargets.Event | AttributeTargets.Struct | AttributeTargets.Method, true))
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