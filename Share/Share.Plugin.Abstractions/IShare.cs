using System;
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
    /// <summary>
    /// Interface for Share
    /// </summary>
    public interface IShare
    {
        /// <summary>
        /// Simply share text on compatible services
        /// </summary>
        /// <param name="text">Text to share</param>
        /// <param name="title">Title of popup on share (not included in message)</param>
        /// <returns>awaitable Task</returns>
        Task Share(string text, string title = null);

        /// <summary>
        /// Open a browser to a specific url
        /// </summary>
        /// <param name="url">Url to open</param>
        /// <param name="readerMode">If in reader mode if available</param>
        /// <param name="showTitle">Show title if avaialble to set</param>
        /// <param name="toolbarColor">Color to set of the  toolbar if avaialble</param>
        /// <returns>awaitable Task</returns>
        Task OpenBrowser(string url, bool showTitle = false, bool readerMode = false, ShareColor toolbarColor = null);

        /// <summary>
        /// Share a link url with compatible services
        /// </summary>
        /// <param name="url">Link to share</param>
        /// <param name="message">Message to share</param>
        /// <param name="title">Title of the popup</param>
        /// <returns>awaitable Task</returns>
        Task ShareLink(string url, string message = null, string title = null);

        /// <summary>
        /// Sets text on the clipboard
        /// </summary>
        /// <param name="text">Text to set</param>
        /// <param name="label">Label to dislay (no required, Android only)</param>
        /// <returns></returns>
        Task<bool> SetClipboardText(string text, string label = null);

        /// <summary>
        /// Gets if clipboard is supported
        /// </summary>
        bool SupportsClipboard { get; }
    }
}
