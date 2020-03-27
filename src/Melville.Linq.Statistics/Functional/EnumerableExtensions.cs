using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;

namespace Melville.Linq.Statistics.Functional
{
  public static class EnumerableExtensions
  {
    /// <summary>
    /// Interleaves the specified collection with delimiters.
    /// </summary>
    /// <typeparam name="T">Type of the underlying collection</typeparam>
    /// <param name="baseEnumeration">The enumeration to be interleaved</param>
    /// <param name="delimiter">The delimeter placed before all elements except the first and last.</param>
    /// <param name="lastDelimiter">The delimiter placed before the last element.</param>
    /// <returns>an enumeration with the original elements interleaved with delimiters</returns>
    public static IEnumerable<T> Interleave<T>(this IEnumerable<T> baseEnumeration, T delimiter, T lastDelimiter)
    {
      Contract.Requires(baseEnumeration != null);
      Contract.Ensures(Contract.Result<IEnumerable<T>>() != null);
      return InnerInterleave(baseEnumeration, delimiter, lastDelimiter);
    }

    private static IEnumerable<T> InnerInterleave<T>(IEnumerable<T> baseEnumeration, T delimiter, T lastDelimiter)
    {
      using (var iter = baseEnumeration.GetEnumerator())
      {
        // guard clause for empty source
        if (!iter.MoveNext()) yield break;

        //first iteration of loop is unrolled because the first item does not have a delimiter
        yield return iter.Current;
        if (!iter.MoveNext()) yield break;

        //This is a while loop with a break in the middle.  I reuse the loop termination test to decide which interleavedItem to
        // use, thus the body of the loop gets repeated.
        while (true)
        {
          T item = iter.Current;
          if (iter.MoveNext())
          {
            yield return delimiter;
            yield return item;
          }
          else
          {
            yield return lastDelimiter;
            yield return item;
            yield break;
          }
        }
      }
    }

    /// <summary>
    /// Interleaves the specified enumerable with the given delimiter
    /// </summary>
    /// <typeparam name="T">Basis type of the enumeration</typeparam>
    /// <param name="enumerableToInterleave">The enumerable to interleave.</param>
    /// <param name="delimiter">The delimiter.</param>
    /// <returns>Original enumeration inteleaved with the given delimiter</returns>
    public static IEnumerable<T> Interleave<T>(this IEnumerable<T> enumerableToInterleave, T delimiter)
    {
      Contract.Requires(enumerableToInterleave != null);
      Contract.Ensures(Contract.Result<IEnumerable<T>>() != null);
      return InnerInterleave<T>(enumerableToInterleave, delimiter, delimiter);
    }

    /// <summary>
    /// Concatenates the strings in the given enumeration
    /// </summary>
    /// <param name="stringsToConcatenate">The strings to concatenate.</param>
    /// <returns>All of the given strings concatenated as a single string.</returns>
    public static String ConcatenateStrings<T>(this IEnumerable<T> stringsToConcatenate)
    {
      Contract.Requires(stringsToConcatenate != null);
      Contract.Ensures(Contract.Result<string>() != null);
      return string.Join(string.Empty, stringsToConcatenate.ToArray());
    }

    /// <summary>
    /// Additional overload to the concat operator in linq to objects lets us just list the items to concatenate
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="collection">The collection to concaternate to.</param>
    /// <param name="items">The items to concatentate.</param>
    /// <returns>An enumerable representing the enumeration of all the items</returns>
    public static IEnumerable<T> Concat<T>(this IEnumerable<T> collection, params T[] items)
    {
      Contract.Requires(collection != null);
      Contract.Requires(items != null);
      Contract.Ensures(Contract.Result<IEnumerable<T>>() != null);
      return Enumerable.Concat(collection, items);
    }


    public static T FirstOrDefault<T>(this IEnumerable<T> items, T defaultValue)
    {
      Contract.Requires(items != null);
      foreach (var item in items)
      {
        return item;
      }
      // only reach here if there are no items in the collection
      return defaultValue;
    }

