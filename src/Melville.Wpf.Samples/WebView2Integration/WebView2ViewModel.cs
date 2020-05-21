using System;
using Microsoft.Web.WebView2.Wpf;

namespace Melville.Wpf.Samples.WebView2Integration
{
    public class WebView2ViewModel
    {
        public void WebViewLoad(WebView2 ctrl)
        {
            ctrl.Source = new Uri("https://www.microsoft.com");
        } 
    }
}