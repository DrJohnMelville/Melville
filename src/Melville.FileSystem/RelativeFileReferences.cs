﻿using System;

namespace Melville.FileSystem;

public static class RelativeFileReferences
{
    public static IFile? FileAtRelativePath(this IFile source, string path) =>
        source.Directory?.FileAtRelativePath(path);

    public static IFile? FileAtRelativePath(this IDirectory source, string path)
    {
        var splitPath = PathElements(path);
        if (splitPath.Length == 0) return null;
        return NavigatePath(source, splitPath.AsSpan()[..^1])?.File(splitPath[^1]);
    }

    public static IDirectory? FolderRelativePath(this IDirectory source, string path)
    {
        var splitPath = PathElements(path);
        if (splitPath.Length == 0) return null;
        return NavigatePath(source, splitPath.AsSpan());
    }

    private static string[] PathElements(string path) => path.Split(new char[] {'/', '\\'}, StringSplitOptions.RemoveEmptyEntries);

    private static IDirectory? NavigatePath(IDirectory source, Span<string> pathElement)
    {
        var ret = source;
        foreach (var element in pathElement)
        {
            ret = NavigateSinglePathElement(ret, element);
        }
        return ret;
    }

    private static IDirectory? NavigateSinglePathElement(IDirectory? source, string element) =>
        element switch
        {
            "." => source,
            ".." => source?.Directory,
            _=> source?.SubDirectory(element)
        };

}