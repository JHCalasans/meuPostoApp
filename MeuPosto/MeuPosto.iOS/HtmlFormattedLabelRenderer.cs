//using System;
//using System.Collections.Generic;
//using System.Text;
//using Foundation;
//using MeuPosto.iOS;
//using MeuPosto.Renderers;
//using Xamarin.Forms;
//using Xamarin.Forms.Platform.iOS;


//[assembly: ExportRenderer(typeof(HtmlFormattedLabel), typeof(HtmlFormattedLabelRenderer))]
//namespace MeuPosto.iOS
//{
//    public class HtmlFormattedLabelRenderer : LabelRenderer
//    {
//        protected override void OnElementChanged(ElementChangedEventArgs<Label> e)
//        {
//            base.OnElementChanged(e);

//            var view = (HtmlFormattedLabel)Element;
//            if (view == null) return;

//            var attr = new NSAttributedStringDocumentAttributes();
//            var nsError = new NSError();
//            attr.DocumentType = NSDocumentType.HTML;

//            Control.AttributedText = new NSAttributedString(view.Text, attr, ref nsError);
//        }
//    }
//}
