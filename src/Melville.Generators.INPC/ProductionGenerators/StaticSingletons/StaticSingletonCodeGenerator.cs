using System.Collections.Immutable;
using System.Linq;
using Melville.Generators.INPC.GenerationTools.AstUtilities;
using Melville.Generators.INPC.GenerationTools.CodeWriters;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace System.Runtime.CompilerServices.ProductionGenerators.StaticSingletons;

public class StaticSingletonCodeGenerator
{
    private readonly TypeDeclarationSyntax classDeclaration;
    private readonly INamedTypeSymbol symbol;
    private readonly string instanceName;

    public StaticSingletonCodeGenerator(GeneratorAttributeSyntaxContext context)
    {
        classDeclaration = (TypeDeclarationSyntax)context.TargetNode;
        symbol = (INamedTypeSymbol)context.TargetSymbol;
        instanceName = ComputeInstanceName(context.Attributes);
    }
    
    private static string ComputeInstanceName(ImmutableArray<AttributeData> attrs) =>
        attrs.FilterToAttributeType(StaticSingletonGenerator.attributeName)
            .SelectMany(i => i.AllValues().Select(j => j.Value))
            .OfType<string>()
            .DefaultIfEmpty("Instance")
            .First();


    public void GenerateCode(CodeWriter cw)
    {
        using var _ = WriteCodeNear.Symbol(classDeclaration, cw);
        cw.AppendLine($"public static readonly {symbol.FullyQualifiedName()} {instanceName} = new();");
        cw.AppendLine($$"""private {{symbol.Name}}() {}""");
    }
}