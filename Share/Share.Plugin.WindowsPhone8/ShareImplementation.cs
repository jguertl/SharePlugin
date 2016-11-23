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
        /// Open a browser to a specific url
        /// </summary>
        /// <param name="url">Url to open</param>
        /// <param name="options">Platform specific options</param>
        /// <returns>True if the operation was successful, false otherwise</returns>
        public Task<bool> OpenBrowser(string url, BrowserOptions options = null)
        {
            try
            {
                var webBrowserTask = new WebBrowserTask();

                webBrowserTask.Uri = new Uri(url, UriKind.Absolute);

                Deployment.Current.Dispatcher.BeginInvoke(webBrowserTask.Show);

                return Task.FromResult(true);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Unable to open browser: " + ex.Message);
                return Task.FromResult(false);
            }
        }


        /// <summary>
        /// Share a message with compatible services
        /// </summary>
        /// <param name="message">Message to share</param>
        /// <param name="options">Platform specific options</param>
        /// <returns>True if the operation was successful, false otherwise</returns>
        public Task<bool> Share(ShareMessage message, ShareOptions options = null)
        {
            if (message == null)
                throw new ArgumentNullException(nameof(message));

            try
            {
                if (message.Url == null)
                {
                    // ShareStatusTask
                    var task = new ShareStatusTask
                    {
                        Status = message.Text ?? string.Empty
                    };

                    Deployment.Current.Dispatcher.BeginInvoke(task.Show);
                }
                else
                {
                    // ShareLinkTask
                    var task = new ShareLinkTask
                    {
                        Title = message.Title ?? string.Empty,
                        Message = message.Text ?? string.Empty,
                        LinkUri = new Uri(message.Url)
                    };

                    Deployment.Current.Dispatcher.BeginInvoke(task.Show);
                }

                return Task.FromResult(true);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Unable to share: " + ex.Message);
                return Task.FromResult(false);
            }
        }

        /// <summary>
        /// Sets text of the clipboard
        /// </summary>
        /// <param name="text">Text to set</param>
        /// <param name="label">Label to display (not required, Android only)</param>
        /// <returns>True if the operation was successful, false otherwise</returns>
        public Task<bool> SetClipboardText(string text, string label = null)
        {
            try
            {
                Clipboard.SetText(text);

                return Task.FromResult(true);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Unable to copy to clipboard: " + ex.Message);
                return Task.FromResult(false);
            }
        }

        /// <summary>
        /// Gets if cliboard is supported
        /// </summary>
        public bool SupportsClipboard => true;
    }
}