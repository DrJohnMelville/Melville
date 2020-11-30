using System.Collections.Generic;

namespace Melville.Generators.Tools.CodeWriters
{
    public class GeneratedFileUniqueNamer
    {
        private Dictionary<string, int> seenItems = new();

        private int IndexForName(string name)
        {
            int ret;
            if (!seenItems.TryGetValue(name, out ret)) ret = 0;
            seenItems[name] = ret + 1;
            return ret;
        }

        private string Suffix(int ordinal) => ordinal == 0 ? "" : $".{ordinal}";

        public string CreateFileName(string className) => 
            $"{className}.Generated{Suffix(IndexForName(className))}.cs";
    }
}