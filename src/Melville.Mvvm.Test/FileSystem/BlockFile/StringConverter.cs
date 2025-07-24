using System;
using System.Text.RegularExpressions;

namespace Melville.Mvvm.Test.FileSystem.BlockFile;

public static partial class StringConverter
{
    public static byte [] BytesFromHexString(this string hex) => 
        Convert.FromHexString(InvalidHexCharacterRegex().Replace(hex, ""));

    [GeneratedRegex("[^0-9a-fA-F]")]
    private static partial Regex InvalidHexCharacterRegex();
}