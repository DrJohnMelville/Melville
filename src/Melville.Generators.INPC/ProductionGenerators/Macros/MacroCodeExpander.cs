using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics;
using System.Linq;
using System.Text.RegularExpressions;
using Melville.Generators.INPC.GenerationTools.AstUtilities;
using Melville.Generators.INPC.GenerationTools.CodeWriters;
using Microsoft.CodeAnalysis;

namespace Melville.Generators.INPC.ProductionGenerators.Macros;

public readonly record struct MacroCodeExpander(string Prefix, string Code, string PostFix)
{
    public void WriteCode(CodeWriter cw, List<ImmutableArray<TypedConstant>> replacements)
    {
        TryWriteNontrivialString(cw, Prefix);
        foreach (var replacement in replacements)
        {
            cw.AppendLine(ReplacedString(replacement));
        }
        TryWriteNontrivialString(cw, PostFix);
    }

    private void TryWriteNontrivialString(CodeWriter cw, string item)
    {
        if (!string.IsNullOrWhiteSpace(item)) cw.AppendLine(item);
    }

    private string ReplacedString(ImmutableArray<TypedConstant> replacements) =>
        RegexReplacer.Replace(Code, m =>
            ComputeExpansion(replacements, m));


    private static Regex RegexReplacer = new("""
        ~   #opening group
        (\d+) # capture number
        (?:\:         # format block
             ([ciabxCIABX]) #source transformation
             (?:  #item separator
                ([^`~]*)
                (?:`  # format string
                    ([^~]*)
                 )?
            )?
        )?~
        """, RegexOptions.Compiled | RegexOptions.IgnorePatternWhitespace);

    private static string ComputeExpansion(ImmutableArray<TypedConstant> replacements, Match m) => 
        ComputeExpansion(
            replacements[int.Parse(m.Groups[1].Value)].CodeString(),
            m.Groups[2].Value,
            m.Groups[3].Value,
            m.Groups[4].Value);

    private static string ComputeExpansion(string source, string dataSelector, string delimiter, string formatter)
    {
        if (dataSelector.Length == 0) return source;
        if (formatter.Length == 0) formatter = "{0}";
        return ByteTransformedStream(source, dataSelector, delimiter, formatter);
    }

    private static string ByteTransformedStream(string source, string dataSelector, string delimiter, string formatter)
    {
        return string.Join(delimiter, CreateData(source, dataSelector).Select(i => string.Format(formatter, (object)i)));
    }

    private static IEnumerable<object> CreateData(string source, string dataSelector)
    {
        return dataSelector[0] switch
        {
            'i' or 'I' => source.Select(i => (int)i).OfType<object>(),
            'c' or 'C' => source.Select(i => (char)i).OfType<object>(),
            'a' or 'A' => source.Select(i => (byte)i).OfType<object>(),
            'x' or 'X' => AsHexString(source),
            'b' or 'B' => AsBinaryString(source),
            _ => throw new InvalidOperationException("Invalid Macro data transformation character")
        };
    }

    private static IEnumerable<object> AsHexString(string source) =>
        AssembleBytesFromChunks(source, HexNibble, 4);
    private static IEnumerable<object> AsBinaryString(string source) =>
        AssembleBytesFromChunks(source, BinaryDigit, 1);

    private static IEnumerable<object> AssembleBytesFromChunks(IEnumerable<char> source, Func<char,int> digitSelector, int chunkBits)
    {
        Debug.Assert(8%chunkBits == 0);
        var topSetBit = 0;
        var value = 0;
        foreach (var character in source)
        {
            var digit = digitSelector(character);
            if (digit < 0) continue;

            topSetBit+= chunkBits;
            value <<= chunkBits;
            value |= digit;

            if (topSetBit < 8) continue;
            
            yield return (byte)value;
            value = 0;
            topSetBit = 0;
        }

        if (topSetBit > 0) yield return (byte)(value<< (8-topSetBit));
    }

    private static int HexNibble(char arg) => arg switch
    {
        >= '0' and <= '9' => arg - '0',
        >= 'A' and <= 'F' => 10 + arg - 'A',
        >= 'a' and <= 'f' => 10 + arg - 'a',
        _ => -1
    };

    private static int BinaryDigit(char arg) => arg switch
    {
        '0' => 0,
        '1' => 1,
        _ => -1
    };
}