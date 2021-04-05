﻿using System;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;

namespace Melville.FileSystem.FileSystem
{
  public static class ArrayExtensions
  {
    public static int FillBuffer(this byte[] buffer, int offset, int maxLength,
      Func<byte[], int, int, int> readMethod)
    {
      int totalRead = 0;
      int localRead = int.MaxValue;
      while (maxLength > totalRead && localRead > 0)
      {
        localRead = readMethod(buffer, offset + totalRead, maxLength - totalRead);
        totalRead += localRead;
      }
      return totalRead;
    }

    public static async Task<int> FillBufferAsync(this byte[] buffer, int offset, int count,
      Stream s)
    {
      int totalRead = 0;
      int localRead = int.MaxValue;
      while (count - totalRead > 0 && localRead > 0)
      {
        localRead = await s.ReadAsync(buffer, offset + totalRead, count - totalRead);
        totalRead += localRead;
      }
      Debug.Assert(buffer.Length >= offset + totalRead);
      Debug.Assert(totalRead >= 0);
      Debug.Assert(totalRead <= count);
      return totalRead;
    }

  }
}