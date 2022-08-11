﻿namespace Melville.FileSystem.CopyWithProgress;

public enum CopyProgressResult : uint
{
    PROGRESS_CONTINUE = 0,
    PROGRESS_CANCEL = 1,
    PROGRESS_STOP = 2,
    PROGRESS_QUIET = 3
}