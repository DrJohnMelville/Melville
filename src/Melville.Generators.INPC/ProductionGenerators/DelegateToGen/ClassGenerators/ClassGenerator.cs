using System;
using System.Collections.Generic;
using System.Linq;
using Melville.Generators.INPC.GenerationTools.AstUtilities;
using Melville.Generators.INPC.GenerationTools.CodeWriters;
using Melville.Generators.INPC.ProductionGenerators.DelegateToGen.MemberGenerators;
using Melville.Generators.INPC.ProductionGenerators.DelegateToGen.OurputWrapping;
using Microsoft.CodeAnalysis;

namespace Melville.Generators.INPC.ProductionGenerators.DelegateToGen.ClassGenerators;

public interface IDelegatedMethodGenerator
{
    void GenerateForwardingMethods(CodeWriter cw);
}

public abstract class ClassGenerator : IDelegatedMethodGenerator
{
    public DelegationOptions Options { get; }
    protected ITypeSymbol SourceType => Options.SourceType;
    public string MethodPrefix => Options.MethodPrefix;
    public ISymbol HostSymbol => Options.HostSymbol;
    public IMethodWrappingStrategy WrappingStrategy => Options.WrappingStrategy;

    protected ClassGenerator(DelegationOptions options)
    {
        Options = options;
    }

    protected ITypeSymbol GeneratedMethodHostSymbol => Options.HostClass;

    protected abstract string MemberDeclarationPrefix(Accessibility suggestedAccess);
    protected virtual string MemberNamePrefix() => "";
    protected abstract IEnumerable<ISymbol> MembersThatCouldBeForwarded();


    public void GenerateForwardingMethods(CodeWriter cw)
    {
        foreach (var symbol in MembersToGenerate())
        {
            symbol.WriteSymbol(cw);
        }
    }

    private IEnumerable<IMemberGenerator> MembersToGenerate() =>
        MembersThatCouldBeForwarded()
            .Where(IsNotSpecialInternalMethod)
            .Select(CreateMemberGenerator)
            .Where(i => !Options.HostClass.GetMembers().Any(i.IsSuppressedBy));

    private IMemberGenerator CreateMemberGenerator(ISymbol member) => 
        (member, Options.Namer.ComputeNameFor(member.Name)) switch
    {
        (_, null) => FilteredMemberGenerator.Instance,
        (IPropertySymbol { IsIndexer: true } ps, _) => new IndexerGenerator(ps, this),
        (IPropertySymbol ps, var newName) => new PropertyGenerator(ps, this, newName),
        (IEventSymbol es, var newName) => new EventGenerator(es, this, newName),
        (IMethodSymbol ms, var newName) => new MethodGenerator(ms, this, newName),
        (ITypeSymbol tS, _) => FilteredMemberGenerator.Instance,
        _ => throw new InvalidOperationException($"Cannot forward member: {member}")
    };



    // we don't generate the component methods of properties, events, or indexers, because
    // we generate those using higher level constructs
    private bool IsNotSpecialInternalMethod(ISymbol i) => i.CanBeReferencedByName || IsIndexer(i);

    private static bool IsIndexer(ISymbol i) => i is IPropertySymbol { IsIndexer: true };
#warning redo this with attention to self references
    protected bool CanSeeSymbol(ISymbol methodToGenerate) =>
        Options.IsSelfGeneration() ||
        (methodToGenerate.DeclaredAccessibility, SourceAndHostInSameAssembly(), HostDescendsFromSource: SourceAndHostAreSameClass) is
        (Accessibility.Public, _, _) or
        (Accessibility.Internal, true, _) or
        (Accessibility.ProtectedAndInternal, true, true) or
        (Accessibility.ProtectedOrInternal, true, _) or
        (Accessibility.ProtectedOrInternal, _, true) or
        (Accessibility.Protected, _, true);

    protected  bool SourceAndHostAreSameClass => 
        SymbolEqualityComparer.Default.Equals(SourceType, Options.HostClass);

    private bool SourceAndHostInSameAssembly() =>
        SymbolEqualityComparer.Default.Equals(SourceType.ContainingAssembly,
            GeneratedMethodHostSymbol.ContainingAssembly);

    public void RenderPrefix(
        CodeWriter cw, Accessibility suggestedAccess, string virtualOverideNew, string eventElt, ITypeSymbol type, string name)
    {
        cw.Append(MemberDeclarationPrefix(Options.ComputeAccessibilityFor(suggestedAccess)));
        cw.Append(virtualOverideNew);
        cw.Append(eventElt);
        cw.Append(type.FullyQualifiedName());
        cw.Append(" ");
        cw.Append(MemberNamePrefix());
        cw.Append(name);
    }
}

    