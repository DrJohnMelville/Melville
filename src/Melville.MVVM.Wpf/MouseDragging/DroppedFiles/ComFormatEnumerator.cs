﻿using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;
using System.Windows.Forms;

namespace Melville.MVVM.Wpf.MouseDragging.DroppedFiles;

internal class ComFormatEnumerator(ClipboardItem[] items): IEnumFORMATETC
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
        int fetched = 0;
        while (fetched < celt && position < items.Length)
        {
            ref var item = ref items[position];
            if (item.IsComCompatible())
            {
                item.WriteFormat(ref rgelt[fetched++]);
            }

            position++;
        }

        if (pceltFetched is not null) pceltFetched[0] = fetched;

        return fetched == celt ? NativeConstants.S_OK: NativeConstants.S_FALSE;
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
        position = Math.Min(position + celt, items.Length);
        return 0;
    }
}

public static class WrapIEnumFormatEtc
{
    public static IEnumerable<FORMATETC> Wrap(this IEnumFORMATETC items)
    {
        var fetched = new int[1];
        var item = new FORMATETC[1];
        while (true)
        {
            items.Next(1, item, fetched);
            if (fetched[0] != 1) yield break;
            yield return item[0];
        }
    }
}