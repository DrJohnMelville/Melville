using  System;
using System.IO;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Melville.MVVM.Wpf.Clipboards
{
  public interface IReadFromClipboard
  {
    /// <summary>
    /// Erase content from the system clipboard.
    /// </summary>
    void Clear();

    /// <summary>Queries the Clipboard for the presence of data in a specified data format.</summary>
    /// <param name="format">The format of the data to look for. See <see cref="T:System.Windows.DataFormats" /> for predefined formats. </param>
    /// <returns>
    /// <see langword="true" /> if data in the specified format is available on the Clipboard; otherwise, <see langword="false" />. See Remarks.</returns>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="format" /> is <see langword="null" />.</exception>
    bool ContainsData(string format);

    /// <summary>Queries the Clipboard for the presence of data in a text data format.</summary>
    /// <param name="format">A member of the <see cref="T:System.Windows.TextDataFormat" /> enumeration that specifies the text data format to query for.</param>
    /// <returns>
    /// <see langword="true" /> if the Clipboard contains data in the specified text data format; otherwise, <see langword="false" />.</returns>
    /// <exception cref="T:System.ComponentModel.InvalidEnumArgumentException">
    /// <paramref name="format" /> does not specify a valid member of <see cref="T:System.Windows.TextDataFormat" />.</exception>
    bool ContainsText(TextDataFormat format = TextDataFormat.UnicodeText);

    /// <summary>Permanently adds the data that is on the <see cref="T:System.Windows.Clipboard" /> so that it is available after the data's original application closes.</summary>
    void Flush();

    /// <summary>Retrieves data in a specified format from the Clipboard.</summary>
    /// <param name="format">A string that specifies the format of the data to retrieve. For a set of predefined data formats, see the <see cref="T:System.Windows.DataFormats" /> class.</param>
    /// <returns>An object that contains the data in the specified format, or <see langword="null" /> if the data is unavailable in the specified format.</returns>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="format" /> is <see langword="null" />.</exception>
    object GetData(string format);

    /// <summary>Returns a string containing text data on the Clipboard. </summary>
    /// <param name="format">A member of <see cref="T:System.Windows.TextDataFormat" /> that specifies the text data format to retrieve.</param>
    /// <returns>A string containing text data in the specified data format, or an empty string if no corresponding text data is available.</returns>
    string GetText(TextDataFormat format = TextDataFormat.UnicodeText);

    /// <summary>Compares a specified data object to the contents of the Clipboard.</summary>
    /// <param name="data">A data object to compare to the contents of the system Clipboard.</param>
    /// <returns>
    /// <see langword="true" /> if the specified data object matches what is on the system Clipboard; otherwise, <see langword="false" />.</returns>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="data" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.Runtime.InteropServices.ExternalException">An error occurred when accessing the Clipboard. The exception details will include an <see langword="HResult" /> that identifies the specific error; see <see cref="P:System.Runtime.InteropServices.ErrorWrapper.ErrorCode" />.</exception>
    bool IsCurrent(IDataObject data);

    /// <summary>Returns a data object that represents the entire contents of the Clipboard.</summary>
    /// <returns>A data object that enables access to the entire contents of the system Clipboard, or <see langword="null" /> if there is no data on the Clipboard.</returns>
    IDataObject GetDataObject();


  }

  public static class ReadFromClipboardOperations
  {
    public static bool ContainsAudio(this IReadFromClipboard clip) => clip.ContainsData(DataFormats.WaveAudio);
    public static bool ContainsFileDropList(this IReadFromClipboard clip) => clip.ContainsData(DataFormats.FileDrop);
    public static bool ContainsImage(this IReadFromClipboard clip) => clip.ContainsData(DataFormats.Bitmap);

    public static Stream? GetAudio(this IReadFromClipboard clip) => clip.GetData(DataFormats.WaveAudio) as Stream;
    public static string[]? GetFileDrop(this IReadFromClipboard clip) => clip.GetData(DataFormats.FileDrop) as string[];

    public static BitmapSource? GetImage(this IReadFromClipboard clip) =>
      clip.GetData(DataFormats.Bitmap) as BitmapSource;

  }
  public class ReadFromClipboard : IReadFromClipboard
  {
    public void Clear() => Clipboard.Clear();
    public bool ContainsData(string format) => Clipboard.ContainsData(format);

    public bool ContainsText(TextDataFormat format = TextDataFormat.UnicodeText) =>
      Clipboard.ContainsText(format);

    public void Flush() => Clipboard.Flush();
    public object GetData(string format) => Clipboard.GetData(format);
    public string GetText(TextDataFormat format = TextDataFormat.UnicodeText) => Clipboard.GetText(format);
    public bool IsCurrent(IDataObject data) => Clipboard.IsCurrent(data);
    public IDataObject GetDataObject() => Clipboard.GetDataObject();
  }

  

}