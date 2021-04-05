using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace Melville.FileSystem
{
  public static class StreamExtensions
  {
    public static void Write(this Stream stream, byte[] buffer) => stream.Write(buffer, 0, buffer.Length);
    public static Task WriteAsync(this Stream str, byte[] buffer) => str.WriteAsync(buffer, 0, buffer.Length);
    public static Task WriteStringAsync(this Stream str, string s, Encoding encoding) => 
      str.WriteAsync(encoding.GetBytes(s)).AsTask();

    public static int FillBuffer(this Stream stream, byte[] buffer, int offset, int maxLength)
    {
      int totalRead = 0;
      int localRead = int.MaxValue;
      while (maxLength > totalRead && localRead > 0)
      {
        localRead = stream.Read(buffer, offset + totalRead, maxLength - totalRead);
        totalRead += localRead;
      }
      return totalRead;
    }

    public static async Task<int> FillBufferAsync(this Stream s, byte[] buffer, int offset, int count)
    {
      int totalRead = 0;
      int localRead = int.MaxValue;
      while (count - totalRead > 0 && localRead > 0)
      {
        localRead = await s.ReadAsync(buffer, offset + totalRead, count - totalRead);
        totalRead += localRead;
      }
      return totalRead;
    }


    public static byte[] ReadToArray(this Stream stream)
    {
      if (!stream.CanSeek) return ReadFully(stream);
      var len = (int)stream.Length - stream.Position;
      var ret = new byte[len];
      stream.FillBuffer(ret, 0, (int)len);
      return ret;
    }

    public static byte[] ReadFully(Stream stream)
    {
      byte[] buffer = new byte[32768];
      using (MemoryStream ms = new MemoryStream())
      {
        while (true)
        {
          int read = stream.Read(buffer, 0, buffer.Length);
          if (read <= 0)
            return ms.ToArray();
          ms.Write(buffer, 0, read);
        }
      }
    }
  }

}