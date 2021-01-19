using System.Collections.Generic;
using Melville.Generators.Tools.CodeWriters;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Melville.Generators.INPC.Macros
{
    public static class MacroRenderer
    {
        public static void RenderInType(this GeneratorExecutionContext context,
            GeneratedFileUniqueNamer namer,
            TypeDeclarationSyntax parentNode, IEnumerable<string> macros)
        {
            
            var codeWriter = new CodeWriter(context);
            RenderInClass(codeWriter, parentNode, string.Join("\r\r\n", macros));
            var code = codeWriter.ToString();
            context.AddSource(namer.CreateFileName(parentNode.Identifier.ToString()), code);
        }
        private static void RenderInClass(CodeWriter codeWriter, TypeDeclarationSyntax parentNode,
            string content)
        {
            using (codeWriter.GenerateEnclosingNamespaces(parentNode))
            using (codeWriter.GenerateEnclosingClasses(parentNode, ""))
            {
                codeWriter.Append(content);
            }
        }
    }
}