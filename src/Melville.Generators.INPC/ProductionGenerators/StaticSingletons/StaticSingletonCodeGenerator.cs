using System.Diagnostics;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using Melville.Generators.INPC.GenerationTools.AbstractGenerators;
using Melville.Generators.INPC.GenerationTools.AstUtilities;
using Melville.Generators.INPC.GenerationTools.CodeWriters;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace System.Runtime.CompilerServices.ProductionGenerators.StaticSingletons;

public class StaticSingletonCodeGenerator
{
    public TypeDeclarationSyntax ClassDeclaration { get; }
    private readonly INamedTypeSymbol symbol;

    public StaticSingletonCodeGenerator(TypeDeclarationSyntax typeSyntax, INamedTypeSymbol symbol)
    {
        ClassDeclaration = typeSyntax;
        this.symbol = symbol;
    }

    public void GenerateCode(CodeWriter cw, string instanceNamw)
    {
        using var context = WriteCodeNear.Symbol(ClassDeclaration, cw);
        var computedName = InstanceName();
       // Debug.Assert(instanceNamw == computedName);
        cw.AppendLine($"public static readonly {symbol.FullyQualifiedName()} {instanceNamw} = new();");
        cw.AppendLine($$"""private {{symbol.Name}}() {}""");
    }

    private string InstanceName()
    {
        return StaticSingletonGenerator.AttributeFinder.FindAttribute(ClassDeclaration)
            ?.AttributeParameters()
            .FirstOrDefault() ?? 
               "Instance";
    }
}