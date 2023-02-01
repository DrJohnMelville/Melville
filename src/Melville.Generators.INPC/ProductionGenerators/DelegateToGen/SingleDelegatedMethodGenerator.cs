using Melville.Generators.INPC.GenerationTools.CodeWriters;
using Microsoft.CodeAnalysis;
using System.Collections.Immutable;
using System;
using System.Linq;
using Melville.Generators.INPC.GenerationTools.AstUtilities;
using Melville.Generators.INPC.ProductionGenerators.DelegateToGen.MethodMappings;

namespace Melville.Generators.INPC.ProductionGenerators.DelegateToGen;

public readonly struct SingleDelegatedMethodGenerator
{
    private readonly CodeWriter cw;
    private readonly DelegatedMethodGenerator parent;

    public SingleDelegatedMethodGenerator(CodeWriter cw, DelegatedMethodGenerator parent)
    {
        this.cw = cw;
        this.parent = parent;
    }

    public void GenerateForwardingMember(ISymbol member)
    {
        CopyInheritedMemberAttributes(member);
        switch (member)
        {
            case IPropertySymbol { IsIndexer: true } ps:
                GenerateIndexer(ps);
                break;
            case IPropertySymbol ps:
                GenerateProperty(ps);
                break;
            case IMethodSymbol ms:
                TryGenerateMethod(ms);
                break;
            case IEventSymbol es:
                GenerateEvent(es);
                break;
            default:
                cw.AppendLine($"// call {member.Name} using : {parent.MethodPrefix}");
                break;
        }
    }

    private void CopyInheritedMemberAttributes(ISymbol member)
    {
        new AttributeCopier(cw, "").CopyAttributesFrom(member.DeclaringSyntaxReferences);
    }
    private void CopyTargetMemberAttributes(string kind) =>
        new AttributeCopier(cw, kind).CopyAttributesFrom(parent.TargetHolderSymbol.DeclaringSyntaxReferences);

    private void GenerateIndexer(IPropertySymbol ps)
    {
        var substitute = parent.WrappingStrategy.MapType(ps.Type);
        CopyTargetMemberAttributes("property");
        MemberPrefix(substitute.FinalType.FullyQualifiedName(), "this", ps);
        ParameterList(ps.Parameters, RenderParameter, "[", "]");
        cw.AppendLine();
        PropertyBlock(ps, $"[{string.Join(", ", ps.Parameters.Select(i => i.Name))}]", substitute);
    }


    private void GenerateProperty(IPropertySymbol ps)
    {
        var substitute = parent.WrappingStrategy.MapType(ps.Type);

        CopyTargetMemberAttributes("property");
        MemberPrefix(substitute.FinalType.FullyQualifiedName(), ps.Name, ps);
        cw.AppendLine();
        PropertyBlock(ps, "." + ps.Name, substitute);
    }

    private void PropertyBlock(IPropertySymbol ps, string propertyCall, MappedMethod mappedMethod)
    {
        using (cw.CurlyBlock())
        {
            if (ps.GetMethod != null)
            {
                cw.AppendLine($"get{mappedMethod.OpenBody}{parent.MethodPrefix}{propertyCall}{mappedMethod.CloseBody}");
            }

            if (ps.SetMethod is { } setMethod)
            {
                var setSubs = parent.WrappingStrategy.MapType(ps.SetMethod.ReturnType);
                cw.AppendLine($"{SetMethodKeyword(setMethod)}{setSubs.OpenBody}{parent.MethodPrefix}{propertyCall} = {mappedMethod.CastResultTo(ps.Type)}value{setSubs.CloseBody}");
            }
        }
    }


    private void GenerateEvent(IEventSymbol es)
    {
        CopyTargetMemberAttributes("event");
        MemberPrefix("event " + es.Type.FullyQualifiedName(), es.Name, es);
        cw.AppendLine();
        using (cw.CurlyBlock())
        {
            if (es.AddMethod != null)
            {
                cw.AppendLine($"add => {parent.MethodPrefix}.{es.Name} += value;");
            }

            if (es.RemoveMethod != null)
            {
                cw.AppendLine($"remove => {parent.MethodPrefix}.{es.Name} -= value;");
            }
        }
    }

    private void TryGenerateMethod(IMethodSymbol ms)
    {
        if (IsSpecialExcludedMethod(ms)) return;
        GenerateMethod(ms);
    }

    private void GenerateMethod(IMethodSymbol ms)
    {
        CopyTargetMemberAttributes("method");

        var substitute = parent.WrappingStrategy.MapType(ms.ReturnType);

        MemberPrefix(substitute.FinalType.FullyQualifiedName(), ms.Name, ms);
        AppendTypeParamList(ms.TypeParameters);
        ParameterList(ms.Parameters, RenderParameter, "(", ")");
        cw.Append(substitute.OpenBody);
        cw.Append(parent.MethodPrefix);
        cw.Append(".");
        cw.Append(ms.Name);
        AppendTypeParamList(ms.TypeParameters);
        ParameterList(ms.Parameters, RenderArgument, "(", ")");
        cw.AppendLine(substitute.CloseBody);
    }

    private void RenderArgument(IParameterSymbol i)
    {
        cw.Append(HandleRefKind(i.RefKind));
        cw.Append(i.Name);
    }

    private void RenderParameter(IParameterSymbol i)
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

    private void TryRenderEnumCast(ITypeSymbol type)
    {
        if (type is INamedTypeSymbol { EnumUnderlyingType: { } })
        {
            cw.Append("(");
            cw.Append(type.FullyQualifiedName());
            cw.Append(")");
        }
    }

    private string HandleRefKind(RefKind refKind) => refKind switch
    {
        RefKind.Ref => "ref ",
        RefKind.Out => "out ",
        RefKind.In => "in ",
        _ => ""
    };

    private string ExplicitValue(string? value) => value switch
    {
        null => "default",
        "True" => "true",
        "False" => "false",
        var i => i
    };

    private void ParameterList(
        ImmutableArray<IParameterSymbol> parameters,
        Action<IParameterSymbol> display, string open, string close)
    {
        cw.Append(open);
        AppendArgumentList(parameters, display);
        cw.Append(close);
    }

    // we don't generate the component methods of properties, events, or indexers, because
    // we generate those using higher level constructs.
    private bool IsSpecialExcludedMethod(IMethodSymbol ms) => !ms.CanBeReferencedByName;

    private void AppendArgumentList(ImmutableArray<IParameterSymbol> parameters,
        Action<IParameterSymbol> paramPrinter)
    {
        if (parameters.Length == 0) return;
        paramPrinter(parameters[0]);
        foreach (var parameter in parameters.Skip(1))
        {
            cw.Append(", ");
            paramPrinter(parameter);
        }
    }

    private void AppendTypeParamList(
        ImmutableArray<ITypeParameterSymbol> parameters)
    {
        if (parameters.Length == 0) return;
        cw.Append("<");
        cw.Append(string.Join(",", parameters.Select(i => i.Name)));
        cw.Append(">");
    }

    private void MemberPrefix(string typeName, string memberName, ISymbol replacedSymbol)
    {
        cw.Append(parent.MemberDeclarationPrefix(replacedSymbol));
        cw.Append(typeName);
        cw.Append(" ");
        cw.Append(parent.MemberNamePrefix());
        cw.Append(memberName);
    }

    private string SetMethodKeyword(IMethodSymbol ps) => ps.IsInitOnly ? "init" : "set";

}