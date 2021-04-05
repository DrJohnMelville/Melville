using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Xml.Linq;
using Accord.Statistics.Analysis;
using Accord.Statistics.Testing;
using Melville.Linq.Statistics.FileWriter;
using Melville.Linq.Statistics.Functional;
using Melville.Linq.Statistics.HypothesisTesting;

namespace Melville.Linq.Statistics.Tables
{
  public interface ITable<TItem>
  {
    // Create Rows And Columns
    ITable<TItem> WithRows<TKey>(Expression<Func<TItem, TKey>> selector);
    ITable<TItem> WithRows<TKey>(string name, Func<TItem, TKey> selector);
    ITable<TItem> WithColumns<TKey>(Expression<Func<TItem, TKey>> selector);
    ITable<TItem> WithColumns<TKey>(string name, Func<TItem, TKey> selector);
    ITable<TItem> WithExplicitColumn(string name, params Expression<Func<TItem, bool>>[] selectors);
    ITable<TItem> WithExplicitRow(string name, params Expression<Func<TItem, bool>>[] selectors);

    ITable<TItem> WithCellFunction(Func<IEnumerable<TItem>, IEnumerable<TItem>, IEnumerable<TItem>, object> func,
      SummaryFunctionSelection selection = SummaryFunctionSelection.All);
    ITable<TItem> WithFormatter<T>(Func<T, string> func);

    object ToDump();
    ChiSquaredStatisic ChiSquared();
    IEnumerable<TItem> RowValues(int i);
    IEnumerable<TItem> ColumnValues(int p0);
    IEnumerable<TItem> CellValues(int row, int col);
    IEnumerable<TItem> AllValues();
  }


  public abstract class TableImplementation<TItem, TStorage> : ITable<TItem> where TStorage : class
  {
    public object ToDump() => new TablePrinter(this).ToDump();
    public List<ITableAxis<TStorage>> RowHeaders { get; } = new List<ITableAxis<TStorage>>();
    public List<ITableAxis<TStorage>> ColumnHeaders { get; } = new List<ITableAxis<TStorage>>();
    private ObjectIdentitySet<TStorage> Items { get; }
    private Func<IEnumerable<TItem>, IEnumerable<TItem>, IEnumerable<TItem>, string> cellFunc;
    private Func<IEnumerable<TItem>, IEnumerable<TItem>, string> rowFunc;
    private Func<IEnumerable<TItem>,IEnumerable<TItem>, string> colFunc;
    private Func<IEnumerable<TItem>, object> allFunc;

    #region Constructor and item mapping

    protected abstract TItem FromStorage(TStorage item);

    public TableImplementation(ObjectIdentitySet<TStorage> items)
    { 
      Items = items;
      this.WithCellFunction(i => i.Count());
    }

    #endregion

    #region Row and Column Definitions

    public ITable<TItem> WithRows<TKey>(Expression<Func<TItem, TKey>> selector) => WithRows(ExpressionPrinter.Print(selector), selector.Compile());
    public ITable<TItem> WithRows<TKey>(string name, Func<TItem, TKey> selector)
    {
      RowHeaders.Add(CreateIdenttiyAxis(name, selector));
      return this;
    }

    private TableAxis<TStorage> CreateIdenttiyAxis<TKey>(string name, Func<TItem, TKey> selector)
    {
      return new TableAxis<TStorage>(name,
        Items.GroupBy(i => selector(FromStorage(i)))
          .OrderBy(i => i.Key)
          .Select(i => new TableRowOrColumn<TStorage>(i.Key?.ToString() ?? "<null>", i)));
    }

    public ITable<TItem> WithColumns<TKey>(Expression<Func<TItem, TKey>> selector) => 
      WithColumns(ExpressionPrinter.Print(selector), selector.Compile());

    public ITable<TItem> WithColumns<TKey>(string name, Func<TItem, TKey> selector)
    {
      ColumnHeaders.Add(CreateIdenttiyAxis(name, selector));
      return this;
    }

    #endregion

    #region Explicit Row and Column Definitions

