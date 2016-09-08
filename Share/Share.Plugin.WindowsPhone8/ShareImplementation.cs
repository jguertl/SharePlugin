using Microsoft.Phone.Tasks;
using Plugin.Share.Abstractions;
using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;

namespace Plugin.Share
{
    /// <summary>
    /// Implementation for Share
    /// </summary>
    public class ShareImplementation : IShare
    {
        /// <summary>
        /// For linker
        /// </summary>
        /// <returns></returns>
        public static async Task Init()
        {
            var test = DateTime.UtcNow;
        }

        /// <summary>
        /// Open a browser to a specific url
        /// </summary>
        /// <param name="url">Url to open</param>
        /// <param name="options">Platform specific options</param>
        /// <returns>awaitable Task</returns>
        public async Task OpenBrowser(string url, BrowserOptions options = null)
        {
            try
            {
                var webBrowserTask = new WebBrowserTask();

                webBrowserTask.Uri = new Uri(url, UriKind.Absolute);

                Deployment.Current.Dispatcher.BeginInvoke(webBrowserTask.Show);

            }
            catch (Exception ex)
            {
                Console.WriteLine("Unable to open browser task: " + ex.Message);
            }
        }

        /// <summary>
        /// Simply share text with compatible services
        /// </summary>
        /// <param name="text">Text to share</param>
        /// <param name="title">Title of the share popup on Android and Windows, email subject if sharing with mail apps</param>
        /// <returns>awaitable Task</returns>
        public async Task Share(string text, string title = null)
        {
            var task = new ShareStatusTask
            {
                Status = text ?? string.Empty
            };

            Deployment.Current.Dispatcher.BeginInvoke(task.Show);
        }

        /// <summary>
        /// Share a link url with compatible services
        /// </summary>
        /// <param name="url">Link to share</param>
        /// <param name="message">Message to include with the link</param>
        /// <param name="title">Title of the share popup on Android and Windows, email subject if sharing with mail apps</param>
        /// <returns>awaitable Task</returns>
        public async Task ShareLink(string url, string message = null, string title = null)
        {
            try
            {
                var task = new ShareLinkTask
                {
                    Message = message ?? string.Empty,
                    LinkUri = new Uri(url),
                    Title = title ?? string.Empty
                };

                Deployment.Current.Dispatcher.BeginInvoke(task.Show);
            }
            catch(Exception ex)
            {
                Console.WriteLine("Unable to create share link task: " + ex);
                return;
            }

        }

        /// <summary>
        /// Sets text on the clipboard
        /// </summary>
        /// <param name="text">Text to set</param>
        /// <param name="label">Label to display (not required, Android only)</param>
        /// <returns></returns>
        public Task<bool> SetClipboardText(string text, string label = null)
        {
            Clipboard.SetText(text);
            return Task.FromResult(true);
        }

        /// <summary>
        /// Gets if cliboard is supported
        /// </summary>
        public bool SupportsClipboard => true;
    }
}