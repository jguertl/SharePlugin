using Plugin.Share.Abstractions;
using System;
using System.Collections.Generic;
using System.Text;
using UIKit;

namespace Plugin.Share
{
    /// <summary>
    /// Extension class used for color conversion
    /// </summary>
    static class ShareColorExtensions
    {
        /// <summary>
        /// Convert <see cref="ShareColor"/> object to native color
        /// </summary>
        /// <param name="color">The color to convert</param>
        /// <returns>The converted color</returns>
        public static UIColor ToUIColor(this ShareColor color)
        {
            if (color == null)
                throw new ArgumentNullException(nameof(color));

            return new UIColor(
                color.R / 255f,
                color.G / 255f,
                color.B / 255f,
                color.A / 255f);
        }
    }
}
