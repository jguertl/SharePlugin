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
        public static async Task Init()
        {
            var test = DateTime.UtcNow;
        } 
        /// <summary>
        /// Open a browser to a specific url
        /// </summary>
        /// <param name="url">Url to open</param>
        /// <returns>awaitable Task</returns>
        public async Task OpenBrowser(string url)
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
        /// Simply share text on compatible services
        /// </summary>
        /// <param name="text">Text to share</param>
        /// <param name="title">Title of popup on share (not included in message)</param>
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
        /// <param name="message">Message to share</param>
        /// <param name="title">Title of the popup</param>
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
    }
}