    public ITable<TItem> WithExplicitColumn(string name, params Expression<Func<TItem, bool>>[] selectors)
    {
      ColumnHeaders.Add(CreateExplicitAxis(name,selectors));
      return this;
    }
    public ITable<TItem> WithExplicitRow(string name, params Expression<Func<TItem, bool>>[] selectors)
    {
      RowHeaders.Add(CreateExplicitAxis(name,selectors));
      return this;
    }

    private ITableAxis<TStorage> CreateExplicitAxis(string title, Expression<Func<TItem, bool>>[] selectors)
    {
      var items = selectors.Select(i => (Name: ExpressionPrinter.Print(i), Func:i.Compile()));
      return new TableAxis<TStorage>(title, 
        items.Select(j=>new TableRowOrColumn<TStorage>(j.Name, 
            Items.Where(k=>j.Func(FromStorage(k)))))
        );
    }

    #endregion

    #region Cell Mapping
    private readonly ObjectFormatter formatter = new ObjectFormatter();
    public ITable<TItem> WithFormatter<T>(Func<T, string> func)
    {
      formatter.AddFormatter(func);
      return this;
    }

    public ITable<TItem> WithCellFunction(Func<IEnumerable<TItem>, IEnumerable<TItem>, IEnumerable<TItem>, object> func,
      SummaryFunctionSelection selection)
    {
      if ((selection & SummaryFunctionSelection.Cell) != 0)
      {
        cellFunc = (row,col,cell)=> formatter.Format(func(row,col,cell));
      }
      if ((selection & SummaryFunctionSelection.Row) != 0)
      {
        if (func == null)
        {
          rowFunc = null;
        }
        else
        {
          rowFunc = (row, all) => formatter.Format(func(row, all, row));
        }
      }
      if ((selection & SummaryFunctionSelection.Column) != 0)
      {
        if (func == null)
        {
          colFunc = null;
        }
        else
        {
          colFunc = (col, all) =>formatter.Format(func(all, col, col));
        }
      }
      if ((selection & SummaryFunctionSelection.LowerRight) != 0)
      {
        if (func == null)
        {
          allFunc = null;
        }
        else
        {
          allFunc = (all)=>formatter.Format(func(all, all, all));
        }
      }
      return this;
    }

    #endregion

    #region Compute rows and columns

    public IList<GridRowOrColumn<TStorage>> GridRows() => GridOrColumnRows(RowHeaders);
    public IList<GridRowOrColumn<TStorage>> GridColumns() => GridOrColumnRows(ColumnHeaders);

    private IList<GridRowOrColumn<TStorage>> GridOrColumnRows(List<ITableAxis<TStorage>> rowOrColumnHeaders)
    {
      List<GridRowOrColumn<TStorage>> ret = new List<GridRowOrColumn<TStorage>>();
      Stack<ITableRowOrColumn<TStorage>> headers = new Stack<ITableRowOrColumn<TStorage>>();
      void AddRows(ObjectIdentitySet<TStorage> parent, int level)
      {
        if (level >= rowOrColumnHeaders.Count)
        {
          ret.Add(new GridRowOrColumn<TStorage>(parent, headers.Reverse()));
          return;
        }
        foreach (var row in rowOrColumnHeaders[level].Elements)
        {
          headers.Push(row);
          AddRows(new ObjectIdentitySet<TStorage>(parent.Intersect(row.Elements)), level + 1);
          headers.Pop();
        }
      }

      AddRows(Items, 0);
      return ret;
    }


    #endregion

    #region Explicit Rows and Columns

    public IEnumerable<TItem> ExtractItems(IEnumerable<TStorage> input) =>
      input.Select(FromStorage);
    public IEnumerable<TItem> RowValues(int i) => RowOrColumnValues(i, GridRows());
    public IEnumerable<TItem> ColumnValues(int i) => RowOrColumnValues(i, GridColumns());
    private IEnumerable<TItem> RowOrColumnValues(int i, IList<GridRowOrColumn<TStorage>> items) => 
      ExtractItems(items.ElementAt(i).Elements);
    public IEnumerable<TItem> CellValues(int row, int col) => 
      ExtractItems(GridRows().ElementAt(row).CellContents(GridColumns().ElementAt(col)));

