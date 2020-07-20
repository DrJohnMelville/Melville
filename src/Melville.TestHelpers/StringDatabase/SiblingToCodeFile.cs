using System.IO;
using System.Runtime.CompilerServices;
using System.Text;

namespace Melville.TestHelpers.StringDatabase
{
    public static class SiblingToCodeFile
    {
        public static string FullPath(string fileName, [CallerFilePath] string? sourceFile = null) => 
            Path.Join(Path.GetDirectoryName(sourceFile), fileName);

        public static Stream ReadableStream(string fileName, [CallerFilePath] string? sourceFile = null) =>
            File.Open(FullPath(fileName, sourceFile), FileMode.Open, FileAccess.Read);
        public static Stream ReadWriteStream(string fileName, [CallerFilePath] string? sourceFile = null) =>
            File.Open(FullPath(fileName, sourceFile), FileMode.OpenOrCreate, FileAccess.ReadWrite);
        public static string AsUtf8String(string fileName, [CallerFilePath] string? sourceFile = null) =>
            File.ReadAllText(FullPath(fileName, sourceFile), Encoding.UTF8);
        public static byte[] AsBytes(string fileName, [CallerFilePath] string? sourceFile = null) =>
            File.ReadAllBytes(FullPath(fileName, sourceFile));
    }
}