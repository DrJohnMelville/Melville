using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using Melville.Hacks;

namespace Melville.FileSystem
{
  public static class FileOperations
  {
    public static string NameWithoutExtension(this IFileSystemObject fso) => 
      Regex.Match(fso.Name, @"(.*?)(\.[^\.]*)?$").Groups[1].Value;

    public static string StripInvalidCharsFromPath(string defaultFileName) => 
      Regex.Replace(defaultFileName, "[\\\\/:*?\"<>|]", "");

    #region Read and write Text Files
    public static async Task<string> ReadAsStringAsync(this IFile file)
    {
      using (var stream = await file.OpenRead())
      {
        return await ReadAsStringAsync(stream);
      }
    }
    public static async Task<string> ReadAsStringAsync(this Stream stream)
    {
      var buf = new byte[stream.Length];
      await buf.FillBufferAsync(0, (int)stream.Length, stream);
      return Encoding.UTF8.GetString(buf);
    }
    #endregion
    #region Read and write Xml files
    public static async Task<XNode> ReadAsXmlAsync(this IFile file)
    {
      using (var stream = await file.OpenRead())
      {
        return await ReadAsXmlAsync(stream);
      }
    }
    public static async Task<XNode> ReadAsXmlAsync(this Stream stream)
    {
      using (var tempStream = CreateMirrorStream(stream))
      {
        await stream.CopyToAsync(tempStream);
        tempStream.Seek(0, SeekOrigin.Begin);
        var xmlReader = XmlReader.Create(tempStream);
        xmlReader.MoveToContent();
        return XNode.ReadFrom(xmlReader);

      }
    }

    private static MemoryStream CreateMirrorStream(Stream stream)
    {
      return stream.CanSeek ? new MemoryStream((int)stream.Length) : new MemoryStream();
    }

    public static async Task WriteXmlAsync(this IFile file, XElement element)
    {
      await using var output = await file.CreateWrite();
      await WriteXmlAsync(output, element);
    }

    public static async Task WriteXmlAsync(this Stream output, XElement element)
    {
      using var writer = CreateXmlWriter(output);
      await element.WriteToAsync(writer, CancellationToken.None);
    }

    public static XmlWriter CreateXmlWriter(Stream output) =>
      XmlWriter.Create(output,
        new XmlWriterSettings
        {
          CheckCharacters = true,
          CloseOutput = false,
          ConformanceLevel = ConformanceLevel.Document,
          Encoding = Encoding.UTF8,
          Indent = false,
          NewLineHandling = NewLineHandling.Replace
        });

    #endregion

    public static string Extension(this IFileSystemObject file) => Extension(file.Path);

    /// <summary>
    /// Verified 3/23/19 that this is necessary path.extension returns the leading dot.
    /// </summary>
    /// <param name="src">A path string</param>
    /// <returns>The extension, if any, without the leading dot.</returns>
    public static string Extension(string src)
    {
      return Path.GetExtension(src).TrimStart('.');
    }

    public static IFile SisterFile(this IFile source, string ext)
    {
      return source.Directory?.File(source.NameWithoutExtension() + ext) ??
        throw new InvalidDataException("File does not have a directory");
    }

    public static bool IsSamePath(this IFile a, IFile b) =>
      String.Equals(a?.Path, b?.Path, StringComparison.OrdinalIgnoreCase);

    public static string MakePageReferenceString(this IFile file, int offset)
    {
      return $"{file.Name}:{offset}";
    }
    
    #region Copy and move folders
    public static async  Task CopyFrom(this IDirectory destination, IDirectory source, CancellationToken? token = null)
    {
      CancellationToken token1 = token ?? CancellationToken.None;
      destination.Create();
      foreach (var subDir in source.AllSubDirectories())
      {
        await CopyFrom(destination.SubDirectory(subDir.Name), subDir, token1);
      }

      foreach (var file in source.AllFiles())
      {
        await CopyFrom(destination.File(file.Name), file, token1, source.Attributes);
      }
    }


