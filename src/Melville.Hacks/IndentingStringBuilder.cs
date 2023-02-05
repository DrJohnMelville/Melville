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

    [DelegateTo(WrapWith = nameof(ReturnThis), Exclude = "Line$")]
    [DelegateTo(WrapWith = nameof(ReturnThisAndSetIndent), Filter = "Line$")]
    private StringBuilder AppendTarget()
    {
        if (needsIndent) AddIndent();
        return target;
    }

    //This one has to be forwarded manually because forwarder will not forward unsafe code
    public unsafe IndentingStringBuilder Append(char* value, int valueCount) => 
        ReturnThis(this.AppendTarget().Append(value, valueCount));

    // Do not autoforward because this should not add an indent if the last line is blank
    public override string ToString() => target.ToString();

    public IndentingStringBuilder ReturnThisAndSetIndent(StringBuilder sb)
    {
        needsIndent = true;
        return this;
    }
    public IndentingStringBuilder ReturnThis(StringBuilder sb) => this;

    private void AddIndent()
    {
        for (int i = 0; i < indentSize; i++)
        {
            target.Append(indentString);
        }
        needsIndent = false;
    }


    private IndentingStringBuilder WrapWithThis(int _) => this;
    public IndentingStringBuilder BeginIndent() => WrapWithThis(indentSize++);
    public IndentingStringBuilder EndIndent() => WrapWithThis(indentSize--);

    public BlockClosingStruct OpenBlock()
    {
        BeginIndent();
        return new(this);
    }

    public readonly partial struct BlockClosingStruct : IDisposable
    {
        [FromConstructor] private readonly IndentingStringBuilder target;
        public void Dispose() => target.EndIndent();
    }

#if NET5_0_OR_GREATER
    [EditorBrowsable(EditorBrowsableState.Never)]
    [InterpolatedStringHandler]
    public partial struct ForwardingInterpolatedStringHandler
    {
        [DelegateTo]
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
        target.Append(Environment.NewLine);
        needsIndent = true;
        return this;
    }

    private IndentingStringBuilder AppendLine(ref StringBuilder.AppendInterpolatedStringHandler handler) => 
        throw new NotSupportedException("I replaced this with the ones above, so prevent it from getting forwarded.");
    private IndentingStringBuilder Append(ref StringBuilder.AppendInterpolatedStringHandler handler) => 
        throw new NotSupportedException("I replaced this with the ones above, so prevent it from getting forwarded.");

#endif
}