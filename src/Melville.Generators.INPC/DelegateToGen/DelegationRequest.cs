﻿using System.Collections.Generic;
using Melville.Generators.INPC.CodeWriters;
using Microsoft.CodeAnalysis;

namespace Melville.Generators.INPC.DelegateToGen
{
    public class DelegationRequest
    {
        private readonly INamedTypeSymbol parentClass;
        private readonly IReadOnlyList<IDelegatedMethodGenerator> targets;

        public DelegationRequest(INamedTypeSymbol parentClass, IReadOnlyList<IDelegatedMethodGenerator> targets)
        {
            this.parentClass = parentClass;
            this.targets = targets;
        }
        
        public void GenerateForwardingMethods(CodeWriter cw)
        {
            foreach (var target in targets)
            {
                target.GenerateForwardingMethods(parentClass, cw);
            }
        }
    }
}