using System.Security.Cryptography.X509Certificates;
using System.Text.RegularExpressions;
using FluentAssertions;
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
    public void AssertContains(string snippet) => Text().Should().Contain(snippet);
    public void AssertDoesNotContain(string snippet) => Text().Should().NotContain(snippet);
    public void AssertEqual(string content) => Text().Should().Be(content);
    public void AssertNotEqual(string content) => Text().Should().NotBe(content);
    public void AssertRegex(Regex regex) => Text().Should().MatchRegex(regex);
    public void AssertRegex(string regex) => Text().Should().MatchRegex(regex);

    public void AssertContainsIgnoreWhiteSpace(string snippet)
    {
        if (!StripWhitespace(Text()).Contains(StripWhitespace(snippet)))
            AssertContains(snippet);
    }

    private string StripWhitespace(string s) => WhitespaceDetector.Replace(s, "");
    private static Regex WhitespaceDetector = new Regex(@"\s", RegexOptions.Compiled);
}