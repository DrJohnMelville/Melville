using System.Collections.Generic;

namespace Melville.Linq;

public static class CountedEnumerable
{
    public static IEnumerable<(T Item, int Index, bool Last)> AsCounted<T>(this IEnumerable<T> list)
    {
        var enumerator = list.GetEnumerator();
        if (!enumerator.MoveNext()) yield break;
        var prior = enumerator.Current;
        var count = 0;

        while (enumerator.MoveNext())
        {
            yield return (prior, count++, false);
            prior = enumerator.Current;
        }
        yield return (prior, count, true);
    }
}