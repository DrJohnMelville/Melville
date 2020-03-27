using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Melville.Linq.Statistics.FileWriter;
using Xunit;

namespace Test.FileWriter
{
  public sealed class ExcelFileWriterTest
  {
    private MemoryStream output;

    [Fact]
    public void WriteSimpleFile()
    {
      output = new MemoryStream();
      var pack = new ExcelFileWriter(output);
      pack.AddPage("FooBar", new List<IList>
      {
        new List<object> {"1", "2", "3"},
        new List<object> { 1, 2, 3}
      });
      pack.Save();
      Assert.Equal(4096, output.GetBuffer().Length);
    }
  }

  public sealed class CsvFileWriterTest
  {
    [Fact]
    public void SimpleWrite()
    {
      var ms = new MemoryStream();
      CsvFileWriter.Write(ms, new List<IList>
      {     
        new List<object> {"1", "2", "3"},
        new List<object> { 1, 2, 3}
      });
      var str = Encoding.UTF8.GetString(ms.GetBuffer());
      Assert.Equal("1,2,3\r\n1,2,3\r\n", str.Trim('\0'));      
    }

    [Fact]
    public void NullWrite()
    {
      var ms = new MemoryStream();
      CsvFileWriter.Write(ms, new List<IList>
      {
        new List<object> {"1", null, "3"},
        new List<object> { 1, 2, 3}
      });
      var str = Encoding.UTF8.GetString(ms.GetBuffer());
      Assert.Equal("1,,3\r\n1,2,3\r\n", str.Trim('\0'));      
    }

    [Fact]
    public void SpecialChars()
    {
      var ms = new MemoryStream();
      CsvFileWriter.Write(ms, new List<IList>
      {
        new List<object> {"a,b", "a\"b", "a\r\nb"},
        new List<object> { 1, 2, 3}
      });
      var str = Encoding.UTF8.GetString(ms.GetBuffer());
      Assert.Equal("\"a,b\",\"a\"\"b\",\"a\r\nb\"\r\n1,2,3\r\n", str.Trim('\0'));      
    }

    [Fact]
    public void Object()
    {
      var ms = new MemoryStream();
      CsvFileWriter.Write(ms, new []
      {
        new {Int = 1, Double = 2.01}
      });
      var str = Encoding.UTF8.GetString(ms.GetBuffer());
      Assert.Equal("Int,Double\r\n1,2.01\r\n", str.Trim('\0'));      
    }
  }
}