    public IEnumerable<TItem> AllValues() => ExtractItems(Items);

    #endregion

    #region Table Printer


    private sealed class TablePrinter
    {
      private readonly TableImplementation<TItem, TStorage> table;
      private IList<ITableAxis<TStorage>> RowHeaders => table.RowHeaders;
      private IList<ITableAxis<TStorage>> ColumnHeaders => table.ColumnHeaders;


      public TablePrinter(TableImplementation<TItem, TStorage> table)
      {
        this.table = table;
      }
      public object ToDump()
      {
        var headderRows = ZipConcat(ColumnHeaderTitles(), ColumnHeaderGrid())
          .Concat(ExtraTitleRow());
        var bodyRows = GridRows();
        var allRows = TablePadder.PadColumns(AddExtraRowTitle(headderRows, bodyRows));
        return HtmlTable.EmitClipboardLink(new XElement("LinqPad.Html",
          new XElement("table",
            allRows.Select(i=>new XElement("tr", i))
          )
        ));
      }

      private IEnumerable<IEnumerable<XElement>> AddExtraRowTitle(
        IEnumerable<IEnumerable<XElement>> headderRows, IEnumerable<IEnumerable<XElement>> bodyRows)
      {
        if (RowHeaders.Count != 1)
        {
          return headderRows.Concat(bodyRows);
        }
        var headderList = headderRows.AsList();
        var bodyList = bodyRows.AsList();
        var bodyListCount = bodyList.Count - (HasFooterRow()?1:0);
        return PrependFirst(headderList, Elt("Td", "", ("rowspan", headderList.Count)))
          .Concat(PrependFirst(bodyList, Elt("th", RowHeaders[0].Name, ("rowspan", bodyListCount))));
      }

      public IEnumerable<IEnumerable<T>> PrependFirst<T>(IEnumerable<IEnumerable<T>> body,
        T elt)
      {
        var first = true;
        foreach (var line in body)
        {
          yield return first?line.Prepend(elt):line;
          first = false;
        }
      }

      private IEnumerable<IEnumerable<XElement>> ExtraTitleRow()
      {
        if (RowHeaders.Count < 2 || ColumnHeaders.Count < 2) return new XElement[0][];
        return new []{ RowHeaders.Select(i => Elt("th", i.Name))
          .Concat(Elt("td", "", ("colspan", ColumnHeaders.DimensionLength(-1))))};
      }

      public IEnumerable<IEnumerable<T>> ZipConcat<T>(IEnumerable<IEnumerable<T>> a,
        IEnumerable<IEnumerable<T>> b) => a.Zip(b, (i, j) => i.Concat(j));

      public IEnumerable<IEnumerable<XElement>> ColumnHeaderTitles()
      {
        if (ColumnHeaders.Count > 1)
        {
          return ColumnHeaderTitles2xn();
        }
        //else
        if (RowHeaders.Count <= 1)
        {
          return ColumnHeaderTitles1x1();
        }
        // 
        return ColumnHeadderTitles1xn();
      }

      private IEnumerable<IEnumerable<XElement>> ColumnHeadderTitles1xn()
      {
        return new[]
        {
          new[] {Elt("td", "", ("colspan", RowHeaders.Count))},
          RowHeaders.Select(i => Elt("th", i.Name))
        };
      }

      private IEnumerable<IEnumerable<XElement>> ColumnHeaderTitles1x1()
      {
        return new[]
        {
          new[] {Elt("td", "", ("colspan", RowHeaders.Count), ("rowspan", ColumnHeaders.Count + 1))},
          new XElement[0],
        };
      }

      private IEnumerable<IEnumerable<XElement>> ColumnHeaderTitles2xn()
      {
        return ColumnHeaders.Select(i => new[]
        {
          Elt("th", i.Name,
            ("colspan", RowHeaders.Count))
        });
      }

