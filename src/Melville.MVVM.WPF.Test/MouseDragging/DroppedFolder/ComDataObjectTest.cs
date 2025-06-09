using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing.Printing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;
using System.Windows;
using FluentAssertions;
using Melville.MVVM.Wpf.MouseDragging.DroppedFiles;
using Xunit;

namespace Melville.MVVM.WPF.Test.MouseDragging.DroppedFolder;

public class ComDataObjectTest
{
    private readonly ComDataObject sut = new();

    [Fact]
    public void EmptyObjectHadNoDataPresent()
    {
        sut.GetDataPresent(typeof(string)).Should().BeFalse();
        sut.GetDataPresent(DataFormats.Text).Should().BeFalse();
        sut.GetDataPresent(DataFormats.Text, true).Should().BeFalse();

        sut.GetFormats().Should().BeEmpty();
        EnumerateComFormats().Should().BeEmpty();
    
        sut.GetData(DataFormats.UnicodeText).Should().BeNull();
        sut.GetData(DataFormats.UnicodeText, true).Should().BeNull();
        sut.GetData(typeof(string)).Should().BeNull();
    }

    public IEnumerable<string> EnumerateComFormats() =>
        sut.EnumFormatEtc(DATADIR.DATADIR_GET)
            .Wrap()
            .Select(i => DataFormats.GetDataFormat(i.cfFormat).Name);

    [Fact]
    public void PushOneFormat()
    {
        sut.SetData(DataFormats.UnicodeText, "Hello", true);

        sut.GetDataPresent(typeof(string)).Should().BeFalse();
        sut.GetDataPresent(DataFormats.Text).Should().BeFalse();
        sut.GetDataPresent(DataFormats.UnicodeText).Should().BeTrue();
        sut.GetDataPresent(DataFormats.UnicodeText, true).Should().BeTrue();

        sut.GetFormats(true).Should().BeEquivalentTo(DataFormats.UnicodeText);
        EnumerateComFormats().Should().BeEquivalentTo(DataFormats.UnicodeText);

        sut.GetData(DataFormats.UnicodeText, true).Should().Be("Hello");
    }

    [Theory]
    [InlineData("Text")]
    [InlineData("OEMText")]
    public void Push8BitStrings(string format)
    {
        sut.SetData(format, new MemoryStream("Hello"u8.ToArray()));
        ((string)sut.GetData(format)!).Should().Be("Hello");
    }

    [Fact]
    public void Push16BitString()
    {
        var data = MemoryMarshal.Cast<char, byte>("Hello".AsSpan()).ToArray();
        sut.SetData("UnicodeText", data);
        ((string)sut.GetData("UnicodeText")!).Should().Be("Hello");
    }

    private record Custom(int Value);
    [Fact]
    public void PushCustomType()
    {
        sut.SetData(new Custom(42));
        sut.GetDataPresent(typeof(Custom)).Should().BeTrue();
        EnumerateComFormats().Should().BeEmpty();

        ((Custom)sut.GetData(typeof(Custom))!).Value.Should().Be(42);
    }

    [Fact]
    public void ReturnBytesAsMemoryStreams()
    {
        // to be strangly compatible with the wpf implementation byte arrays
        // come back wrapped in memory streams.
        var bytes = new byte[] { 1, 2, 3, 4, 5 };
        sut.SetData("MyFormat", bytes);
        ((MemoryStream)sut.GetData("MyFormat")!).ToArray()
            .Should().BeEquivalentTo(bytes);
    }
}