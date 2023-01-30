using System.Collections.Generic;
using System.Linq;
using Melville.Generators.INPC.GenerationTools.AstUtilities;
using Microsoft.CodeAnalysis;

namespace Melville.Generators.INPC.ProductionGenerators.DelegateToGen;

public class InterfaceMethodGenerator : DelegatedMethodGenerator
{
    public InterfaceMethodGenerator(
        ITypeSymbol targetType, string methodPrefix, ISymbol parentSymbol) : 
        base(targetType, methodPrefix, parentSymbol)
    {
    }

    protected override IEnumerable<ISymbol> MembersThatCouldBeForwarded() => 
        TargetTypeAndParents().SelectMany(i => i.GetMembers());

    private IEnumerable<ITypeSymbol> TargetTypeAndParents() => 
        GeneratedMethodSourceSymbol.AllInterfaces.Append(GeneratedMethodSourceSymbol);

    protected override string MemberDeclarationPrefix(ISymbol sym) => sym.AccessDeclaration()+" ";
        
    protected override bool ImplementationMissing(ISymbol i) =>
        GeneratedMethodHostSymbol.FindImplementationForInterfaceMember(i) == null;

}

public class ExplicitMethodGenerator : InterfaceMethodGenerator
{
    private readonly string namePrefix;

    public ExplicitMethodGenerator(
        ITypeSymbol targetType, string methodPrefix, string namePrefix, ISymbol parentSymbol) 
        : base(targetType, methodPrefix, parentSymbol)
    {
        this.namePrefix = namePrefix;
    }

    protected override string MemberDeclarationPrefix(ISymbol sym) => ""; // Explicit methods are inherently private
    protected override string MemberNamePrefix() => namePrefix;
}