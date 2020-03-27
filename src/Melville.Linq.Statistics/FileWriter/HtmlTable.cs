using Melville.Linq.Statistics.Functional;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using System.Xml.Linq;
using LINQPad;
using Melville.Linq.Statistics.DescriptiveStats;
using Melville.Linq.Statistics.HypothesisTesting;
using Melville.Linq.Statistics.Tables;

namespace Melville.Linq.Statistics.FileWriter
{
    public class HtmlTable
  {
    private readonly List<IEnumerable> rows = new List<IEnumerable>();

    public HtmlTable WithRow(params object[] row)
    {
      if (row.Length == 1 && !(row[0] is string))
      {
        var enumerable = row[0] as IEnumerable;
        if (enumerable != null)
        {
          return WithRow(enumerable.OfType<object>().ToArray());
        }
      }
      rows.Add(row);
      return this;
    }

    public HtmlTable WithTitleRow(params object[] row) =>
      WithRow(row.Select(i => TH(i)).ToArray());

    public HtmlTable WithMultiRow(IEnumerable<IEnumerable> rows) =>
      rows.Select(i => WithRow(i.OfType<object>())).LastOrDefault();

    private readonly ObjectFormatter formatter = new ObjectFormatter();
    public HtmlTable WithFormatter<T>(Func<T, string> func)
    {
      formatter.AddFormatter(func);
      return this;
    }

    public HtmlTable WithMultiDataRow<T, TTitle>(IEnumerable<TTitle> titleItems,
      Func<TTitle, object> titleFunc, Func<IList<T>, TTitle, object> item,
      params IList<T>[] collections)
    {
      foreach (var title in titleItems)
      {
        WithDataRow(titleFunc(title), i=>item(i, title), collections);
      }
      return this;
    }

    public HtmlTable WithDataRow<T>(object title, Func<IList<T>, object> item,
      params IList<T>[] collections) => 
      WithRow(collections.Select(item).Prepend(HtmlTable.TD(title, ("align", "right"))));

    public HtmlTable PercentileRow<T>(object title, Func<T, bool> selector, params IList<T>[] collections) =>
      PercentileRow(title, selector, -1, -1, collections);
    public HtmlTable PercentileRow<T>(object title, Func<T, bool> selector, int p0, int p1, params IList<T>[] collections)
    {
      var cells = collections.Select(i => i.CountAndPercent(selector)).
        Prepend(HtmlTable.TD(title, ("align", "right")));
      if (p0 >= 0 && p1 >= 0)
      {
        var f0 = collections[p0].Fraction(selector);
        var f1 = collections[p1].Fraction(selector);
        var t = ProportionStatistics.DifferenceOfProportions(f0.numerator, f0.denominator, f1.numerator,
          f1.denominator);
        cells = cells.Concat($"{t.TwoTailedP:0.0000}");
      }
      WithRow(cells);
      return this;
    }

    public HtmlTable ValuesAsPercentileRows<T, TValue>(Func<T, TValue> value, Func<TValue, object> title,
      params IList<T>[] collections) =>
      ValuesAsPercentileRows(value, title, -1, -1, collections);
    public HtmlTable ValuesAsPercentileRows<T, TValue>(Func<T, TValue> value, Func<TValue, object> title, int p0, int p1, params IList<T>[] collections)
    {
      var values = collections.SelectMany(i => i).Select(value).Distinct().OrderBy(i => i).AsList();
      foreach (var which in values)
      {
        PercentileRow(title(which), i => value(i).Equals(which), p0, p1, collections);
      }
      return this;
    }


    public object ToDump()
    {
      var paddedRows = TablePadder.PadColumns(rows.Select(i => i.OfType<object>().Select(RenderCell).AsList()));
      return EmitClipboardLink(new XElement("LinqPad.Html",
        new XElement("table",
          paddedRows.Select(i => new XElement("tr", i))
        )));
    }

    private XElement RenderCell(object arg)
    {
      switch (arg)
      {
        case XElement xelt:
          return xelt;
        case ContentCell content:
          return content.Render(formatter);
        default:
          return new XElement("td", formatter.Format(arg));
      }
    }

    public static object TH(object content, params (string attr, object val)[] attrs)=>
      MakeContent(content, attrs, "th");
    public static object TD(object content, params (string attr, object val)[] attrs)=>
      MakeContent(content, attrs, "td");

    private static ContentCell MakeContent(object content, (string attr, object val)[] attrs, string tag)
    {
     return new ContentCell(tag, content, 
        attrs.Select(i=>new XAttribute(i.attr, i.val)).ToList());
    }

    private class ContentCell
    {
      private readonly string tag;
      private readonly object content;
      private readonly IList<XAttribute> attrs;

      public ContentCell(string tag, object content, IList<XAttribute> attrs)
      {
        this.tag = tag;
        this.content = content;
        this.attrs = attrs;
      }
      public XElement Render(ObjectFormatter formatter) =>
        new XElement(tag, formatter.Format(content), attrs);
    }

    public static Func<bool> DetectCtrlDown { get; set; } =
      () => Keyboard.IsKeyDown(Key.RightCtrl) || Keyboard.IsKeyDown(Key.LeftCtrl);

    public static XElement EmitClipboardLink(XElement ret)
    {
      if (DetectCtrlDown())
      {
        Console.WriteLine(new Hyperlinq(() =>
          ClipboardHelper.CopyToClipboard(ret.Elements().First().ToString()), "Copy to Clipboard", false));
      }
      return ret;
    }
  }
}