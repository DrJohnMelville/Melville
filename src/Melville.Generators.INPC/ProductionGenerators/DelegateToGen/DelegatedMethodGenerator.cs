﻿using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using Melville.Generators.INPC.GenerationTools.AstUtilities;
using Melville.Generators.INPC.GenerationTools.CodeWriters;
using Microsoft.CodeAnalysis;

namespace Melville.Generators.INPC.ProductionGenerators.DelegateToGen;

public interface IDelegatedMethodGenerator
{
    void GenerateForwardingMethods(CodeWriter cw);
}

public abstract class DelegatedMethodGenerator : IDelegatedMethodGenerator
{
    protected readonly ITypeSymbol TargetType;
    private readonly string methodPrefix;
    private readonly ISymbol targetSymbol;

    protected ITypeSymbol ParentSymbol => targetSymbol.ContainingType;

    protected abstract string MemberDeclarationPrefix();
    protected virtual string MemberNamePrefix() => "";
    protected abstract IEnumerable<ISymbol> MembersThatCouldBeForwarded();

    protected DelegatedMethodGenerator(
        ITypeSymbol targetType, string methodPrefix, ISymbol targetSymbol)
    {
        this.TargetType = targetType;
        this.methodPrefix = methodPrefix;
        this.targetSymbol = targetSymbol;
    }

    public string InheritFrom() => TargetType.FullyQualifiedName();

    public void GenerateForwardingMethods(CodeWriter cw)
    {
        foreach (var member in MembersToForward())
        {
            GenerateForwardingMember(cw, member);
        }
    }

    private IEnumerable<ISymbol> MembersToForward() =>
        MembersThatCouldBeForwarded()
            .Where(i => ImplementationMissing(i));

    private IEnumerable<ITypeSymbol> TargetTypeAndParents() =>
        TargetType.AllInterfaces.Cast<ITypeSymbol>().Append(TargetType);

    protected abstract bool ImplementationMissing(ISymbol i);

    private void GenerateForwardingMember(CodeWriter cw, ISymbol member)
    {
        CopyInheritedMemberAttributes(cw, member);
        switch (member)
        {
            case IPropertySymbol { IsIndexer: true } ps:
                GenerateIndexer(ps, cw);
                break;
            case IPropertySymbol ps:
                GenerateProperty(ps, cw);
                break;
            case IMethodSymbol ms:
                TryGenerateMethod(ms, cw);
                break;
            case IEventSymbol es:
                GenerateEvent(es, cw);
                break;
            default:
                cw.AppendLine($"// call {member.Name} using : {methodPrefix}");
                break;
        }
    }

    private void CopyInheritedMemberAttributes(CodeWriter cw, ISymbol member)
    {
        new AttributeCopier(cw, "").CopyAttributesFrom(member.DeclaringSyntaxReferences);
    }

    private void GenerateIndexer(IPropertySymbol ps, CodeWriter cw)
    {
        CopyTargetMemberAttributes(cw, "property");
        MemberPrefix(cw, ps.Type.FullyQualifiedName(), "this");
        ParameterList(cw, ps.Parameters, RenderParameter, "[", "]");
        cw.AppendLine();
        PropertyBlock(ps, cw, $"[{string.Join(", ", ps.Parameters.Select(i => i.Name))}]");
    }

    private void CopyTargetMemberAttributes(CodeWriter cw, string kind) => 
        new AttributeCopier(cw, kind).CopyAttributesFrom(targetSymbol.DeclaringSyntaxReferences);

    private void GenerateProperty(IPropertySymbol ps, CodeWriter cw)
    {
        CopyTargetMemberAttributes(cw, "property");
        MemberPrefix(cw, ps.Type.FullyQualifiedName(), ps.Name);
        cw.AppendLine();
        PropertyBlock(ps, cw, "." + ps.Name);
    }

    private void PropertyBlock(IPropertySymbol ps, CodeWriter cw, string propertyCall)
    {
        using (cw.CurlyBlock())
        {
            if (ps.GetMethod != null)
            {
                cw.AppendLine($"get => {methodPrefix}{propertyCall};");
            }

            if (ps.SetMethod is { } setMethod)
            {
                cw.AppendLine($"{setMethodKeyword(setMethod)} => {methodPrefix}{propertyCall} = value;");
            }
        }
    }


