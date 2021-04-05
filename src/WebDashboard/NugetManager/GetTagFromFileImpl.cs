using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Melville.FileSystem.FileSystem;

namespace WebDashboard.NugetManager
{
    public static class GetTagFromFileImpl
    {
        public static async Task<string?> GetUniqueTag(this IFile file, string tagname) =>
            (await ExptractMultipleFromFile(file, RegexForTag(tagname)))
            .SingleOrDefault();

        private static async Task<IEnumerable<string>> ExptractMultipleFromFile(IFile file, Regex searchRegex) => 
            ExtractMultipleFromString(await (await file.OpenRead()).ReadAsStringAsync(), searchRegex);

        private static IEnumerable<string> ExtractMultipleFromString(string text, Regex searchRegex)
        {
            return searchRegex
                .Matches(text)
                .OfType<Match>()
                .Select(i => i.Groups[1].Value);
        }

        private static Regex RegexForTag(string tagname)
        {
            var escapedTag = Regex.Escape(tagname);
            return new Regex(@$"<{escapedTag}>\s*(.*\S)\s*</{escapedTag}>");
        }
    }
}