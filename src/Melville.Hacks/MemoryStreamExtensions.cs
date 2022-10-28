using System;
using System.IO;

namespace Melville.Hacks;

public static class MemoryStreamExtensions
{
    public static Span<byte> AsSpan(this MemoryStream ms) =>
        ms.GetBuffer().AsSpan()[..(int)ms.Length];
}