    /// <summary>
    /// Separate the given enumeration into "chunks" of a givens size.  The last chunk may be shorter than ChunkLength
    /// </summary>
    /// <typeparam name="T">Tye type the enumerable is enumerating</typeparam>
    /// <param name="input">The enumerable to chunk.</param>
    /// <param name="chunkLength">Desired length of each chunk.</param>
    /// <returns></returns>
    public static IEnumerable<T[]> Chunks<T>(this IEnumerable<T> input, int chunkLength)
    {
      Contract.Requires(input != null);
      Contract.Requires(chunkLength > 0);
      Contract.Ensures(Contract.Result<IEnumerable<T[]>>() != null);
      return InnerChunks(input, chunkLength);
    }

    private static IEnumerable<T[]> InnerChunks<T>(IEnumerable<T> input, int chunkLength)
    {
      using (var enumerator = input.GetEnumerator())
      {
        int i;
        do
        {
          var ret = new T[chunkLength];
          for (i = 0; i < chunkLength && enumerator.MoveNext(); i++)
          {
            ret[i] = enumerator.Current;
          }
          if (i > 0)
            yield return chunkLength == i ? ret : ret.Take(i).ToArray();
        } while (i > 0);
      }
    }

    /// <summary>
    /// This returns an enumerable of n enumerables of T.  it will put one element in each enumerable before the Item1
    /// gets a second and so forth untill all the elements are exhausted.
    /// </summary>
    /// <typeparam name="T">The type of the element parameters</typeparam>
    /// <param name="input">The input enumerable.</param>
    /// <param name="numberOfOutputs">The number of output enumerables.</param>
    /// <returns></returns>
    public static IList<T>[] Decolate<T>(this IEnumerable<T> input, int numberOfOutputs)
    {
      Contract.Requires(input != null);
      Contract.Requires(numberOfOutputs > 0);
      var outputs = new List<T>[numberOfOutputs];
      for (int i = 0; i < outputs.Length; i++)
      {
        outputs[i] = new List<T>();
      }
      int j = 0;
      foreach (var item in input)
      {
        outputs[(j++) % numberOfOutputs].Add(item);
      }
      return outputs;
    }

    /// <summary>
    /// Returns a enumerable of all the elements prior to the sentinal, exclusive.
    /// If the sentinal is not found, it returns all the elements.
    /// </summary>
    /// <param name="input">The input enumerable.</param>
    /// <param name="sentinal">The sentinal value.</param>
    /// <returns>An enumerable of all the elements befor the sentinal, or
    /// all the elements if the senitnal is not found</returns>
    public static IEnumerable<T> AllBefore<T>(this IEnumerable<T> input, T sentinal)
    {
      Contract.Requires(input != null);
      return InnerAllBefore(input, sentinal);
    }

    public static IEnumerable<T> InnerAllBefore<T>(IEnumerable<T> input, T sentinal)
    {
      foreach (T elt in input)
      {
        if (object.Equals(elt, sentinal)) yield break;
        yield return elt;
      }
    }

    /// <summary>
    /// Constructs an list of the strings with commas, and the appropriate and conjunction
    /// </summary>
    /// <param name="items">The items to put oin the list.</param>
    /// <returns>A string with the items as an english list</returns>
    public static string EnglishList(this IEnumerable<string> items)
    {
      Contract.Requires(items != null);
      Contract.Ensures(Contract.Result<string>() != null);
      Contract.Ensures(Contract.ForAll(items, i => Contract.Result<string>().Contains(i)));
      Contract.Ensures(items.Count() < 2 || Contract.Result<string>().Contains("and"));
      Contract.Ensures(items.Count() < 3 || Contract.Result<string>().Contains(", and"));

      string prev = string.Empty;
      StringBuilder accumulator = new StringBuilder();
      int elementCount = 0;
      foreach (string current in items)
      {
        if (LeadingCommaRequired(elementCount))
        {
          accumulator.Append(", ");
        }
        accumulator.Append(prev);
        prev = current;
        elementCount++;
      }
      accumulator.Append(EnglishConjunction(elementCount));
      accumulator.Append(prev);
      return accumulator.ToString();
    }

    /// <summary>
    /// Creates the proper english conjunction for the list
    /// </summary>
    /// <param name="totalElements">The total number of elements in the list elements.</param>
    /// <returns>the conjunction between the beginning and end of the list</returns>
    private static string EnglishConjunction(int totalElements)
    {
      Contract.Requires(totalElements >= 0);
      Contract.Ensures(Contract.Result<string>() != null);
      switch (totalElements)
      {
        case 0:
        case 1:
          return String.Empty;
        case 2:
          return " and ";
        default:
          return ", and ";
      }
    }

