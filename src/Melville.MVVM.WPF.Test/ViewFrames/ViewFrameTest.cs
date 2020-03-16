#nullable disable warnings
using  System;
using System.Windows;
using System.Windows.Controls;
using Melville.MVVM.Wpf.DiParameterSources;
using Melville.MVVM.Wpf.ViewFrames;
using Moq;
using Xunit;

namespace Melville.MVVM.WPF.Test.ViewFrames
{
  public sealed class ViewFrameTest
  {
    [StaFact]
    public void GenerateChildNull()
    {
      var sut = new ViewFrame();
#if DEBUG
      AssertTextBox(sut, "Content Never Set");
#else
      Assert.Null(sut.Child);
#endif
      sut.Content = "StringContent";
      AssertTextBox(sut, "StringContent");
      sut.Content = new Exception();
      AssertTextBox(sut, "System.Exception does not contain \"ViewModel\".");
      sut.Content = null;
      AssertTextBox(sut, "<Null>");
    }


    private static void AssertTextBox(ViewFrame innerSut, string text)
    {
      Assert.IsType<TextBlock>(innerSut.Child);
      Assert.Equal(text, ((TextBlock) innerSut.Child).Text);
    }

    public class AViewModel
    {
    }
    public class BViewModel
    {
    }

    public class AView: FrameworkElement { }

    [StaFact]
    public void FindViewModel()
    {
      var data = new AViewModel();
      var sut = new ViewFrame();

      var creator = new Mock<IDIIntegration>();
      creator.Setup(i => i.Get(typeof(AView))).Returns(new AView());
      DiIntegration.SetContainer(sut, creator.Object);

      sut.Content = data;
      
      Assert.Equal(data, sut.Content);
      
      Assert.IsType<AView>(sut.Child);
      Assert.Equal(data, (sut.Child as FrameworkElement)?.DataContext);
    }

    [StaFact]
    public void CannotFindClass()
    {
      var sut = new ViewFrame {Content = new BViewModel()};
      AssertTextBox(sut, "Could Not Find Type Melville.MVVM.WPF.Test.ViewFrames.ViewFrameTest+BView.");
    }
  }
}