using  System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace Melville.MVVM.Functional
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
    public static IEnumerable<T> Repeat<T>(T element, int copies)
    {
      return InnerRepeat(element, copies);
    }
    public static IEnumerable<T> InnerRepeat<T>(T element, int copies)
    {
      for (int i = 0; i < copies; i++)
      {
        yield return element;
      }
    }

    /// <summary>
    /// "Clamps" a value to be within the range of min...max inclusive
    /// </summary>
    /// <param name="value">the value to be clamped</param>
    /// <param name="min">The minimum value</param>
    /// <param name="max">the maximum value</param>
    /// <returns>The initial value, clamped to the range of min...max</returns>
    public static double Clamp(this double value, double min, double max) => Math.Max(min, Math.Min(max, value));

    /// <summary>
    /// "Clamps" a value to be within the range of min...max inclusive
    /// </summary>
    /// <param name="value">the value to be clamped</param>
    /// <param name="min">The minimum value</param>
    /// <param name="max">the maximum value</param>
    /// <returns>The initial value, clamped to the range of min...max</returns>
    public static int Clamp(this int value, int min, int max) => Math.Max(min, Math.Min(max, value));

    /// <summary>
    /// Returns true if Min &lt;= value &lt;= max
    /// </summary>
    /// <param name="value">the value to be clamped</param>
    /// <param name="min">The minimum value</param>
    /// <param name="max">the maximum value</param>
    /// <returns>The initial value, clamped to the range of min...max</returns>
    public static bool IsInRange(this double value, double min, double max) => (min <= value) && (value <= max);

    /// <summary>
    /// Returns true if Min &lt;= value &lt;= max
    /// </summary>
    /// <param name="value">the value to be clamped</param>
    /// <param name="min">The minimum value</param>
    /// <param name="max">the maximum value</param>
    /// <returns>The initial value, clamped to the range of min...max</returns>
    public static bool IsInRange(this int value, int min, int max) => (min <= value) && (value <= max);

    public static T Fix<T>(this T item, Action<T> action)
    {
      action(item);
      return item;
    }
  }
}