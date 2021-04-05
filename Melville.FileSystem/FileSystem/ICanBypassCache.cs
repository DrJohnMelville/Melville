using System.IO;
using System.Threading.Tasks;

namespace Melville.FileSystem.FileSystem
{
  public interface ICanBypassCache
  {
    Task<Stream> OpenReadImmediate();
  }
  public static class ImmediateFileOperations
  {
    public static Task<Stream> OpenReadImmediate(this IFile file) =>
      (file is ICanBypassCache bc) ? bc.OpenReadImmediate() : file.OpenRead();
  }

}