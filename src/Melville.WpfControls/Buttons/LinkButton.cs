using  System.Windows;
using System.Windows.Controls;

namespace Melville.WpfControls.Buttons
{
  public class LinkButton : Button
  {
    static LinkButton()
    {
      DefaultStyleKeyProperty.OverrideMetadata(typeof(LinkButton), new FrameworkPropertyMetadata(typeof(LinkButton)));
    }
  }
}