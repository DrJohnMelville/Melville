using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Runtime.InteropServices;
using System.Xml;

namespace Melville.Linq.Statistics.Functional
{

  public static class FunctionalMethods
  {

    public static TDest Map<TSource, TDest>(TSource item,
      params (TSource Source, TDest Dest)[] tuples)
    {
      return tuples.First(i => Object.Equals(item, i.Source)).Dest;
    }

    public static TDest Map<TSource, TDest>(TSource item, TDest defaultValue,
      params (TSource Source, TDest Dest)[] tuples)
    {
      return tuples.Where(i => Object.Equals(item, i.Source))
        .DefaultIfEmpty((Source:default(TSource), Dest:defaultValue))
        .First()
        .Dest;
    }

    public static IList<T> AsList<T>(this IEnumerable<T> source) =>
      source as IList<T> ?? source.ToList();

    public static IEnumerable<object> ObjectListWithNulls(this IEnumerable src)
    {
      foreach (var item in src)
      {
        yield return item;
      }
    }

    /// <summary>
    /// Construct an IEnumerable where each element is a function of the previous element
    /// </summary>
    /// <typeparam name="T">The type of the elements</typeparam>
    /// <param name="first">The first element, which is returned in the sequence.</param>
    /// <param name="sequenceFunction">The sequence function, which returns the next member given the previous.</param>
    /// <param name="sentinal">The sentinal, or the value that indicates the end point.  The Sentinal is NOT returned in the sequence.</param>
    /// <returns></returns>
    public static IEnumerable<T> Sequence<T>(T first, Func<T, T> sequenceFunction, T sentinal)
    {
      Contract.Requires(sequenceFunction != null);
      Contract.Ensures(Contract.Result<IEnumerable<T>>() != null);
      Contract.Ensures(Contract.ForAll(Contract.Result<IEnumerable<T>>(),
        i => !Equals(i, sentinal)));
      Contract.Ensures(Equals(first, sentinal) ||
                       Equals(first, Contract.Result<IEnumerable<T>>().First()));
      return InnerSequence(first, sequenceFunction, sentinal);
    }
    public static IEnumerable<T> InnerSequence<T>(T first, Func<T, T> sequenceFunction, T sentinal)
    {
      while (!Equals(first, sentinal))
      {
        yield return first;
        first = sequenceFunction(first);
      }
    }
    /// <summary>
    /// Construct an IEnumerable where each element is a function of the previous element
    /// </summary>
    /// <typeparam name="T">The type of the elements</typeparam>
    /// <param name="first">The first element, which is returned in the sequence.</param>
    /// <param name="sequenceFunction">The sequence function, which returns the next member given the previous.</param>
    /// <returns></returns>
    public static IEnumerable<T> Sequence<T>(T first, Func<T, T> sequenceFunction)
    {
      Contract.Requires(sequenceFunction != null);
      return Sequence(first, sequenceFunction, default(T));
    }

    /// <summary>
    /// This method takes a parameter (the aggregator) and an enumerable object.
    /// each enumerable item is passed with the aggregator to an Action delegate,
    /// finally the aggregator is returned.
    /// </summary>
    /// <typeparam name="TReturn">The type of the aggregator.</typeparam>
    /// <typeparam name="TElement">The type of the elements to be aggregated.</typeparam>
    /// <param name="sequence">The sequence of elements to aggregate.</param>
    /// <param name="aggregator">The aggregator.</param>
    /// <param name="aggregatorAction">The aggregator action.</param>
    /// <returns></returns>
    public static TReturn AggregateAction<TReturn, TElement>(this IEnumerable<TElement> sequence, TReturn aggregator, Action<TReturn, TElement> aggregatorAction)
    {
      Contract.Requires(sequence != null);
      Contract.Requires(aggregatorAction != null);
      Contract.Ensures(Equals(Contract.Result<TReturn>(), aggregator));
      foreach (TElement element in sequence)
      {
        aggregatorAction(aggregator, element);
      }
      return aggregator;
    }

