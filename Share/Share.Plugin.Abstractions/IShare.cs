using System;
using System.Threading.Tasks;

namespace Plugin.Share.Abstractions
{

    /// <summary>
    /// Interface for Share
    /// </summary>
    public interface IShare
    {
        /// <summary>
        /// Open a browser to a specific url
        /// </summary>
        /// <param name="url">Url to open</param>
        /// <param name="options">Platform specific options</param>
        /// <returns>True if the operation was successful, false otherwise</returns>
        Task<bool> OpenBrowser(string url, BrowserOptions options = null);

		/// <summary>
		/// Checks if the url can be opened
		/// </summary>
		/// <param name="url">Url to check</param>
		/// <returns>True if it can</returns>
		bool CanOpenUrl(string url);

        /// <summary>
        /// Share a message with compatible services
        /// </summary>
        /// <param name="message">Message to share</param>
        /// <param name="options">Platform specific options</param>
        /// <returns>True if the operation was successful, false otherwise</returns>
        Task<bool> Share(ShareMessage message, ShareOptions options = null);

        /// <summary>
        /// Sets text of the clipboard
        /// </summary>
        /// <param name="text">Text to set</param>
        /// <param name="label">Label to display (not required, Android only)</param>
        /// <returns>True if the operation was successful, false otherwise</returns>
        Task<bool> SetClipboardText(string text, string label = null);

        /// <summary>
        /// Gets if clipboard is supported
        /// </summary>
        bool SupportsClipboard { get; }
    }
}
