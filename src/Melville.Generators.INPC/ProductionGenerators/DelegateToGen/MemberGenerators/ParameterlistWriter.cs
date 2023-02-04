using System;
using System.Collections.Immutable;
using Melville.Generators.INPC.GenerationTools.AstUtilities;
using Melville.Generators.INPC.GenerationTools.CodeWriters;
using Microsoft.CodeAnalysis;

namespace Melville.Generators.INPC.ProductionGenerators.DelegateToGen.MemberGenerators;

internal readonly struct ParameterlistWriter
{
    private readonly CodeWriter cw;
    private readonly string openParen;
    private readonly ImmutableArray<IParameterSymbol> parameters;
    private readonly string closeParen;

    public ParameterlistWriter(
        CodeWriter cw, string openParen, ImmutableArray<IParameterSymbol> parameters,
        string closeParen)
    {
        this.cw = cw;
        this.openParen = openParen;
        this.parameters = parameters;
        this.closeParen = closeParen;
    }

    public void RenderParameterList() => RenderBracketedList(RenderSingleParameter);

    private void RenderSingleParameter(IParameterSymbol i)
    {
        new AttributeCopier(cw, "", AttributeWriterStrategy.MultiplePerLine)
            .CopyAttributesFrom(i.DeclaringSyntaxReferences);
        cw.Append(HandleRefKind(i.RefKind));
        cw.Append(i.Type.FullyQualifiedName());
        cw.Append(" ");
        cw.Append(i.Name);
        if (i.HasExplicitDefaultValue)
        {
            cw.Append(" = ");
            TryRenderEnumCast(i.Type);
            cw.Append(ExplicitValue(i.ExplicitDefaultValue?.ToString()));
        }
    }

    private string HandleRefKind(RefKind refKind) => refKind switch
    {
        RefKind.Ref => "ref ",
        RefKind.Out => "out ",
        RefKind.In => "in ",
        _ => ""
    };

    private void TryRenderEnumCast(ITypeSymbol type)
    {
        if (type is INamedTypeSymbol { EnumUnderlyingType: { } })
        {
            cw.Append("(");
            cw.Append(type.FullyQualifiedName());
            cw.Append(")");
        }
    }


    private string ExplicitValue(string? value) => value switch
    {
        null => "default",
        "True" => "true",
        "False" => "false",
        var i => i
    };

    private void RenderBracketedList(Action<IParameterSymbol> contentGenerator)
    {
        cw.Append(openParen);
        for (int i = 0; i < parameters.Length; i++)
        {
            if (i > 0) cw.Append(", ");
            contentGenerator(parameters[i]);
        }

        cw.Append(closeParen);
    }

    public void AppendArgumentList() =>
        RenderBracketedList(RenderSingleArgument);

    private void RenderSingleArgument(IParameterSymbol arg)
    {
        ;
        cw.Append(HandleRefKind(arg.RefKind));
        cw.Append(arg.Name);
    }
}