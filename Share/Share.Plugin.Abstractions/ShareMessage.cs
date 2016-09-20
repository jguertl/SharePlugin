using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Plugin.Share.Abstractions
{
    public class ShareMessage
    {
        /// <summary>
        /// Gets or sets the title of the message. Used as email subject if sharing with mail apps.
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Gets or sets the text of the message.
        /// </summary>
        public string Text { get; set; }

        /// <summary>
        /// Gets or sets the link to include with the message.
        /// </summary>
        public string Url { get; set; }
    }
}
