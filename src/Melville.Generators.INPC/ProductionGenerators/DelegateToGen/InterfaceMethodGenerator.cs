using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis;

namespace Melville.Generators.INPC.ProductionGenerators.DelegateToGen;

public class InterfaceMethodGenerator : DelegatedMethodGenerator
{
    public InterfaceMethodGenerator(
        ITypeSymbol targetType, string methodPrefix, ITypeSymbol parentSymbol, SyntaxNode writeNextTo) : 
        base(targetType, methodPrefix, parentSymbol, writeNextTo)
    {
    }

    protected override IEnumerable<ISymbol> MembersThatCouldBeForwarded() => 
        TargetTypeAndParents().SelectMany(i => i.GetMembers());

    private IEnumerable<ITypeSymbol> TargetTypeAndParents() => 
        TargetType.AllInterfaces.Append(TargetType);

    protected override string MemberDeclarationPrefix() => "public ";
        
    protected override bool ImplementationMissing(ISymbol i) => 
        parentSymbol.FindImplementationForInterfaceMember(i) == null;

}

public class ExplicitMethodGenerator : InterfaceMethodGenerator
{
    private readonly string namePrefix;

    public ExplicitMethodGenerator(
        ITypeSymbol targetType, string methodPrefix, string namePrefix, ITypeSymbol parentSymbol, SyntaxNode location) 
        : base(targetType, methodPrefix, parentSymbol, location)
    {
        this.namePrefix = namePrefix;
    }

    protected override string MemberDeclarationPrefix() => "";
    protected override string MemberNamePrefix() => namePrefix;
}