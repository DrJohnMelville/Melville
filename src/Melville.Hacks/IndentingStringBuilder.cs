using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using Melville.INPC;

namespace Melville.Hacks;

public partial class IndentingStringBuilder
{
    [FromConstructor]
    private readonly StringBuilder target;
    [FromConstructor] private readonly string indentString;
    private int indentSize;
    private bool needsIndent;

    public IndentingStringBuilder(string? indent = null) : this(new StringBuilder(), indent ??"    ")
    {
    }
    
    [DelegateTo(WrapWith = "ReturnMe")]
    private StringBuilder AppendTarget()
    {
        if (needsIndent) AddIndent();
        return target;
    }

    //This one has to be forwarded manually because forwarder will not forward unsafe code
    public unsafe IndentingStringBuilder Append(char* value, int valueCount) => 
        ReturnMe(this.AppendTarget().Append(value, valueCount));


    public IndentingStringBuilder ReturnMe(StringBuilder sb) => this;

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
        [DelegateTo(false)]
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