    /// <summary>
    /// Because the first element of the list is a fake blank, commas are required before the third and following items
    /// </summary>
    /// <param name="elementCount">The element count so far.</param>
    /// <returns>true if a comma is required, false otherwise</returns>
    private static bool LeadingCommaRequired(int elementCount)
    {
      return elementCount > 1;
    }

    public static IEnumerable<T> Rotate<T>(this IEnumerable<T> source, int length)
    {
      var enumerator = source.GetEnumerator();
      var buffer = new T[length];
      int i = 0;
      for (; i < length; i++)
      {
        if (!enumerator.MoveNext()) break;
        buffer[i] = enumerator.Current;
      }
      while (enumerator.MoveNext())
      {
        yield return enumerator.Current;
      }
      for (int j = 0; j < i; j++)
      {
        yield return buffer[j];
      }
    }

    /// <summary>
    /// Applies a given action to every element of col
    /// </summary>
    /// <typeparam name="T">Element typew of the IEnumerable.</typeparam>
    /// <param name="col">IEnumerable to follow</param>
    /// <param name="action">Action to perform on each element</param>
    public static void ForEach<T>(this IEnumerable<T> col, Action<T> action)
    {
      Contract.Requires(col != null);
      Contract.Requires(action != null);

      foreach (var item in col)
      {
        action(item);

      }
    }

    /// <summary>
    /// Execute a side effect on each element of the enumerable as it is evaluated
    /// </summary>
    /// <typeparam name="T">Type of the enumerable</typeparam>
    /// <param name="source">The enumeration to iterate over</param>
    /// <param name="action">The side effect to execute on each item.</param>
    /// <returns></returns>
    public static IEnumerable<T> SideEffect<T>(this IEnumerable<T> source, Action<T> action)
    {
      foreach (var item in source)
      {
        action(item);
        yield return item;
      }
    }

    public static (TItem Min, TItem Max) MinMax<TItem, TKey>(this IEnumerable<TItem> items, Func<TItem, TKey> func)
      where TKey : IComparable
    {
      using (var enumerator = items.GetEnumerator())
      {
        if (!enumerator.MoveNext())
        {
          throw new InvalidOperationException("No members in query");
        }
        TItem min = enumerator.Current;
        TKey minKey = func(min);
        TItem max = min;
        TKey maxKey = minKey;
        while (enumerator.MoveNext())
        {
          var currentKey = func(enumerator.Current);
          if (minKey.CompareTo(currentKey) > 0)
          {
            min = enumerator.Current;
            minKey = currentKey;
          }
          else if (maxKey.CompareTo(currentKey) < 0)
          {
            max = enumerator.Current;
            maxKey = currentKey;
          }
        }
        return (min, max);
      }
    }

    public static (T Min, T Max) MinMax<T>(this IEnumerable<T> items) where T : IComparable => MinMax<T, T>(items, i => i);

    /// <summary>
    /// Skip up to and over the first instance of the item.
    /// If item is the default (null) then return the original sequency
    /// </summary>
    /// <typeparam name="T">Element type of the enumeration</typeparam>
    /// <param name="col">Enumeration to skip through</param>
    /// <param name="item">item to skip over</param>
    /// <returns>The enumeration after the given element, or the original
    /// enumeration if the element is the default, or null.</returns>
    public static IEnumerable<T> SkipOver<T>(this IEnumerable<T> col, T item)
    {
      if (Equals(item, default(T))) return col;
      return col.SkipWhile(i => !item.Equals(i)).Skip(1);
    }

    public static int IndexOf<T>(this IEnumerable<T> seq, T target)
    {
      Contract.Requires(seq != null);

      int count = 0;
      foreach (var item in seq)
      {
        if (Equals(item, target))
          return count;
        count++;
      }
      return -1;
    }

    public static T NextWithWrapping<T>(this IEnumerable<T> coll, T target)
    {
      return coll.SkipOver(target).Concat(coll).FirstOrDefault();
    }

