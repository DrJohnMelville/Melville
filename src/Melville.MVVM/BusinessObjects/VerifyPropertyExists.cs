using  System;
using System.Diagnostics;

namespace Melville.MVVM.BusinessObjects;

public static class VerifyPropertyExists
{
  [Conditional("DEBUG")]
  public static void InDebugBuilds(object obj, string property)
  {
    InReleaseBuilds(obj, property);
  }
  [Conditional("DEBUG")]
  public static void InDebugBuilds(object obj, string[] properties)
  {
    foreach (var property in properties)
    {
      InReleaseBuilds(obj, property);
    }
  }

  public static void InReleaseBuilds(object obj, string property)
  {
    if (!PropertyExists(obj, property))
    {
      throw new InvalidOperationException(String.Format("{0} does not have property {1}",
        obj.GetType().Name, property));
    }
  }
   
  private static bool PropertyExists(object obj, string property) => 
    String.IsNullOrEmpty(property) || obj.GetType().GetProperty(property) != null;
}