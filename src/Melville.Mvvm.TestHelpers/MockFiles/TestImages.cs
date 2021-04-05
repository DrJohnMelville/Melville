using Melville.FileSystem.FileSystem;

#nullable disable warnings
namespace Melville.Mvvm.TestHelpers.MockFiles
{
    /// <summary>
    /// This class has a number of embedded image files which are, at times useful for testing.
    /// </summary>
    public static class TestImages
    {
        public static IFile Jpeg() => FromString("Test.jpg");
        public static IFile HasGpsInfo() => FromString("HasGpsInfo.jpg");
        public static IFile Bmp() => FromString("Test.bmp");
        public static IFile Png() => FromString("Test.png");
        public static IFile Tif() => FromString("Test.tif");
        public static IFile Gif() => FromString("Test.gif");
        public static IFile Xaml() => FromString("Test.xamlText", "Test.xaml");

        public static IFile FromString(string name, string externalName = null)
        {
            return new MemoryFile(externalName??name, name, "Melville.Mvvm.TestHelpers.MockFiles.", typeof(TestImages).Assembly);
        }
    }

    public static class TestAudio
    {
      public static IFile Mp3() => TestImages.FromString("sample.mp3");
    }
}