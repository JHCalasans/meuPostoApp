using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using MeuPosto.Droid;
using MeuPosto.Renderers;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(CustomWebView), typeof(CustomWebViewRenderer))]
namespace MeuPosto.Droid
{
    public class CustomWebViewRenderer : WebViewRenderer
    {
        protected override void OnElementChanged(ElementChangedEventArgs<WebView> e)
        {
            base.OnElementChanged(e);

            if (e.NewElement != null)
            {
                var customWebView = Element as CustomWebView;
                Control.Settings.AllowUniversalAccessFromFileURLs = true;

                var documentsPath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments);
                //var filePath = Path.Combine(documentsPath, filename);
                //File.WriteAllBytes(filePath, bytes);

                Control.LoadUrl(string.Format("file:///android_asset/pdfjs/web/viewer.html?file={0}", "/data/data/br.com.meuposto/files/TesteNovo.pdf"));
                   
               // Control.LoadUrl(string.Format("file:///android_asset/pdfjs/web/viewer.html?file={0}", "/storage/emulated/0/TesteNovo.pdf"));
            }
        }
    }
}