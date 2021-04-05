using System.Linq;
using Melville.Linq.Statistics.FileWriter;
using Melville.Linq.Statistics.Functional;
using Melville.Linq.Statistics.Tables;
using Test.TestStringDB;
using Xunit;

namespace Test.DescriptiveStats
{

  public sealed class TableTestData : StringTestDatabase
  {
    public TableTestData() : base()
    {
      HtmlTable.DetectCtrlDown = () => false;
    }
  }
  public sealed class TableTest: IClassFixture<TableTestData>
  {
    private readonly TableTestData data;

    public TableTest(TableTestData data)
    {
      this.data = data;
    }

    [Fact]
    public void RowTest()
    {
      var table = Enumerable.Range(1, 10).Table()
        .WithRows(i => i % 2 == 0)
        .WithColumns(i => i % 3 == 0);

      Assert.Equal(new [] {1,3,5,7,9}, table.RowValues(0));
      Assert.Equal(new [] {2,4,6,8,10}, table.RowValues(1));
    }
    [Fact]
    public void RowPercentageTest()
    {
      var table = Enumerable.Range(1, 10).Table()
        .WithRows(i => i % 2 == 0)
        .WithColumns(i => i % 3 == 0)
        .WithCellFunction(CellFunctions.RowPercentage);

      data.AssertDatabase(table.ToDump().ToString());

    }
    [Fact]
    public void ColumnPercentageTest()
    {
      var table = Enumerable.Range(1, 10).Table()
        .WithRows(i => i % 2 == 0)
        .WithColumns(i => i % 3 == 0)
        .WithCellFunction(CellFunctions.ColumnPercentage);

      data.AssertDatabase(table.ToDump().ToString());

    }
    [Fact]
    public void TablePercentageTest()
    {
      var table = Enumerable.Range(1, 10).Table()
        .WithRows(i => i % 2 == 0)
        .WithColumns(i => i % 3 == 0)
        .WithCellFunction(CellFunctions.TablePercentage);

      data.AssertDatabase(table.ToDump().ToString());

    }
    [Fact]
    public void ColumnTest()
    {
      var table = Enumerable.Range(1, 10).Table()
        .WithRows(i => i % 2 == 0)
        .WithColumns(i => i % 3 == 0);

      Assert.Equal(new [] {1,2,4,5,7,8,10}, table.ColumnValues(0));
      Assert.Equal(new [] {3,6,9}, table.ColumnValues(1));
    }
    [Fact]
    public void CellsTest()
    {
      var table = Enumerable.Range(1, 10).Table()
        .WithRows(i => i % 2 == 0)
        .WithColumns(i => i % 3 == 0);

      Assert.Equal(new [] {1,5,7}, table.CellValues(0,0));
      Assert.Equal(new [] {6}, table.CellValues(1,1));
    }
    [Fact]
    public void GetRows()
    {
     var table = 
       (TableImplementation<int, TableFactory.ValueBox<int>>)
       Enumerable.Range(1, 10).Table()
        .WithRows("Divisable by 2",i => i % 2 == 0)
        .WithColumns("Divisable By 3", i => i % 3 == 0);

      var rows = table.GridRows().AsList();
      Assert.Equal(2, rows.Count());
      for (int i = 0; i < 5; i++)
      {
        Assert.Equal((2*i)+1, rows[0].Elements.Skip(i).First().Value);
        Assert.Equal((2*i)+2, rows[1].Elements.Skip(i).First().Value);        
      }
      data.AssertDatabase(table.ToDump().ToString());
    }
    
    [Fact]
    public void GetMultiRow()
    {
     var table = (TableImplementation<int, TableFactory.ValueBox<int>>)
         Enumerable.Range(1, 10).Table()
        .WithRows("Divisable by 2",i => i % 2 == 0)
        .WithRows("Divisable By 3", i => i % 3 == 0);

      var columns = table.GridRows().AsList();
      Assert.Equal(4, columns.Count());

      data.AssertDatabase(table.ToDump().ToString());
    }

    [Fact]
    public void ExplicitRowsAndColumns()
    {
     var table = 
       (TableImplementation<int, TableFactory.ValueBox<int>>)
       Enumerable.Range(1, 20).Table().
         WithExplicitRow("Rows", i => i % 7 == 0, ie => ie % 10 == 0)
         .WithExplicitColumn("Divisible", i => i % 2 == 0, i => i % 3 == 0, i => i % 5 == 0);

      data.AssertDatabase(table.ToDump().ToString());
    }

    [Fact]
    public void GetRowHeight()
    {
      var table = 
        (TableImplementation<int, TableFactory.ValueBox<int>>)
        Enumerable.Range(1, 10).Table()
          .WithRows("Divisable by 2",i => i % 2 == 0)
          .WithColumns("Divisable By 3", i => i % 3 == 0);
      
      Assert.Equal(1, table.RowHeaders.DimensionLength(0));
      table.ToDump();
    }

    [Theory]
    [InlineData(-1, 30)]
    [InlineData(0, 15)]
    [InlineData(1, 5)]
    [InlineData(2, 1)]
    public void DimensionLengthAndCopies(int row, int dimension)
        {
      var table =
        (TableImplementation<int, TableFactory.ValueBox<int>>) 
        Enumerable.Range(1, 100)
        .Table()
        .WithRows(i => i % 2)
        .WithRows(i => i % 3)
        .WithRows(i => i % 5);
      Assert.Equal(dimension, table.RowHeaders.DimensionLength(row));
    }

    [Fact]
    public void NoRowsTable()
    {
      data.AssertDatabase(
        Enumerable.Range(1, 100).Table()
        .WithColumns(i => i % 10)
        .ToDump().ToString());
    }
    
    [Fact]
    public void NoColumnTable()
    {
      data.AssertDatabase(
        Enumerable.Range(1, 100).Table()
          .WithRows(i => i % 10)
          .ToDump().ToString());
    }
    [Fact]
    public void MultiRow1Col()
    {
      data.AssertDatabase(
        Enumerable.Range(1, 10).Table()
          .WithRows("Divisable by 2", i => i % 2 == 0)
          .WithRows("Divisable By 3", i => i % 3 == 0)
          .WithColumns("Mod 5", i => i % 5)
          .ToDump().ToString());
    }
    [Fact]
    public void NullItem()
    {
      data.AssertDatabase(
        Enumerable.Range(1, 10).Table()
          .WithRows("Divisable by 2", i => i % 2 == 0)
          .WithRows("Divisable By 3", i => i % 3 == 0 ? null : (i % 3).ToString())
          .WithColumns("Mod 5", i => i % 5 == 2 ? null : (i % 5).ToString())
          .ToDump().ToString());
    }
    [Fact]
    public void MultiCol1Row()
    {
      data.AssertDatabase(
        Enumerable.Range(1, 10).Table()
          .WithRows("Divisable by 2", i => i % 2 == 0)
          .WithColumns("Divisable By 3", i => i % 3 == 0)
          .WithColumns("Mod 5", i => i % 5)
          .ToDump().ToString());
    }
    [Fact]
    public void MultiColMultiRow()
    {
      data.AssertDatabase(
        Enumerable.Range(1, 100).Table()
          .WithRows("Divisable by 2", i => i % 2 == 0)
          .WithRows("Divisable By 3", i => i % 3 == 0)
          .WithColumns("Mod 5", i => i % 5)
          .WithColumns("Mod 7", i => i % 7)
          .ToDump().ToString());
    }
  }
} 