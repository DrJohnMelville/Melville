using System.Collections.Generic;
using System.Linq;
using Melville.Generators.INPC.GenerationTools.AstUtilities;
using Melville.Generators.INPC.ProductionGenerators.DelegateToGen.OurputWrapping;
using Microsoft.CodeAnalysis;

namespace Melville.Generators.INPC.ProductionGenerators.DelegateToGen;

public class InterfaceMethodGenerator : ClassGenerator
{
    public InterfaceMethodGenerator(
        ITypeSymbol sourceType, string methodPrefix, ISymbol parentSymbol,
        IMethodWrappingStrategy wrappingStrategy) : 
        base(sourceType, methodPrefix, parentSymbol, wrappingStrategy)
    {
    }

    protected override IEnumerable<ISymbol> MembersThatCouldBeForwarded() => 
        TargetTypeAndParents().SelectMany(i => i.GetMembers());

    private IEnumerable<ITypeSymbol> TargetTypeAndParents() => 
        SourceType.AllInterfaces.Append(SourceType);

    protected override string MemberDeclarationPrefix(Accessibility sym) => sym.AccessDeclaration()+" ";
        
    protected override bool ImplementationMissing(ISymbol i) =>
        GeneratedMethodHostSymbol.FindImplementationForInterfaceMember(i) == null;

}

public class InterfaceMixinGenerator : InterfaceMethodGenerator
{
    public InterfaceMixinGenerator(
        ITypeSymbol sourceType, string methodPrefix, ISymbol parentSymbol, IMethodWrappingStrategy wrappingStrategy) :
        base(sourceType, methodPrefix, parentSymbol, wrappingStrategy)
    {
    }

    protected override bool ImplementationMissing(ISymbol i) =>
        !GeneratedMethodHostSymbol.HasSimilarSymbol(i);

    protected override bool HostDescendsFromSource => false;
}

public class ExplicitMethodGenerator : InterfaceMethodGenerator
{
    private readonly string namePrefix;

    public ExplicitMethodGenerator(
        ITypeSymbol sourceType, string methodPrefix, string namePrefix, ISymbol parentSymbol, IMethodWrappingStrategy wrappingStrategy) 
        : base(sourceType, methodPrefix, parentSymbol, wrappingStrategy)
    {
        this.namePrefix = namePrefix;
    }

    protected override string MemberDeclarationPrefix(Accessibility suggestedAccess) => ""; // Explicit methods are inherently private
    protected override string MemberNamePrefix() => namePrefix;
}