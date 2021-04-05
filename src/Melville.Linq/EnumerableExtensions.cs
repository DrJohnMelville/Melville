using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;

namespace Melville.Linq
{
  public static class EnumerableExtensions
  {
    /// <summary>
    /// Interleaves the specified enumerable with the given delimiter
    /// </summary>
    /// <typeparam name="T">Basis type of the enumeration</typeparam>
    /// <param name="enumerableToInterleave">The enumerable to interleave.</param>
    /// <param name="delimiter">The delimiter.</param>
    /// <returns>Original enumeration inteleaved with the given delimiter</returns>
    public static IEnumerable<T> Interleave<T>(this IEnumerable<T> enumerableToInterleave, T delimiter) =>
      Interleave(enumerableToInterleave, delimiter, delimiter);

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
      using var iter = baseEnumeration.GetEnumerator();
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

    /// <summary>
    /// Concatenates the strings in the given enumeration
    /// </summary>
    /// <param name="stringsToConcatenate">The strings to concatenate.</param>
    /// <returns>All of the given strings concatenated as a single string.</returns>
    public static String ConcatenateStrings<T>(this IEnumerable<T> stringsToConcatenate, string? separator = null) => 
      string.Join(separator ?? "", stringsToConcatenate.ToArray());

    /// <summary>
    /// Additional overload to the concat operator in linq to objects lets us just list the items to concatenate
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="collection">The collection to concaternate to.</param>
    /// <param name="items">The items to concatentate.</param>
    /// <returns>An enumerable representing the enumeration of all the items</returns>
    public static IEnumerable<T> Concat<T>(this IEnumerable<T> collection, params T[] items) => 
      Enumerable.Concat(collection, items);


