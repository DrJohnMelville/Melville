using System.Text.RegularExpressions;
using Microsoft.CodeAnalysis;
using Xunit;

namespace Melville.Generators.INPC.Test.UnitTests;

public readonly struct FileAsserts
{
    private readonly SyntaxTree source;

    public FileAsserts(SyntaxTree source)
    {
        this.source = source;
    }

    public string Text() => source.ToString();
    public void AssertContains(string snippet) => Assert.Contains(snippet, Text());
    public void AssertDoesNotContain(string snippet) => Assert.DoesNotContain(snippet, Text());
    public void AssertEqual(string content) => Assert.Equal(content, Text());
    public void AssertNotEqual(string content) => Assert.NotEqual(content, Text());
    public void AssertRegex(Regex regex) => Assert.Matches(regex, Text());
    public void AssertRegex(string regex) => Assert.Matches(regex, Text());
}