using Share.Plugin.Abstractions;
using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Windows.ApplicationModel.DataTransfer;
using Windows.Foundation;

namespace Share.Plugin
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
                await Windows.System.Launcher.LaunchUriAsync(new Uri(url));
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Unable to open browser task: " + ex.Message);
            }
        }

        string text, title, url;
        DataTransferManager dataTransferManager;
        /// <summary>
        /// Simply share text on compatible services
        /// </summary>
        /// <param name="text">Text to share</param>
        /// <param name="title">Title of popup on share (not included in message)</param>
        /// <returns>awaitable Task</returns>
        public async Task Share(string text, string title = null)
        {
            ShareLink(text, title, null);
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
            this.text = text;
            this.title = title;
            this.url = url;
            if (dataTransferManager == null)
            {
                dataTransferManager = DataTransferManager.GetForCurrentView();
                dataTransferManager.DataRequested += new TypedEventHandler<DataTransferManager, DataRequestedEventArgs>(this.ShareTextHandler);
            }
            DataTransferManager.ShowShareUI();
        }

        private void ShareTextHandler(DataTransferManager sender, DataRequestedEventArgs e)
        {
            try
            {
                DataRequest request = e.Request;
                // The Title is mandatory
                request.Data.Properties.Title = title ?? string.Empty;
                request.Data.Properties.Description = text ?? string.Empty;
                if (url != null)
                {
                    request.Data.Properties.ContentSourceWebLink = new Uri(url);
                    request.Data.SetUri(new Uri(url));
                }
                request.Data.SetText(title ?? string.Empty);
            }
            catch(Exception ex)
            {
                Debug.WriteLine("Unable to share text: " + ex);
            }
        }
    }
}