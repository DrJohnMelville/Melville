using System.IO;
using System.Threading.Tasks;

namespace Melville.FileSystem
{
  public sealed class AsyncWriterBuffer 
  {
    private MemoryStream buffer = new MemoryStream();
    public Stream Buffer => buffer;

    public async Task WriteAsync(Stream s)
    {
      buffer.Seek(0, SeekOrigin.Begin);
      await buffer.CopyToAsync(s);
      buffer.Dispose();
    }
  }
}