using  System;
using System.Diagnostics;

namespace Melville.MVVM.Invariants
{
  public static class MyDebug
  {
    [Conditional("DEBUG")]
    public static void Assert(bool condition, string message = "Assertion Failed")
    {
      if (!condition)
      {
        throw new InvalidOperationException(message);
      }
    }
    
  }
}