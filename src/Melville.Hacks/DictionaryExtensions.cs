using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

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
#if NET5_0_OR_GREATER
    public static TItem GetOrCreate<TKey, TItem>(
        this Dictionary<TKey, TItem> dict, TKey key, Func<TKey, TItem> creator) where TKey: notnull
    {
        ref var value = ref CollectionsMarshal.GetValueRefOrAddDefault(dict, key, out bool exists);
        if (!exists)
        {
            value = creator(key);
        }
        return value!;
    }
#endif
}