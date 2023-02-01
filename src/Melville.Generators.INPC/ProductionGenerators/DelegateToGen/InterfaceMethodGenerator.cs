using System.Collections.Generic;
using System.Linq;
using Melville.Generators.INPC.GenerationTools.AstUtilities;
using Melville.Generators.INPC.ProductionGenerators.DelegateToGen.MethodMappings;
using Microsoft.CodeAnalysis;

namespace Melville.Generators.INPC.ProductionGenerators.DelegateToGen;

public class InterfaceMethodGenerator : DelegatedMethodGenerator
{
    public InterfaceMethodGenerator(
        ITypeSymbol targetType, string methodPrefix, ISymbol parentSymbol,
        IMethodWrappingStrategy wrappingStrategy) : 
        base(targetType, methodPrefix, parentSymbol, wrappingStrategy)
    {
    }

    protected override IEnumerable<ISymbol> MembersThatCouldBeForwarded() => 
        TargetTypeAndParents().SelectMany(i => i.GetMembers());

    private IEnumerable<ITypeSymbol> TargetTypeAndParents() => 
        GeneratedMethodSourceSymbol.AllInterfaces.Append(GeneratedMethodSourceSymbol);

    public override string MemberDeclarationPrefix(ISymbol sym) => sym.AccessDeclaration()+" ";
        
    protected override bool ImplementationMissing(ISymbol i) =>
        GeneratedMethodHostSymbol.FindImplementationForInterfaceMember(i) == null;

}

public class InterfaceMixinGenerator : InterfaceMethodGenerator
{
    public InterfaceMixinGenerator(
        ITypeSymbol targetType, string methodPrefix, ISymbol parentSymbol, IMethodWrappingStrategy wrappingStrategy) :
        base(targetType, methodPrefix, parentSymbol, wrappingStrategy)
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
        ITypeSymbol targetType, string methodPrefix, string namePrefix, ISymbol parentSymbol, IMethodWrappingStrategy wrappingStrategy) 
        : base(targetType, methodPrefix, parentSymbol, wrappingStrategy)
    {
        this.namePrefix = namePrefix;
    }

    public override string MemberDeclarationPrefix(ISymbol sym) => ""; // Explicit methods are inherently private
    public override string MemberNamePrefix() => namePrefix;
}