    public static T FirstOrDefault<T>(this IEnumerable<T> items, T defaultValue)
    {
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
    public static IEnumerable<IList<T>> Chunks<T>(this IEnumerable<T> input, int chunkLength)
    {
      using (var enumerator = input.GetEnumerator())
      {
        int i;
        do
        {
          var ret = new List<T>(chunkLength);
          for (i = 0; i < chunkLength && enumerator.MoveNext(); i++)
          {
            ret.Add(enumerator.Current);
          }

          if (i > 0)
            yield return ret;
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
      foreach (T elt in input)
      {
        if (Equals(elt, sentinal)) yield break;
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
      string prev = "";
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
    private static string EnglishConjunction(int totalElements) =>
      totalElements switch
      {
        0 => "",
        1 => "",
        2 => " and ",
        _ => ", and "
      };

    /// <summary>
    /// Because the first element of the list is a fake blank, commas are required before the third and following items
    /// </summary>
    /// <param name="elementCount">The element count so far.</param>
    /// <returns>true if a comma is required, false otherwise</returns>
    private static bool LeadingCommaRequired(int elementCount)
    {
      return elementCount > 1;
    }

    public static IEnumerable<T> Rotate<T>(this IEnumerable<T> source, int i) =>
      source.Skip(i).Concat(source.Take(i));

    /// <summary>
    /// Applies a given action to every element of col
    /// </summary>
    /// <typeparam name="T">Element typew of the IEnumerable.</typeparam>
    /// <param name="col">IEnumerable to follow</param>
    /// <param name="action">Action to perform on each element</param>
    public static void ForEach<T>(this IEnumerable<T> col, Action<T> action)
    {
      foreach (var item in col)
      {
        action(item);
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

    public static (T Min, T Max) MinMax<T>(this IEnumerable<T> items) where T : IComparable
    {
      return MinMax(items, i => i);
    }

    /// <summary>
    /// Skip up to and over the first instance of the item.
    /// If item is the default (null) then return the original sequency
    /// </summary>
    /// <typeparam name="T">Element type of the enumeration</typeparam>
    /// <param name="col">Enumeration to skip through</param>
    /// <param name="item">item to skip over</param>
    /// <returns>The enumeration after the given element</returns>
    public static IEnumerable<T> SkipOver<T>(this IEnumerable<T> col, [MaybeNull]T item)
    {
      if (object.Equals(item, default(T)!)) return col;
      return col.SkipWhile(i => !object.Equals(item, i)).Skip(1);
    }

    public static int IndexOf<T>(this IEnumerable<T> seq, T target)
    {
      int count = 0;
      foreach (var item in seq)
      {
        if (Equals(item, target))
          return count;
        count++;
      }

      return -1;
    }
    
    public static T? NextWithWrapping<T>(this IEnumerable<T> coll, T target)
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
      IEqualityComparer<TKey>? cmp = null)
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

    public static TItem MaxThat<TItem, TKey>(this IEnumerable<TItem> data,
      Func<TItem, TKey> keyFunc) where TKey : IComparable<TKey> => InnerMaxThat(data, keyFunc, 1);

    public static TItem MinThat<TItem, TKey>(this IEnumerable<TItem> data,
      Func<TItem, TKey> keyFunc) where TKey : IComparable<TKey> => InnerMaxThat(data, keyFunc, -1);

    private static TItem InnerMaxThat<TItem, TKey>(IEnumerable<TItem> data,
      Func<TItem, TKey> keyFunc, int factor) where TKey : IComparable<TKey>
    {
      using (var enumerator = data.GetEnumerator())
      {
        if (!enumerator.MoveNext())
        {
          throw new InvalidOperationException("Sequence has no elements");
        }

        var ret = enumerator.Current;
        var retKey = keyFunc(ret);
        while (enumerator.MoveNext())
        {
          var newKey = keyFunc(enumerator.Current);
          if (factor * newKey.CompareTo(retKey) > 0)
          {
            retKey = newKey;
            ret = enumerator.Current;
          }
        }

        return ret;
      }
    }

    /// <summary>
    /// Execute a side effect on each element of the enumerable as it is evaluated
    /// </summary>
    /// <typeparam name="T">Type of the enumerable</typeparam>
    /// <param name="source">The enumeration to iterate over</param>
    /// <param name="action">The side effect to execute on each item.</param>
    /// <returns></returns>
    public static IEnumerable<T> FixElements<T>(this IEnumerable<T> source, Action<T> action)
    {
      foreach (var item in source)
      {
        action(item);
        yield return item;
      }
    }

    /// <summary>
    /// Swap the element of a list with another element relative to the first
    /// </summary>
    /// <typeparam name="T">Type of elements in the list</typeparam>
    /// <param name="list">List to be mutated by this operation</param>
    /// <param name="item">Item to move</param>
    /// <param name="delta">Location where the element should go, relative to its current position</param>
    public static void SwapRelative<T>(this IList<T> list, T item, int delta)
    {
      var index = list.IndexOf(item);
      list[index] = list[index + delta];
      list[index + delta] = item;
    }

    /// <summary>
    /// Enumerate the enumerable and create an observable collection
    /// </summary>
    /// <typeparam name="T">Element type of the observable collection</typeparam>
    /// <param name="source">Elements to put in  the collection</param>
    /// <returns>An observable collection of the elements in the enumerable</returns>
    public static ObservableCollection<T> ToObservableCollection<T>(this IEnumerable<T> source) =>
      new ObservableCollection<T>(source);


    public static IEnumerable<T> FromObject<T>(params T[] obj) => obj;

    /// <summary>
    /// Execute the action on all pairs of equal indexed item in two enumerables
    /// </summary>
    /// <typeparam name="TFirst">Type of the first enumeration of items.</typeparam>
    /// <typeparam name="TSecond">Type of the second enumeration of items.</typeparam>
    /// <param name="first">First enumeration of items</param>
    /// <param name="second">second enumeration of items</param>
    /// <param name="action">action to perform on the pairs of items</param>
    public static void Zip<TFirst, TSecond>(this IEnumerable<TFirst> first, IEnumerable<TSecond> second,
      Action<TFirst, TSecond> action)
    {
      using (IEnumerator<TFirst> e1 = first.GetEnumerator())
      {
        using (IEnumerator<TSecond> e2 = second.GetEnumerator())
        {
          while (e1.MoveNext() && e2.MoveNext())
            action(e1.Current, e2.Current);
        }
      }

    }

    /// <summary>
    /// Returns the nth element of the enumerable.  This is the enumerable
    /// equivilent of an array index
    /// </summary>
    /// <typeparam name="T">Type of the enumerable</typeparam>
    /// <param name="items">base enumerable</param>
    /// <param name="pos">Item to take0</param>
    /// <returns>The nth element of the enumerable.  Throws if it does not exist</returns>
    public static T ByIndex<T>(this IEnumerable<T> items, int pos) =>
      items.Skip(pos).First();

    /// <summary>
    /// Returns the first string which will show up if not printed
    /// </summary>
    /// <param name="strings">An enumerable of candidate strings, in preferred order</param>
    /// <returns></returns>
    public static string FirstVisible(this IEnumerable<string> strings) =>
      strings.FirstOrDefault(i => !string.IsNullOrWhiteSpace(i))??"";

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

    public static IEnumerable<T> EmptyIfNull<T>(this IEnumerable<T>? source) =>
      source ?? Array.Empty<T>();
    public static IEnumerable EmptyIfNull(this IEnumerable? source) =>
      source ?? Array.Empty<object>();

  }
}