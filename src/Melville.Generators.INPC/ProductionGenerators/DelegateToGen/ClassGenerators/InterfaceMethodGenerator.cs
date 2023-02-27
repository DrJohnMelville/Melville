using System.Collections.Generic;
using System.Linq;
using Melville.Generators.INPC.GenerationTools.AstUtilities;
using Melville.Generators.INPC.ProductionGenerators.DelegateToGen.OurputWrapping;
using Microsoft.CodeAnalysis;

namespace Melville.Generators.INPC.ProductionGenerators.DelegateToGen.ClassGenerators;

public class InterfaceMethodGenerator : ClassGenerator
{
    public InterfaceMethodGenerator(DelegationOptions options) : base(options)  
    {
    }

    protected override IEnumerable<ISymbol> MembersThatCouldBeForwarded() =>
        TargetTypeAndParents().SelectMany(i => i.GetMembers());

    private IEnumerable<ITypeSymbol> TargetTypeAndParents() =>
        SourceType.AllInterfaces.Append(SourceType);

    protected override string MemberDeclarationPrefix(Accessibility sym) => 
        sym.AccessDeclaration() + " ";
}

public class ExplicitMethodGenerator : InterfaceMethodGenerator
{
    private readonly string namePrefix;

    public ExplicitMethodGenerator(DelegationOptions options) : base(options)
    {
        namePrefix = options.SourceType.FullyQualifiedName() + ".";
    }

    protected override string MemberDeclarationPrefix(Accessibility suggestedAccess) 
        => ""; // Explicit methods are inherently private
    protected override string MemberNamePrefix() => namePrefix;
}