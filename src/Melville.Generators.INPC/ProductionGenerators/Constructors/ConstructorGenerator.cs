using System.Linq;
using Melville.Generators.INPC.GenerationTools.CodeWriters;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Melville.Generators.INPC.ProductionGenerators.Constructors;

[Generator]
public class ConstructorGenerator : IIncrementalGenerator
{
    public const string AttributeName = "Melville.INPC.FromConstructorAttribute";

    public void Initialize(IncrementalGeneratorInitializationContext context)
    {
        context.RegisterSourceOutput(
            context.SyntaxProvider.ForAttributeWithMetadataName(AttributeName,
                    static (i, _) => i is
                        ClassDeclarationSyntax or VariableDeclaratorSyntax or PropertyDeclarationSyntax,
                    static (i, _) => i)
                .Collect()
                .SelectMany(static (i, _) => i
                .GroupBy(KeySelector, SymbolEqualityComparer.Default)
                .Select(static i=>i.Key)),
            Generate
        );
    }

    private void Generate(SourceProductionContext codeTarget, ISymbol classToGenerate)
    {
        if (classToGenerate is not ITypeSymbol typeToGenerate) return;
        var cw = new SourceProductionCodeWriter(codeTarget);
        using (cw.GenerateInClassFile(classToGenerate, "Constructors"))
        {
            new WriteConstructorsForSymbol(typeToGenerate, cw).Genarate();
        }
    }

    private static ITypeSymbol KeySelector(GeneratorAttributeSyntaxContext j)
    {
        return j.TargetSymbol as ITypeSymbol ?? j.TargetSymbol.ContainingType;
    }
}