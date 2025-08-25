using System;
using System.Collections.Generic;

namespace Melville.FileSystem.BlockFile.FileSystemObjects;

public readonly struct DictionaryCleaner<TKey, TItem>(
    IDictionary<TKey, TItem> dictionary,
    Func<KeyValuePair<TKey, TItem>, bool> shouldCull)
{
    public void DoCull()
    {
        IEnumerator<KeyValuePair<TKey, TItem>> iterator;
        do
        {
            iterator = dictionary.GetEnumerator();
        } while (RecursiveCull(iterator, 0));

    }

    private bool RecursiveCull(IEnumerator<KeyValuePair<TKey, TItem>> iterator, int count)
    {
        if (count > 100) return true;
        while (iterator.MoveNext())
        {
            if (!shouldCull(iterator.Current)) continue;
            
            var currentKey = iterator.Current.Key;
            var ret = RecursiveCull(iterator, count + 1);
            dictionary.Remove(currentKey);
            return ret;
        }
        return false;
    }
}