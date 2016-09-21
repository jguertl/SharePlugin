using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Plugin.Share.Abstractions
{
    /// <summary>
    /// RGB values of 0-255 for shared
    /// </summary>
    public class ShareColor
    {
        /// <summary>
        /// Alpha 0-255
        /// </summary>
        public int A { get; set; }
        /// <summary>
        /// Red 0-255
        /// </summary>
        public int R { get; set; }
        /// <summary>
        /// Green 0-255
        /// </summary>
        public int G { get; set; }
        /// <summary>
        /// Blue 0-255
        /// </summary>
        public int B { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ShareColor"/> class with default values.
        /// </summary>
        public ShareColor()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ShareColor"/> class with the specified values. Alpha is set to 255 (fully opaque).
        /// </summary>
        /// <param name="r">Red 0-255</param>
        /// <param name="g">Green 0-255</param>
        /// <param name="b">Blue 0-255</param>
        public ShareColor(int r, int g, int b) : this(r, g, b, 255)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ShareColor"/> class with the specified values.
        /// </summary>
        /// <param name="r">Red 0-255</param>
        /// <param name="g">Green 0-255</param>
        /// <param name="b">Blue 0-255</param>
        /// <param name="a">Alpha 0-255</param>
        public ShareColor(int r, int g, int b, int a)
        {
            A = a;
            R = r;
            G = g;
            B = b;
        }
    }
}
