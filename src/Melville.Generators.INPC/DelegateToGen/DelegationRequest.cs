using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Melville.Generators.INPC.AstUtilities;
using Melville.Generators.INPC.CodeWriters;
using Microsoft.CodeAnalysis;

namespace Melville.Generators.INPC.DelegateToGen
{
    public class DelegationRequestMember
    {
        private readonly ITypeSymbol targetType;
        private readonly string methodPrefix;

        public DelegationRequestMember(ITypeSymbol targetType, string methodPrefix)
        {
            this.targetType = targetType;
            this.methodPrefix = methodPrefix;
        }

        public string InheritFrom() => targetType.FullyQualifiedName();

        public void GenerateForwardingMethods(ITypeSymbol parentClass, CodeWriter cw)
        {
            switch (targetType)
            {
                
            }
            // eventually use INamedTypeSymbol.FindImpementationForInterface to find already overriden members
            cw.AppendLine("// call Method using : " + methodPrefix);
        }
    }
    
    public class DelegationRequest
    {
        private readonly INamedTypeSymbol parentClass;
        private readonly IReadOnlyList<DelegationRequestMember> targets;

        public DelegationRequest(INamedTypeSymbol parentClass, IReadOnlyList<DelegationRequestMember> targets)
        {
            this.parentClass = parentClass;
            this.targets = targets;
        }

        public string ClassSuffix() => ": "+string.Join(", ", targets.Select(i=>i.InheritFrom()));
        public void GenerateForwardingMethods(CodeWriter cw)
        {
            foreach (var target in targets)
            {
                target.GenerateForwardingMethods(parentClass, cw);
            }
        }
    }
}