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
    private const string attributeName = "Melville.INPC.StaticSingletonAttribute"; 
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
            new StaticSingletonCodeGenerator((TypeDeclarationSyntax)attr.TargetNode,
                (INamedTypeSymbol)attr.TargetSymbol).GenerateCode(writer, instanceNamw(attr.Attributes));
        }
    }

    private string instanceNamw(ImmutableArray<AttributeData> attrs) =>
        attrs.FilterToAttributeType(attributeName)
            .SelectMany(i => i.AllValues().Select(j => j.Value))
            .OfType<string>()
            .DefaultIfEmpty("Instance")
            .First();

    public static readonly SearchForAttribute AttributeFinder = new("Melville.INPC.StaticSingletonAttribute");
}