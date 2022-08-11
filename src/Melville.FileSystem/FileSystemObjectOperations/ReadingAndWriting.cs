using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using Melville.Hacks;

namespace Melville.FileSystem;

public static partial class FileOperations
{
      public static async Task<string> ReadAsStringAsync(this IFile file)
      {
        await using var stream = await file.OpenRead();
        return await ReadAsStringAsync(stream);
      }

      public static async Task<string> ReadAsStringAsync(this Stream stream)
      {
        var buf = new byte[stream.Length];
        await buf.FillBufferAsync(0, (int)stream.Length, stream);
        return Encoding.UTF8.GetString(buf);
      }
}