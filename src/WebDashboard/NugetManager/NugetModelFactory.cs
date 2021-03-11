using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Melville.MVVM.FileSystem;

namespace WebDashboard.NugetManager
{
    public static class GetTagFromFileImpl
    {
        public static async Task<string?> GetUniqueTag(this IFile file, string tagname) => 
            ExtractTagFromString(tagname, await (await file.OpenRead()).ReadAsStringAsync());

        private static string? ExtractTagFromString(string tagname, string text) =>
            RegexForTag(tagname)
                .Matches(text)
                .OfType<Match>()
                .Select(i => i.Groups[1].Value)
                .SingleOrDefault();

        private static Regex RegexForTag(string tagname)
        {
            var escapedTag = Regex.Escape(tagname);
            return new Regex(@$"<{escapedTag}>\s*(.*\S)\s*</{escapedTag}>");
        }
    }
    public class NugetModelFactory
    {
        public async Task<NugetModel> Create(IFile buildPropsFile)
        {
            return new NugetModel(await buildPropsFile.GetUniqueTag("Version")??"No Version Found");
        }
    }
}