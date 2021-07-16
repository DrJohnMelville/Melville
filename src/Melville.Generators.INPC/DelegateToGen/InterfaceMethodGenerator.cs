using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using Melville.Generators.INPC.AstUtilities;
using Melville.Generators.INPC.CodeWriters;
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

}