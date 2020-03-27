using System.Linq;
using Melville.Linq.Statistics.FileWriter;
using Melville.Linq.Statistics.Functional;
using Test.TestStringDB;
using Xunit;

namespace Test.FileWriter
{
  public sealed class HtmlTableTestData : StringTestDatabase
  {
    public HtmlTableTestData() : base()
    {
      HtmlTable.DetectCtrlDown = () => false;
    }
  }
  public sealed class HtmlWriterTest: IClassFixture<HtmlTableTestData>
  {
    private readonly HtmlTableTestData data;

    public HtmlWriterTest(HtmlTableTestData data)
    {
      this.data = data;
    }

    [Fact]
    public void WithDataRowTest()
    {
      data.AssertDatabase(new HtmlTable().WithDataRow<int>("Data",
        i=>i.Count.ToString(), Enumerable.Range(1,10).AsList(), Enumerable.Range(1,20).AsList())
        .ToDump().ToString());
    }

    [Fact]
    public void SimpleTable()
    {
      var actual = new HtmlTable()
        .WithRow("A","B").WithRow("C","D").ToDump().ToString();
      Assert.Equal(@"<LinqPad.Html>
  <table>
    <tr>
      <td>A</td>
      <td>B</td>
    </tr>
    <tr>
      <td>C</td>
      <td>D</td>
    </tr>
  </table>
</LinqPad.Html>", actual);
    }

    [Fact]
    public void SimpleNNonsquareTable()
    {
      HtmlTable.DetectCtrlDown = ()=>false;
      var actual = new HtmlTable()
        .WithRow("A","B").WithRow("C").ToDump().ToString();
      Assert.Equal(@"<LinqPad.Html>
  <table>
    <tr>
      <td>A</td>
      <td>B</td>
    </tr>
    <tr>
      <td>C</td>
      <td colspan=""1"" />
    </tr>
  </table>
</LinqPad.Html>", actual);
    }

    [Fact]
    public void BugCheck()
    {
      new HtmlTable()
        .WithRow(HtmlTable.TD("This is a summary row", ("colspan", 13)));
    }
  }
}