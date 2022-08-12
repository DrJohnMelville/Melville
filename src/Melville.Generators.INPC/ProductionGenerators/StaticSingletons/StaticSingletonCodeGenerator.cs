using System.Linq;
using System.Net.Sockets;
using System.Text;
using Melville.Generators.INPC.GenerationTools.AbstractGenerators;
using Melville.Generators.INPC.GenerationTools.AstUtilities;
using Melville.Generators.INPC.GenerationTools.CodeWriters;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace System.Runtime.CompilerServices.ProductionGenerators.StaticSingletons;

public class StaticSingletonCodeGenerator: ILabeledMembersSemanticModel
{
    public TypeDeclarationSyntax ClassDeclaration { get; }
    private readonly INamedTypeSymbol symbol;

    public StaticSingletonCodeGenerator(TypeDeclarationSyntax typeSyntax, INamedTypeSymbol symbol)
    {
        ClassDeclaration = typeSyntax;
        this.symbol = symbol;
    }

    public void GenerateCode(CodeWriter cw)
    {
        using var context = WriteCodeNear.Symbol(ClassDeclaration, cw);
        cw.Append("public static readonly ");
        cw.Append(symbol.FullyQualifiedName());
        cw.Append(" ");
        cw.Append(InstanceName());
        cw.AppendLine(" = new();");
        cw.Append("private ");
        cw.Append(symbol.Name);
        cw.AppendLine("() {}");
    }

    private string InstanceName()
    {
        return StaticSingletonGenerator.AttributeFinder.FindAttribute(ClassDeclaration)
            ?.AttributeParameters()
            .FirstOrDefault() ?? 
               "Instance";
    }
}