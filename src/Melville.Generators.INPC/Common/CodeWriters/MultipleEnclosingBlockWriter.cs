using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis;

namespace Melville.Generators.INPC.Common.CodeWriters
{
    public static class MultipleEnclosingBlockWriter
    {
        public static IDisposable EnclosingBlockWriter<T>(this CodeWriter writer, SyntaxNode node,
            Action<CodeWriter, T> writeBlockHeader) where T : SyntaxNode =>
            writer.EnclosingBlockWriter(node, writeBlockHeader, writeBlockHeader);
        public static IDisposable EnclosingBlockWriter<T>(this CodeWriter writer, SyntaxNode node,
            Action<CodeWriter, T> writeAncestorBlock, Action<CodeWriter, T> writeCurrentBlock) where T : SyntaxNode
        {
            List<IDisposable> contexts = new();
            foreach (var ns in node.Ancestors().OfType<T>().Reverse())
            {
                writeAncestorBlock(writer,ns);
                contexts.Add(writer.CurlyBlock());
            }

            if (node is T currentBlock)
            {
                writeCurrentBlock(writer, currentBlock);
                contexts.Add(writer.CurlyBlock());
            }

            return CompositeDispose.DisposeInReverseOrder(contexts);
        }
    }
}