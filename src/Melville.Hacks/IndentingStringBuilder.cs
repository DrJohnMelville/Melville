using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using Melville.INPC;

namespace Melville.Hacks;

[MacroItem("string?")]
[MacroItem("sbyte")]
[MacroItem("byte")]
[MacroItem("short")]
[MacroItem("int")]
[MacroItem("long")]
[MacroItem("float")]
[MacroItem("double")]
[MacroItem("decimal")]
[MacroItem("ushort")]
[MacroItem("uint")]
[MacroItem("ulong")]
[MacroItem("object?")]
[MacroItem("ReadOnlySpan<char>")]
[MacroCode("public IndentingStringBuilder Append(~0~ value) => ReturnThis(AppendTarget().Append(value));")]
[MacroCode("""
        public IndentingStringBuilder AppendLine(~0~ value) 
        {
            AppendTarget().Append(value); 
            AddNewLine();
            return this;
        }
        """)]
[MacroCode("public IndentingStringBuilder Insert(int index, ~0~ value) => ReturnThis(AppendTarget().Insert(index, value));")]
public partial class IndentingStringBuilder
{
    [FromConstructor] private readonly StringBuilder target;
    [FromConstructor] private readonly string indentString;
    private int indentSize;
    private bool needsIndent;

    public IndentingStringBuilder(string? indent = null) : this(new StringBuilder(), indent ??"    ")
    {
    }

    private StringBuilder AppendTarget()
    {
        if (needsIndent) AddIndent();
        return target;
    }

    private void AddIndent()
    {
        for (int i = 0; i < indentSize; i++)
        {
            target.Append(indentString);
        }

        needsIndent = false;
    }

    public override string ToString() => target.ToString();

    public void AddNewLine()
    {
        target.Append(Environment.NewLine);
        needsIndent = true;
    }

    private IndentingStringBuilder ReturnThis<T>(T ignore) => this;

    public IndentingStringBuilder BeginIndent() => ReturnThis(indentSize++);
    public IndentingStringBuilder EndIndent() => ReturnThis(indentSize--);

    public BlockClosingStruct OpenBlock()
    {
        BeginIndent();
        return new(this);
    }

    public partial struct BlockClosingStruct : IDisposable
    {
        [FromConstructor] private readonly IndentingStringBuilder target;
        public void Dispose() => target.EndIndent();
    }

#if NET5_0_OR_GREATER
    [EditorBrowsable(EditorBrowsableState.Never)]
    [InterpolatedStringHandler]
    public partial struct ForwardingInterpolatedStringHandler
    {
        [DelegateTo()]
        private StringBuilder.AppendInterpolatedStringHandler handler;

        public ForwardingInterpolatedStringHandler(
            int literalChars, int formatHoles, IndentingStringBuilder parent)
        {
            handler = new StringBuilder.AppendInterpolatedStringHandler(literalChars, formatHoles,
                parent.AppendTarget());
        }
    }

    public IndentingStringBuilder Append(
        [InterpolatedStringHandlerArgument("")] ForwardingInterpolatedStringHandler handler) => this;
    
    public IndentingStringBuilder AppendLine(
        [InterpolatedStringHandlerArgument("")] ForwardingInterpolatedStringHandler handler)
    {
        AddNewLine();
        return this;
    }
#endif
} 