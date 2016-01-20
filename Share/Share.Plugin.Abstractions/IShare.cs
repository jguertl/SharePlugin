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
		/// Raised when a sharing error occurs.
		/// </summary>
		event EventHandler<ShareErrorEventArgs> ShareError;

        /// <summary>
        /// Simply share text on compatible services
        /// </summary>
        /// <param name="text">Text to share</param>
        /// <param name="title">Title of popup on share (not included in message)</param>
        /// <returns>awaitable Task</returns>
        Task Share(string text, string title = null);

        /// <summary>
        /// Simply share a local file on compatible services
        /// </summary>
		/// <param name="localFilePath">path to local file</param>
        /// <param name="title">Title of popup on share (not included in message)</param>
        /// <returns>awaitable Task</returns>
		Task ShareLocalFile(string localFilePath, string title = "");

		/// <summary>
		/// Simply share a file from an external resource on compatible services
		/// </summary>
		/// <param name="fileUri">uir to external file</param>
		/// <param name="fileName">name of the file</param>
		/// <param name="title">Title of popup on share (not included in message)</param>
		/// <returns>awaitable Task<bool></returns>
		Task<bool> ShareExternalFile(string fileUri, string fileName, string title = "");

        /// <summary>
        /// Open a browser to a specific url
        /// </summary>
        /// <param name="url">Url to open</param>
        /// <returns>awaitable Task</returns>
        Task OpenBrowser(string url);

        /// <summary>
        /// Share a link url with compatible services
        /// </summary>
        /// <param name="url">Link to share</param>
        /// <param name="message">Message to share</param>
        /// <param name="title">Title of the popup</param>
        /// <returns>awaitable Task</returns>
        Task ShareLink(string url, string message = null, string title = null);

    }
}
