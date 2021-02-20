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
            "Melville.INPC.MacroCodeAttribute", "Melville.INPC.MacroCodeAttribute")
        {
        }

        protected override bool GlobalDeclarations(CodeWriter cw) => false;

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