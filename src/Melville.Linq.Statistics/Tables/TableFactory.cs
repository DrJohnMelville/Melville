using System;
using System.Collections.Generic;
using System.Linq;

namespace Melville.Linq.Statistics.Tables
{
  public class RequireStruct<T> where T : struct { private RequireStruct() { } }
  public class RequireClass<T> where T : class { private RequireClass() { } }

  public static class TableFactory
  {

    public static ITable<T> Table<T>(this IEnumerable<T> items, RequireStruct<T> reservedMustBeNull = null) where  T:struct =>
      new ValueTable<T>(items);
    public static ITable<T> Table<T>(this IEnumerable<T> items, RequireClass<T> reservedMustBeNull = null) where
      T: class=> new ReferenceTable<T>(items);
    public static ITable<T> Table<T>(this ISet<T> items, RequireClass<T> reservedMustBeNull = null) where
      T: class=> new ReferenceTable<T>(items);

    public static ITable<T> WithCellFunction<T>(this ITable<T> table, Func<IEnumerable<T>, object> func,
      SummaryFunctionSelection selection = SummaryFunctionSelection.All)
    {
      return table.WithCellFunction((row, col, cell) => func(cell), selection);
    }

    public static ITable<T> WithCellFunction<T>(this ITable<T> table, Func<ITable<T>,ConfiguredCellFunction<T>> config)
    {
      var func = config(table);
      return table.WithCellFunction(func.Func, func.DefaultSelection);
    }
    ///selection parameter overrides the default
    public static ITable<T> WithCellFunction<T>(this ITable<T> table, Func<ITable<T>, ConfiguredCellFunction<T>> config,
      SummaryFunctionSelection selection)
    {
      var func = config(table);
      return table.WithCellFunction(func.Func, selection);
    }
    #region TableDefinitions

    private sealed class ReferenceTable<TItem> : TableImplementation<TItem, TItem> where TItem: class
    {
      public ReferenceTable(IEnumerable<TItem> items) : this(new ObjectIdentitySet<TItem>(items))
      {
      }

      public ReferenceTable(ObjectIdentitySet<TItem> items) : base(items)
      {
      }

      protected override TItem FromStorage(TItem item) => item;

    }


    public sealed class ValueBox<T> where T :struct
    {
      public T Value { get; }

      public ValueBox(T value)
      {
        Value = value;
      }
    }

    private sealed class ValueTable<T> : TableImplementation<T, ValueBox<T>> where T:struct
    {
      public ValueTable(IEnumerable<T> items) : 
        base(new ObjectIdentitySet<ValueBox<T>>(items.Select(i => new ValueBox<T>(i))))
      {
      }

      protected override T FromStorage(ValueBox<T> item) => item.Value;
    }


    #endregion
  }

  public static class CellFunctions
  {
    public static string PercentAndFraction<T>(this IEnumerable<T>items, Func<T, bool> predicate)
    {
      int numerator = 0;
      int denominator = 0;
      foreach (var item in items)
      {
        denominator++;
        if (predicate(item))
        {
          numerator++;
        }
      }
      return PercentAndFraction(numerator, denominator);
    }
    public static string PercentAndFraction<TNum, TDenom>(IEnumerable<TNum> numerator, IEnumerable<TDenom> denominator) =>
      PercentAndFraction(numerator.Count(), denominator.Count());
    public static string PercentAndFraction(double numerator, double denominator) => 
      denominator == 0.0?$"{numerator}/{denominator}":string.Format(PercentAndFractionFormat(denominator),
      100 * numerator / denominator, numerator, denominator);
    private static string PercentAndFractionFormat(double denominator) => 
      denominator >= 100 ? "{0:#0.0}% ({1}/{2})" : "{0:#0}% ({1}/{2})";

    private static string AxisPercentFormat(int denominator) => denominator >= 100 ? "{0} ({1:##0.0%})" : "{0} ({1:##0%)}";
    private static string AxisPercentage<T>(IEnumerable<T> axis, IEnumerable<T> cell)
    {
      var numerator = cell?.Count() ??0;
      var denominator = axis?.Count() ?? 1;
      return string.Format(AxisPercentFormat(denominator), numerator, (1.0 * numerator) / denominator);
   }
    //unused is not used, but it's presence allows the type resolution to work automatically
    public static ConfiguredCellFunction<T> RowPercentage<T>(ITable<T> table) =>
      new ConfiguredCellFunction<T>((row,col,cell)=>AxisPercentage(row,cell), 
        SummaryFunctionSelection.Cell | SummaryFunctionSelection.Column);
    //unused is not used, but it's presence allows the type resolution to work automatically
    public static ConfiguredCellFunction<T> ColumnPercentage<T>(ITable<T> table) =>
      new ConfiguredCellFunction<T>((row,col,cell)=>AxisPercentage(col,cell), 
        SummaryFunctionSelection.Cell | SummaryFunctionSelection.Row);

    public static ConfiguredCellFunction<T> TablePercentage<T>(ITable<T> table) =>
      new ConfiguredCellFunction<T>((row,col,cell)=>AxisPercentage(table.AllValues(),cell),
        SummaryFunctionSelection.Cell | SummaryFunctionSelection.Column | SummaryFunctionSelection.Row);
  }

  public class ConfiguredCellFunction<T>
  {
    public Func<IEnumerable<T>, IEnumerable<T>, IEnumerable<T>, string> Func { get; }
    public SummaryFunctionSelection DefaultSelection { get; }

    public ConfiguredCellFunction(Func<IEnumerable<T>, IEnumerable<T>, IEnumerable<T>, string> func, SummaryFunctionSelection defaultSelection)
    {
      Func = func;
      DefaultSelection = defaultSelection;
    }
  }
}