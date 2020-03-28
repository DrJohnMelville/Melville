using System.Collections.Generic;
using LINQPad.Extensibility.DataContext;
using Melville.ExcelDataContext.FileReader;

namespace Melville.ExcelDataContext
{
    public static class ExcelFileCache
    {
        private static readonly Dictionary<IConnectionInfo, RawFileReader> Readers =
            new Dictionary<IConnectionInfo, RawFileReader>();

        public static RawFileReader LoadFile(IConnectionInfo info)
        {
            if (Readers.TryGetValue(info, out var file))
            {
                return file;
            }
            var newFile = RawFileReader.LoadFile(new ExcelFileProperties(info).FilePath);
            Readers[info] = newFile;
            return newFile;
        }
    }
}