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
    }
}
