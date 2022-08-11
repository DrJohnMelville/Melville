using System;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

// Namespace discordant from file system for backward compatibility
namespace Melville.FileSystem;


public static partial class FileOperations
{
    public static string NameWithoutExtension(this IFileSystemObject fso) => 
        Regex.Match(fso.Name, @"(.*?)(\.[^\.]*)?$").Groups[1].Value;

    public static string StripInvalidCharsFromPath(string defaultFileName) => 
        Regex.Replace(defaultFileName, "[\\\\/:*?\"<>|]", "");
    
    public static string Extension(this IFileSystemObject file) => Extension(file.Path);
    public static string Extension(string src) => Path.GetExtension(src).TrimStart('.');

    public static IFile SisterFile(this IFile source, string ext) =>
        source.Directory?.File(source.NameWithoutExtension() + ext) ??
        throw new InvalidDataException("File does not have a directory");

    public static bool IsSamePath(this IFile a, IFile b) =>
        String.Equals(a?.Path, b?.Path, StringComparison.OrdinalIgnoreCase);

    public static string MakePageReferenceString(this IFile file, int offset) => 
        $"{file.Name}:{offset}";
    
    public static readonly Regex SimplePath = new Regex(@"^(?:[\\/][\\/]\?[\\/])?([A-Za-z]):[\\/]");
    public static readonly Regex UncPath = new Regex(@"^(?:[\\/][\\/]UNC[\\/])?[\\/][\\/]([^\\/]+)[\\/]([^\\/]+)[\\/]");
    public static bool SameVolume(string pathA, string pathB)
    {
        var mA = SimplePath.Match(pathA);
        var mB = SimplePath.Match(pathB);

        if (!(mA.Success && mB.Success))
        {
            mA = UncPath.Match(pathA);
            mB = UncPath.Match(pathB);
            if (!(mA.Success && mB.Success))
                return false;
        }
        return mA.Groups.Zip(
            mB.Groups,
            (i, j) => i.Value.Equals(j.Value, StringComparison.OrdinalIgnoreCase)
        ).Skip(1).All(i => i);
    }

}