      public IEnumerable<IEnumerable<XElement>> ColumnHeaderGrid()
      {
        switch (ColumnHeaders.Count)
        {
          case 0:
            return new[] {new []{Elt("td","")},new[] {Elt("th", "Value")}};
          case 1:
            return new[]
            {
              new[]
              {
                Elt("th", ColumnHeaders[0].Name, ("style", "text-align:center"),
                  ("colspan", ColumnHeaders.DimensionLength(-1)))
              },
              ColumnHeaders[0].Elements.Select(i => Elt("th", i.Name))
            };
          default:
            return ColumnHeaders
              .Select(ElementList);
        }
      }

      private IEnumerable<XElement> ElementList(ITableAxis<TStorage> col, int line)
      {
        return col.Elements.Repeat(ColumnHeaders.DimensionCopies(line))
          .Select(i => Elt("th", i.Name, ("colspan", ColumnHeaders.DimensionLength(line))));
      }


      private IEnumerable<IEnumerable<XElement>> GridRows()
      {
        var cols = table.GridColumns().AsList();
        int rowNum = 0;
        foreach (var row in table.GridRows())
        {
          yield return SingleRowHeader(row).Concat(SingleRow(row, rowNum, cols));
          rowNum++;
        }
        if (HasFooterRow())
        {
          yield return FooterRow(cols);
        }
      }

      private IEnumerable<XElement> FooterRow(IList<GridRowOrColumn<TStorage>> cols)
      {
        return new[] {Elt("td", "", ("colspan", Math.Max(2, RowHeaders.Count)))}.Concat(
          table.colFunc == null
            ? new[] {Elt("td", "", ("colspan", cols.Count))}
            : cols.Select(i => Elt("td", 
              table.colFunc(i.Elements.Select(table.FromStorage), 
                table.Items.Select(table.FromStorage))))).Concat(FooterTotalCell());
      }

      private XElement FooterTotalCell()
      {
        return HasFinalSummaryCell() ? Elt("td", table.allFunc(table.Items.Select(table.FromStorage))) : 
          null;
      }

      private bool HasFinalSummaryCell()
      {
        return table.allFunc != null && HasRowSummary();
      }

      private bool HasFooterRow()
      {
        return table.RowHeaders.Count > 0 && (table.colFunc != null || table.allFunc != null);
      }

      private IEnumerable<XElement> SingleRowHeader(GridRowOrColumn<TStorage> row) => 
        row.Headers.Select(i => Elt("th", i.Name)).DefaultIfEmpty(Elt("th",""));

      private IEnumerable<XElement> SingleRow(GridRowOrColumn<TStorage> row, int rowNum, IList<GridRowOrColumn<TStorage>> cols)
      {
        var cells = cols.Select(i => new XElement("td",
          table.cellFunc == null?"<null func>":
          table.cellFunc(table.ExtractItems(row.Elements), 
            table.ExtractItems(i.Elements),
            table.ExtractItems(row.CellContents(i)))));
        if (HasRowSummary())
        {
          cells = cells.Concat(Elt("td", table.rowFunc(
            row.Elements.Select(table.FromStorage), table.Items.Select(table.FromStorage))));
        }
        return cells;
      }

      private bool HasRowSummary()
      {
        return table.rowFunc != null && table.ColumnHeaders.Count > 0;
      }

      private XElement Elt(string name, object content,
        params (string attr, object attrvalue)[] attributes)=>
        new XElement(name, content, attributes.Select(i=>new XAttribute(i.attr, i.attrvalue)));
    }


    #endregion

    #region Chi Square

    private GeneralConfusionMatrix GeneralizedConfusionMatrix()
    {
      var rows = GridRows().AsList();
      var cols = GridColumns().AsList();
      return new GeneralConfusionMatrix(
        rows.Select(r => cols.Select(c => r.Elements.Intersect(c.Elements).Count()).ToList() as IList<int>)
        .ToList().To2x2());
    }
    public ChiSquaredStatisic ChiSquared()
    {
      var mat = GeneralizedConfusionMatrix();
      //Notixce that we have to set the degrees of freedom ourselves and not to get it from the
      // generalizedconfusionmatrix.  GCM has a bug where it assumes that it is square.
      return new ChiSquaredStatisic(new ChiSquareTest(mat, false).Statistic, 
        (mat.Matrix.GetLength(0) - 1) * (mat.Matrix.GetLength(1) - 1));
    }

    #endregion
  }
}