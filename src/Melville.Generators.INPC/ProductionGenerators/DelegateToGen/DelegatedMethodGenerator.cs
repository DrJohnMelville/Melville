using System;
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
    protected readonly ITypeSymbol GeneratedMethodSourceSymbol;
    public string MethodPrefix { get; }
    public ISymbol TargetHolderSymbol { get; }

    protected ITypeSymbol GeneratedMethodHostSymbol => TargetHolderSymbol.ContainingType;

    public abstract string MemberDeclarationPrefix(ISymbol replacedSymbol);
    public virtual string MemberNamePrefix() => "";
    protected abstract IEnumerable<ISymbol> MembersThatCouldBeForwarded();

    protected DelegatedMethodGenerator(
        ITypeSymbol targetType, string methodPrefix, ISymbol targetHolderSymbol)
    {
        this.GeneratedMethodSourceSymbol = targetType;
        this.MethodPrefix = methodPrefix;
        this.TargetHolderSymbol = targetHolderSymbol;
    }

    public string InheritFrom() => GeneratedMethodSourceSymbol.FullyQualifiedName();

    public void GenerateForwardingMethods(CodeWriter cw)
    {
        var writer = new SingleDelegatedMethodGenerator(cw, this);
        foreach (var member in MembersToForward())
        {
            writer.GenerateForwardingMember(member);
        }
    }

    private IEnumerable<ISymbol> MembersToForward() =>
        MembersThatCouldBeForwarded()
            .Where(i => ImplementationMissing(i));

    protected abstract bool ImplementationMissing(ISymbol i);

    protected bool CanSeeSymbol(ISymbol methodToGenerate) =>
        (methodToGenerate.DeclaredAccessibility, SourceAndHostInSameAssembly(), HostDescendsFromSource) is
        (Accessibility.Public, _, _) or
        (Accessibility.Internal, true, _) or
        (Accessibility.ProtectedAndInternal, true, true) or
        (Accessibility.ProtectedOrInternal, true, _) or
        (Accessibility.ProtectedOrInternal, _, true) or
        (Accessibility.Protected, _, true);

    protected virtual bool HostDescendsFromSource => true;
    private bool SourceAndHostInSameAssembly() =>
        SymbolEqualityComparer.Default.Equals(GeneratedMethodSourceSymbol.ContainingAssembly, GeneratedMethodHostSymbol.ContainingAssembly);

}