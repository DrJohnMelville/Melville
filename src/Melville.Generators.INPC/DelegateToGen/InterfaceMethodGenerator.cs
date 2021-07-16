using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis;

namespace Melville.Generators.INPC.DelegateToGen
{

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
    }

    public class BaseClassMethodGenerator : DelegatedMethodGenerator
    {
        public BaseClassMethodGenerator(ITypeSymbol targetType, string methodPrefix) : base(targetType, methodPrefix)
        {
        }

        protected override string MemberDeclarationPrefix() => "public override ";

        protected override IEnumerable<ISymbol> MembersThatCouldBeForwarded(ITypeSymbol parentClass) => 
            TargetType.GetMembers().Where(i => i.IsVirtual || i.IsAbstract);
    }
}