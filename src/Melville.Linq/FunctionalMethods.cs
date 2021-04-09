using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace Melville.Linq
{
  public static class FunctionalMethods
  {
    /// <summary>
    /// Construct an IEnumerable where each element is a function of the previous element
    /// </summary>
    /// <typeparam name="T">The type of the elements</typeparam>
    /// <param name="first">The first element, which is returned in the sequence.</param>
    /// <param name="sequenceFunction">The sequence function, which returns the next member given the previous.</param>
    /// <param name="sentinal">The sentinal, or the value that indicates the end point.  The Sentinal is NOT returned in the sequence.</param>
    /// <returns></returns>
    public static IEnumerable<T> Sequence<T>(T first, Func<T, T> sequenceFunction, [MaybeNull]T sentinal)
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
      return Sequence(first, sequenceFunction, default(T)!);
    }

    /// <summary>
    /// Repeats the specified element a specified number of times.
    /// </summary>
    /// <typeparam name="T">Element type of the returned sequence</typeparam>
    /// <param name="element">The element to be repeated.</param>
    /// <param name="copies">The number of copies to return.</param>
    /// <returns>an enumerable object with a </returns>
    public static IEnumerable<T> Repeat<T>(T element, int copies) => Enumerable.Repeat(element, copies);
    
    public static T Fix<T>(this T item, Action<T> action)
    {
      action(item);
      return item;
    }
    
    /// <summary>
    /// Get an entire tree of nodes using the given function to find children.
    /// This function implements a preorder traversal.
    /// </summary>
    /// <typeparam name="TSource">The type of the node being found.</typeparam>
    /// <param name="source">The root of the tree.</param>
    /// <param name="recursiveSelector">The function the maps any node to the enumeration of its children.</param>
    /// <returns>All of the children and subchildren of this node in pre-order</returns>
    public static IEnumerable<TSource> SelectRecursive<TSource>(
      this IEnumerable<TSource> source,
      Func<TSource, IEnumerable<TSource>?> recursiveSelector) =>
      new RecursiveSelector<TSource>(source, recursiveSelector);


    public static IEnumerable<TSource> SelectRecursive<TSource>(
      TSource source, Func<TSource, IEnumerable<TSource>?> recursiveSelector) =>
      new EnumerateSingle<TSource>(source).SelectRecursive(recursiveSelector);
  }
}