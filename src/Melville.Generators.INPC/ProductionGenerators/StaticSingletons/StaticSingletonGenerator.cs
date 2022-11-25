using System.Collections.Immutable;
using System.Linq;
using Melville.Generators.INPC.GenerationTools.AbstractGenerators;
using Melville.Generators.INPC.GenerationTools.AstUtilities;
using Melville.Generators.INPC.GenerationTools.CodeWriters;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace System.Runtime.CompilerServices.ProductionGenerators.StaticSingletons;

[Generator]
public class StaticSingletonGenerator: IIncrementalGenerator
{
    public const string attributeName = "Melville.INPC.StaticSingletonAttribute"; 
    public void Initialize(IncrementalGeneratorInitializationContext context)
    {
        context.RegisterSourceOutput(
            context.SyntaxProvider.ForAttributeWithMetadataName(attributeName,
                (i,_)=>i is ClassDeclarationSyntax,
                (i,_)=>i),
             Generate
            );
    }

    private void Generate(SourceProductionContext context, GeneratorAttributeSyntaxContext attr)
    {
        var writer = new SourceProductionCodeWriter(context);
        using (writer.GenerateInClassFile(attr.TargetNode, "StaticSingleton"))
        {
            new StaticSingletonCodeGenerator(attr).GenerateCode(writer);
        }
    }
}