    /// <summary>
    /// Repeats the specified element a specified number of times.
    /// </summary>
    /// <typeparam name="T">Element type of the returned sequence</typeparam>
    /// <param name="element">The element to be repeated.</param>
    /// <param name="copies">The number of copies to return.</param>
    /// <returns>an enumerable object with a </returns>
    public static IEnumerable<T> Repeat<T>(T element, int copies)
    {
      Contract.Requires(copies >= 0);
      Contract.Ensures(Contract.Result<IEnumerable<T>>() != null);
      Contract.Ensures(Contract.Result<IEnumerable<T>>().Count() == copies);
      Contract.Ensures(Contract.ForAll(Contract.Result<IEnumerable<T>>(),
        i => Equals(element, i)));
      return InnerRepeat(element, copies);
    }
    public static IEnumerable<T> InnerRepeat<T>(T element, int copies)
    {
      for (int i = 0; i < copies; i++)
      {
        yield return element;
      }
    }

    public static IEnumerable<T> Repeat<T>(this IEnumerable<T> src, int copies)
    {
      if (copies < 1) return new T[0];
      var list = src.AsList();
      return list.Cycle(copies * list.Count);
    }

    /// <summary>
    /// "Clamps" a value to be within the range of min...max inclusive
    /// </summary>
    /// <param name="value">the value to be clamped</param>
    /// <param name="min">The minimum value</param>
    /// <param name="max">the maximum value</param>
    /// <returns>The initial value, clamped to the range of min...max</returns>
    public static double Clamp(this double value, double min, double max)
    {
      Contract.Requires(min <= max);

      return Math.Max(min, Math.Min(max, value));
    }

    /// <summary>
    /// "Clamps" a value to be within the range of min...max inclusive
    /// </summary>
    /// <param name="value">the value to be clamped</param>
    /// <param name="min">The minimum value</param>
    /// <param name="max">the maximum value</param>
    /// <returns>The initial value, clamped to the range of min...max</returns>
    public static int Clamp(this int value, int min, int max)
    {
      Contract.Requires(min <= max);

      return Math.Max(min, Math.Min(max, value));
    }

    /// <summary>
    /// Returns true if Min &lt;= value &lt;= max
    /// </summary>
    /// <param name="value">the value to be clamped</param>
    /// <param name="min">The minimum value</param>
    /// <param name="max">the maximum value</param>
    /// <returns>The initial value, clamped to the range of min...max</returns>
    public static bool IsInRange(this double value, double min, double max)
    {
      Contract.Requires(min <= max);

      return (min <= value) && (value <= max);
    }
    /// <summary>
    /// Returns true if Min &lt;= value &lt;= max
    /// </summary>
    /// <param name="value">the value to be clamped</param>
    /// <param name="min">The minimum value</param>
    /// <param name="max">the maximum value</param>
    /// <returns>The initial value, clamped to the range of min...max</returns>
    public static bool IsInRange(this int value, int min, int max)
    {
      Contract.Requires(min <= max);

      return (min <= value) && (value <= max);
    }

    public static T Fix<T>(this T item, Action<T> action)
    {
      Contract.Requires(action != null);
      action(item);
      return item;
    }

    /// <summary>
    /// Convert a jagged array into a 2 dimensional array
    /// </summary>
    /// <typeparam name="T">Type of the elements</typeparam>
    /// <param name="data">2 dimensional array of arrays</param>
    /// <returns>2d array with the same data</returns>
    public static T[,] To2x2<T>(this IList<IList<T>> data)
    {
      var rows = data.Count;
      var cols = data.Max(i => i.Count);
      var ret = new T[rows, cols];
      for (int i = 0; i < rows; i++)
      {
        for (int j = 0; j < data[i].Count; j++)
        {
          ret[i, j] = data[i][j];
        }
      }
      return ret;
    }
  }
}