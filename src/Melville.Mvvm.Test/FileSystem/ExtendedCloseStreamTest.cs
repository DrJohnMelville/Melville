﻿#nullable disable warnings
using System.IO;
using Melville.FileSystem;
using Xunit;

namespace Melville.Mvvm.Test.FileSystem;

public sealed class ExtendedCloseStreamTest
{
  [Fact]
  public void ExtendedCloseTest()
  {
    var i = 0;
    using (var str = new ExtendedCloseStream(new MemoryStream(), () => { i++; }))
    {
      Assert.Equal(0, i);
    }

    Assert.Equal(1, i);

  }
}