﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Text;
using Android.Views;
using Android.Widget;
using MeuPosto.Droid;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

//[assembly: ExportRenderer(typeof(HtmlFormattedLabel), typeof(HtmlFormattedLabelRenderer))]
//namespace MeuPosto.Droid
//{
//    public class HtmlFormattedLabelRenderer : LabelRenderer
//    {
//        protected override void OnElementChanged(ElementChangedEventArgs<Label> e)
//        {
//            base.OnElementChanged(e);

//            var view = (HtmlFormattedLabel)Element;
//            if (view == null) return;

//            Control.SetText(Html.FromHtml(view.Text.ToString()), TextView.BufferType.Spannable);
//        }
//    }
//}