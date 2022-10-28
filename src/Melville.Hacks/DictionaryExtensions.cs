using System;
using System.Collections.Generic;

namespace Melville.Hacks;

public static class DictionaryExtensions
{
    public static TItem GetOrCreate<TKey, TItem>(
        this IDictionary<TKey, TItem> dict, TKey key, Func<TKey, TItem> creator)
    {
        if (dict.TryGetValue(key, out var ret)) return ret;
        var newItem = creator(key);
        dict[key] = newItem;
        return newItem;
    }
}