    public static Task MoveFrom(this IDirectory destination, IDirectory source, CancellationToken? token = null)
    {
      // this is a performance optimization -- let the OS do the Moveing for us
      return destination.ValidFileSystemPath() && source.ValidFileSystemPath() ?
        FileSystemMove(destination, source) :
        MoveUsingStreams(destination, source, token ?? CancellationToken.None);
    }

    private static Task FileSystemMove(IDirectory destination, IDirectory source) =>
      Task.Run(()=>Directory.Move(source.Path, destination.Path));

    private static async Task MoveUsingStreams(IDirectory destination, IDirectory source, CancellationToken token)
    {
      destination.Create();
      foreach (var subDir in source.AllSubDirectories())
      {
        await MoveUsingStreams(destination.SubDirectory(subDir.Name), subDir, token);
      }

      foreach (var file in source.AllFiles())
      {
        await MoveUsingStreams(destination.File(file.Name), file, token, source.Attributes);
      }
      source.Delete();
    }

    #endregion

    #region Copy and move files
    //This code has an important performance optimization.  If both the source and the destination
    //are filesystem files then allow the file system to do what it does best.
    public static Task MoveFrom(this IFile destination, IFile source, CancellationToken token, FileAttributes attributes)
    {
      return destination.ValidFileSystemPath() && source.ValidFileSystemPath() ?
        MoveUsingFileSystem(destination, source, attributes) :
        MoveUsingStreams(destination, source, token, attributes);
    }
    private static async Task MoveUsingFileSystem(IFile destination, IFile source, FileAttributes attributes)
    {
      if (SameVolume(destination.Path, source.Path))
      {
        await Task.Run(() =>
        {
          // this is a guard clause for a bad bug I can't duplicate.  See issue # 79
          Debug.Assert(source.Exists());
          if (source.Exists())
          {
            File.Delete(destination.Path);
            File.Move(source.Path, destination.Path);
            if (attributes != FileAttributes.Normal)
            {
              File.SetAttributes(destination.Path, attributes);
            }
          }
        });
        return;
      }
      // else
      await FileSystemCopy(destination, source, attributes);
      source.Delete();
    }
    private static async Task MoveUsingStreams(IFile destination, IFile source, CancellationToken token,
      FileAttributes attributes)
    {
      // this is a guard clause for a bad bug I can't duplicate.  See issue # 79
      Debug.Assert(source.Exists());
      if (!source.Exists()) return;
      await CopyFrom(destination, source, token, attributes);
      source.Delete();
    }
    public static Task CopyFrom(this IFile destination, IFile source, CancellationToken token, FileAttributes attributes)
    {
      // this is a performance optimization -- let the OS do the copying for us
      return destination.ValidFileSystemPath() && source.ValidFileSystemPath() ?
        FileSystemCopy(destination, source, attributes) :
        CopyUsingStreams(destination, source, token, attributes);
    }
    private static Task FileSystemCopy(IFile destination, IFile source, FileAttributes attributes)
    {
      return Task.Run(() =>
      {
        File.Delete(destination.Path);
        File.Copy(source.Path, destination.Path);
        File.SetAttributes(destination.Path, attributes);
      });
    }
    private static async Task CopyUsingStreams(IFile destination, IFile source, CancellationToken token,
      FileAttributes attributes)
    {
      using (var src = await source.OpenRead())
      using (var dest = await destination.CreateWrite(attributes))
      {
        await src.CopyToAsync(dest, 10240, token);
      }
    }
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
      return mA.Groups.Cast<Group>().Zip(
        mB.Groups.Cast<Group>(),
        (i, j) => i.Value.Equals(j.Value, StringComparison.OrdinalIgnoreCase)
      ).Skip(1).All(i => i);
    }

    #endregion
  }
}