using System;
using System.IO;
using System.Threading.Tasks;
using Melville.FileSystem;

namespace WebDashboard.SecretManager.Views
{
    public class PreserveFile: IAsyncDisposable
    {
        private readonly IFile file;
        private MemoryStream? stored;
        public PreserveFile(IFile file)
        {
            this.file = file;
        }

        public async Task Initialize()
        {
            if (file.Exists())
            {
                stored = new MemoryStream(2+(int)file.Size);
                using (var input = await file.OpenRead())
                {
                    await input.CopyToAsync(stored);
                }
            }
        }

        public async ValueTask DisposeAsync()
        {
            if (stored == null) return;
            stored.Seek(0, SeekOrigin.Begin);
            using (var output = await file.CreateWrite())
            {
                await stored.CopyToAsync(output);
            }
        }
    }
}