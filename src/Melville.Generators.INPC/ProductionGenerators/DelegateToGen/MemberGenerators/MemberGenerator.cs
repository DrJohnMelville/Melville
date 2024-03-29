﻿using Microsoft.CodeAnalysis;
using System;
using System.Collections.Generic;
using System.Linq;
using Melville.Generators.INPC.GenerationTools.AstUtilities;
using Melville.Generators.INPC.GenerationTools.CodeWriters;
using Melville.Generators.INPC.GenerationTools.DocumentationCopiers;
using Melville.Generators.INPC.ProductionGenerators.DelegateToGen.ClassGenerators;

namespace Melville.Generators.INPC.ProductionGenerators.DelegateToGen.MemberGenerators
{
    public interface IMemberGenerator
    {
        bool IsSuppressedBy(ISymbol comparisonItem);
        void WriteSymbol(CodeWriter cw);
    }

    public abstract class MemberGenerator<T>: IMemberGenerator where T: ISymbol    {
        protected T SourceSymbol { get; }
        private readonly ClassGenerator host;
        private readonly string memberName;

        protected abstract ITypeSymbol ResultType { get; }
        protected abstract string CopiedAttributeName();
        protected virtual string EventElt()=>"";

        protected MemberGenerator(T sourceSymbol, ClassGenerator host, string memberName)
        {
            SourceSymbol = sourceSymbol;
            this.host = host;
            this.memberName = memberName;
        }

        public virtual bool IsSuppressedBy(ISymbol comparisonItem) => 
            comparisonItem.Name.Equals(memberName, StringComparison.Ordinal);

        public virtual void WriteSymbol(CodeWriter cw)
        {
            MergeTargedAndSourceXmlDocumentation(cw);
            CopySourceAttributes(cw);
            CopyApplicableHostAttributes(cw);
            host.RenderPrefix(cw, 
                SourceSymbol.DeclaredAccessibility, OverrideOrNew(), EventElt(), ResultType, memberName);
        }

        private void MergeTargedAndSourceXmlDocumentation(CodeWriter cw)
        {
            var copier = new DocumentationCopier(cw);
            copier.Copy(SourceSymbol, host.Options.DocumentationLibrary);
            copier.Copy(host.HostDocumentation);
        }

        private void CopySourceAttributes(CodeWriter cw) => 
            new AttributeCopier(cw, "").CopyAttributesFrom(SourceSymbol.DeclaringSyntaxReferences);

        private void CopyApplicableHostAttributes(CodeWriter cw) =>
            new AttributeCopier(cw, CopiedAttributeName()).CopyAttributesFrom(host.HostSymbol.DeclaringSyntaxReferences);

        protected void AppendHostSymbolAccess(CodeWriter cw) => cw.Append(host.MethodPrefix);


        private string OverrideOrNew()
        {
            var prior = GetBaseMembers().FirstOrDefault(IsSuppressedBy);
            return prior switch
            {
                { IsVirtual: true } or {IsAbstract: true} => "override ",
                null => "",
                _ => "new "
            };
        }

        private IEnumerable<ISymbol> GetBaseMembers() =>
            host.HostSymbol.ContainingType.AllBases().SelectMany(i => i.GetMembers());
    }
}
