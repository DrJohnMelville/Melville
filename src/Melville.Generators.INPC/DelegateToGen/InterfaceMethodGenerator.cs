using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis;

namespace Melville.Generators.INPC.DelegateToGen;

public class InterfaceMethodGenerator : DelegatedMethodGenerator
{
    public InterfaceMethodGenerator(ITypeSymbol targetType, string methodPrefix) : base(targetType, methodPrefix)
    {
    }

    protected override IEnumerable<ISymbol> MembersThatCouldBeForwarded(ITypeSymbol parentClass) => 
        TargetTypeAndParents().SelectMany(i => i.GetMembers());

    private IEnumerable<ITypeSymbol> TargetTypeAndParents() => 
        TargetType.AllInterfaces.Cast<ITypeSymbol>().Append(TargetType);

    protected override string MemberDeclarationPrefix() => "public ";
        
    protected override bool ImplementationMissing(ITypeSymbol parentClass, ISymbol i) => 
        parentClass.FindImplementationForInterfaceMember(i) == null;

}

public class ExplicitMethodGenerator : InterfaceMethodGenerator
{
    private readonly string namePrefix;

    public ExplicitMethodGenerator(ITypeSymbol targetType, string methodPrefix, string namePrefix) : base(targetType, methodPrefix)
    {
        this.namePrefix = namePrefix;
    }

    protected override string MemberDeclarationPrefix() => "";
    protected override string MemberNamePrefix() => namePrefix;
}