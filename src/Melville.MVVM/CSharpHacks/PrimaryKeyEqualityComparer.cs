using  System;
using System.Collections.Generic;

namespace Melville.MVVM.CSharpHacks
{
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

    public override bool Equals(TItem x, TItem y)
    {
      var xKey = keySelector(x);
      var yKey = keySelector(y);

      return xKey?.Equals(yKey) ?? yKey == null;
    }

    public override int GetHashCode(TItem obj) => 
      keySelector(obj)?.GetHashCode() ?? 0;
  }
}