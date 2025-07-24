using System;
using System.Collections.Generic;
using System.Linq;

namespace Melville.Linq;

public static class BinaryPrinter
{
    public static string BinaryDump(this IEnumerable<byte> bytes)
    {
        return string.Join(Environment.NewLine, 
            bytes.BinaryFormat().Select((i,index)=>$"{index * 16:X8} {i}"));
    }
    public static IEnumerable<string> BinaryFormat(this IEnumerable<byte> bytes)
    {
        return bytes.Chunks(16).
            Select(chunk => $"{BinaryRep(chunk),-48}  {AsciiRep(chunk)}");
    }

    private static string AsciiRep(IEnumerable<byte> chunk) => string.Join("", AsciiRepEnum(chunk));

    private static IEnumerable<char> AsciiRepEnum(IEnumerable<byte> chunk) =>
        chunk
            .Select(Convert.ToChar)
            .Select(i => IsPrintable(i) ? i : '.');

    private static bool IsPrintable(char item) => (item >= 32) && (item <= 128);

    private static string BinaryRep(IEnumerable<byte> chunk) => 
        string.Join(" ", chunk.Select(i => i.ToString("X2")));
}