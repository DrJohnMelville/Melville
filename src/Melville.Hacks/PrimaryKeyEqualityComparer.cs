using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace Melville.Hacks;

/// <summary>
/// This clas lets you define identity semantics based on a primary key
/// </summary>
public sealed class PrimaryKeyEqualityComparer<TItem, TKey>:EqualityComparer<TItem> where TKey: IEquatable<TKey>
{
  private readonly Func<TItem, TKey> keySelector;

  public PrimaryKeyEqualityComparer(Func<TItem, TKey> keySelector)
  {
    this.keySelector = keySelector;
  }

  public override bool Equals([AllowNull]TItem x, [AllowNull]TItem y) =>
    (x, y) switch
    {
      (null, null) => true,
      (null, _) => false,
      (_, null) => false,
      (TItem a, TItem b) => RealCompare(a, b)
    };

  private bool RealCompare(TItem x, TItem y)
  {
    var xKey = keySelector(x);
    var yKey = keySelector(y);
    return xKey?.Equals(yKey) ?? yKey == null;
  }

  public override int GetHashCode(TItem obj) => 
    keySelector(obj)?.GetHashCode() ?? 0;
}