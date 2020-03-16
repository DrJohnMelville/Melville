using System.Diagnostics.CodeAnalysis;
using  System.Windows;

namespace Melville.MVVM.Wpf.MouseDragging.Drop
{
  public static class IDataObjectExtensions
  {
    [return:MaybeNull()]
    public static T GetData<T>(this IDataObject src) where T:class => (src.GetData(typeof(T)) as T)!;
    public static string? GetString(this IDataObject data) =>
      (data.GetData(DataFormats.UnicodeText) ?? data.GetData(DataFormats.Text)) as string;
  }
}