    /// <summary>
    /// Prepends a list to another list
    /// </summary>
    /// <typeparam name="T">Type of the items in the list</typeparam>
    /// <param name="items">Man list</param>
    /// <param name="prefix">List to prepend</param>
    /// <returns></returns>
    public static IEnumerable<T> Prepend<T>(this IEnumerable<T> items, params T[] prefix)
    {
      return prefix.Concat(items);
    }

    /// <summary>
    /// Prepends a list to another list
    /// </summary>
    /// <typeparam name="T">Type of the items in the list</typeparam>
    /// <param name="items">Man list</param>
    /// <param name="prefix">List to prepend</param>
    /// <returns></returns>
    public static IEnumerable<T> Prepend<T>(this IEnumerable<T> items, IEnumerable<T> prefix)
    {
      return prefix.Concat(items);
    }

    // This implementation came from Stack Overflow: http://stackoverflow.com/questions/5489987/linq-full-outer-join
    /// <summary>
    /// Compute outer join of two sequences
    /// </summary>
    /// <typeparam name="TA">Type of first sequence</typeparam>
    /// <typeparam name="TB">Type of second sequence</typeparam>
    /// <typeparam name="TKey">Type of the keys</typeparam>
    /// <typeparam name="TResult">Result of the join</typeparam>
    /// <param name="a">First sequence</param>
    /// <param name="b">Second sequence</param>
    /// <param name="selectKeyA">Extract the key from an A</param>
    /// <param name="selectKeyB">Extract tje leu from a B</param>
    /// <param name="projection">Project an A and a B onto each other</param>
    /// <param name="defaultA">Default value for a missing A</param>
    /// <param name="defaultB">Default value for a missing B</param>
    /// <param name="cmp">Equality comparer for the key</param>
    /// <returns></returns>
    public static IEnumerable<TResult> FullOuterJoin<TA, TB, TKey, TResult>(
      this IEnumerable<TA> a,
      IEnumerable<TB> b,
      Func<TA, TKey> selectKeyA,
      Func<TB, TKey> selectKeyB,
      Func<TA, TB, TKey, TResult> projection,
      TA defaultA = default(TA),
      TB defaultB = default(TB),
      IEqualityComparer<TKey> cmp = null)
    {
      cmp = cmp ?? EqualityComparer<TKey>.Default;
      var alookup = a.ToLookup(selectKeyA, cmp);
      var blookup = b.ToLookup(selectKeyB, cmp);

      var keys = new HashSet<TKey>(alookup.Select(p => p.Key), cmp);
      keys.UnionWith(blookup.Select(p => p.Key));

      var join = from key in keys
        from xa in alookup[key].DefaultIfEmpty(defaultA)
        from xb in blookup[key].DefaultIfEmpty(defaultB)
        select projection(xa, xb, key);

      return join;
    }

    /// <summary>
    /// Returns the index of the item that has the given predicate
    /// </summary>
    /// <typeparam name="TSource">Type of the underlying enumeration</typeparam>
    /// <param name="source">Enumeration to search for</param>
    /// <param name="predicate">Describes the item to find</param>
    /// <returns></returns>
    public static int IndexOf<TSource>(this IEnumerable<TSource> source, Func<TSource, bool> predicate)
    {

      var index = 0;
      foreach (var item in source)
      {
        if (predicate.Invoke(item))
        {
          return index;
        }
        index++;
      }

      return -1;
    }

    /// <summary>
    /// Create an infinite enumaration that cycles through the given list over and over
    /// </summary>
    /// <typeparam name="T">Type of the items in the list</typeparam>
    /// <param name="source">List to cycle through</param>
    /// <returns>An infinite enumeration of the items in the surce list repeated over and over.</returns>
    public static IEnumerable<T> Cycle<T>(this IEnumerable<T> source)
    {
      var elts = source.ToList();
      if (elts.Count == 0) yield break;
      int current = 0;
      while (true)
      {
        yield return elts[current];
        current = (current + 1) % elts.Count;
      }
    }

    /// <summary>
    /// Returns an enumeration of a given length, repeating elements as needed.
    /// </summary>
    /// <typeparam name="T">Type of the enumerated elements</typeparam>
    /// <param name="source">Elements to enumerate</param>
    /// <param name="length">Length of the desired enumeration</param>
    /// <returns>enumeration of the given length</returns>
    public static IEnumerable<T> Cycle<T>(this IEnumerable<T> source, int length) =>
      source.Cycle().Take(length);
  }
}