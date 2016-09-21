using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.Graphics;
using Plugin.Share.Abstractions;

namespace Plugin.Share
{
    static class ShareColorExtensions
    {
        public static Color ToNativeColor(this ShareColor color)
        {
            if (color == null)
                throw new ArgumentNullException(nameof(color));

            return new Color(
                color.R,
                color.G,
                color.B,
                color.A);
        }
    }
}