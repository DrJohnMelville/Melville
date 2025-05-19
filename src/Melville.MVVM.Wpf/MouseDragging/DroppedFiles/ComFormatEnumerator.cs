using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;

namespace Melville.MVVM.Wpf.MouseDragging.DroppedFiles;

internal class ComFormatEnumerator(List<ClipboardItem> items): IEnumFORMATETC
{
    private int position = 0;

    /// <inheritdoc />
    public void Clone(out IEnumFORMATETC newEnum)
    {
        var ret = new ComFormatEnumerator(items);
        ret.position = position;
        newEnum = ret;

    }

    /// <inheritdoc />
    public int Next(int celt, FORMATETC[] rgelt, int[]? pceltFetched)
    {
        var source = CollectionsMarshal.AsSpan(items);
        var count = Math.Min(celt, source.Length-position);
        for (int i = 0; i < count; i++)
        {
            source[position++].WriteFormat(ref rgelt[i]);
        }

        if (pceltFetched is not null) pceltFetched[0] = count;

        return count == celt ? NativeConstants.S_OK: NativeConstants.S_FALSE;
    }

    /// <inheritdoc />
    public int Reset()
    {
        position = 0;
        return 0;
    }

    /// <inheritdoc />
    public int Skip(int celt)
    {
        position = Math.Min(position + celt, items.Count);
        return 0;
    }
}