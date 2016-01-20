using Plugin.Share.Abstractions;
using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Windows.ApplicationModel.DataTransfer;
using Windows.Foundation;

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
            await ShareLink(text, title, null);
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
            this.text = message ?? string.Empty;
            this.title = title ?? string.Empty;
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
#if WINDOWS_UWP
                request.Data.Properties.Title = title ?? Windows.ApplicationModel.Package.Current.DisplayName;
#elif WINDOWS_APP
                request.Data.Properties.Title = title ?? Windows.ApplicationModel.Package.Current.DisplayName;
#else
                request.Data.Properties.Title = title ?? string.Empty;

#endif

                if (!string.IsNullOrWhiteSpace(url))
                {
                  
                    request.Data.SetWebLink(new Uri(url));

                }
                request.Data.SetText(text ?? string.Empty);
            }
            catch(Exception ex)
            {
                Debug.WriteLine("Unable to share text: " + ex);
            }
        }

        public event EventHandler<ShareErrorEventArgs> ShareError;

        public Task ShareLocalFile(string localFilePath, string title = "")
        {
            throw new NotImplementedException();
        }

        public Task<bool> ShareRemoteFile(string fileUri, string fileName, string title = "")
        {
            throw new NotImplementedException();
        }
    }
}