    private void GenerateEvent(IEventSymbol es, CodeWriter cw)
    {
        CopyTargetMemberAttributes(cw, "event");
        MemberPrefix(cw, "event " + es.Type.FullyQualifiedName(), es.Name);
        cw.AppendLine();
        using (cw.CurlyBlock())
        {
            if (es.AddMethod != null)
            {
                cw.AppendLine($"add => {methodPrefix}.{es.Name} += value;");
            }

            if (es.RemoveMethod != null)
            {
                cw.AppendLine($"remove => {methodPrefix}.{es.Name} -= value;");
            }
        }
    }

    private void TryGenerateMethod(IMethodSymbol ms, CodeWriter cw)
    {
        if (IsSpecialExcludedMethod(ms)) return;
        GenerateMethod(ms, cw);
    }

    private void GenerateMethod(IMethodSymbol ms, CodeWriter cw)
    {
        CopyTargetMemberAttributes(cw, "method");
        MemberPrefix(cw, ms.ReturnType.FullyQualifiedName(), ms.Name);
        AppendTypeParamList(cw, ms.TypeParameters);
        ParameterList(cw, ms.Parameters, RenderParameter, "(", ")");
        cw.Append(" => ");
        cw.Append(methodPrefix);
        cw.Append(".");
        cw.Append(ms.Name);
        AppendTypeParamList(cw, ms.TypeParameters);
        ParameterList(cw, ms.Parameters, RenderArgument, "(", ")");
        cw.AppendLine(";");
    }

    private void RenderArgument(CodeWriter cw, IParameterSymbol i)
    {
        cw.Append(HandleRefKind(i.RefKind));
        cw.Append(i.Name);
    }

    private void RenderParameter(CodeWriter cw, IParameterSymbol i)
    {
        new AttributeCopier(cw,"", AttributeWriterStrategy.MultiplePerLine)
            .CopyAttributesFrom(i.DeclaringSyntaxReferences);
        cw.Append(HandleRefKind(i.RefKind));
        cw.Append(i.Type.FullyQualifiedName());
        cw.Append(" ");
        cw.Append(i.Name);
        if (i.HasExplicitDefaultValue)
        {
            cw.Append(" = ");
            TryRenderEnumCast(cw, i.Type);
            cw.Append(ExplicitValue(i.ExplicitDefaultValue?.ToString()));
        }
    }

    private void TryRenderEnumCast(CodeWriter cw, ITypeSymbol type)
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
        CodeWriter cw, ImmutableArray<IParameterSymbol> parameters,
        Action<CodeWriter, IParameterSymbol> display, string open, string close)
    {
        cw.Append(open);
        AppendArgumentList(cw, parameters, display);
        cw.Append(close);
    }

    // we don't generate the component methods of properties, events, or indexers, because
    // we generate those using higher level constructs.
    private bool IsSpecialExcludedMethod(IMethodSymbol ms) => !ms.CanBeReferencedByName;

    private void AppendArgumentList(CodeWriter cw, ImmutableArray<IParameterSymbol> parameters,
        Action<CodeWriter, IParameterSymbol> paramPrinter)
    {
        if (parameters.Length == 0) return;
        paramPrinter(cw, parameters[0]);
        foreach (var parameter in parameters.Skip(1))
        {
            cw.Append(", ");
            paramPrinter(cw, parameter);
        }
    }

    private void AppendTypeParamList(
        CodeWriter cw, ImmutableArray<ITypeParameterSymbol> parameters)
    {
        if (parameters.Length == 0) return;
        cw.Append("<");
        cw.Append(string.Join(",", parameters.Select(i => i.Name)));
        cw.Append(">");
    }

    private void MemberPrefix(CodeWriter cw, string typeName, string memberName)
    {
        cw.Append(MemberDeclarationPrefix());
        cw.Append(typeName);
        cw.Append(" ");
        cw.Append(MemberNamePrefix());
        cw.Append(memberName);
    }

    private string setMethodKeyword(IMethodSymbol ps) => ps.IsInitOnly ? "init" : "set";
}