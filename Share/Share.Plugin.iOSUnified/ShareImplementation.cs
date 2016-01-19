using CoreGraphics;
using Foundation;
using Plugin.Share.Abstractions;
using Social;
using System;
using System.Net;
using System.Threading.Tasks;
using UIKit;

namespace Plugin.Share
{
    /// <summary>
    /// Implementation for Share
    /// </summary>
    public class ShareImplementation : IShare
    {
        public object File { get; private set; }
		public event EventHandler ShareCompleted;
		public event EventHandler ShareError;

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
                UIApplication.SharedApplication.OpenUrl(new NSUrl(url));
            }
            catch (Exception ex)
            {
                Console.WriteLine("Unable to open browser: " + ex.Message);
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
            try
            {
                var items = new NSObject[] { new NSString(text ?? string.Empty) };
                var activityController = new UIActivityViewController(items, null);
                if (activityController.PopoverPresentationController != null)
                {
                    activityController.PopoverPresentationController.SourceView =
                      UIApplication.SharedApplication.KeyWindow.RootViewController.ChildViewControllers != null
                        ? UIApplication.SharedApplication.KeyWindow.RootViewController.ChildViewControllers[0].View
                        : UIApplication.SharedApplication.KeyWindow.RootViewController.View;
                }
                var vc = UIApplication.SharedApplication.KeyWindow.RootViewController;

                await vc.PresentViewControllerAsync(activityController, true);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Unable to share text" + ex.Message);
            }
        }

		public async Task ShareLocalFile(string localFilePath, string title = "")
        {
            var _openInWindow = UIDocumentInteractionController.FromUrl(NSUrl.FromFilename(localFilePath.Trim()));
            _openInWindow.PresentOpenInMenu(new CGRect(0, 260, 320, 320), UIApplication.SharedApplication.KeyWindow.RootViewController.View, false);

			if (ShareCompleted != null)
				ShareCompleted (this, null);
        }

		public async Task ShareExternalFile(string fileUri, string fileName)
        {
            var uri = new System.Uri(fileUri);

            var webClient = new WebClient();
            webClient.DownloadDataCompleted += (s, e) => {

				if(e.Error != null)
				{
					if(ShareError != null)
						ShareError(s, e);

					if (ShareCompleted != null)
						ShareCompleted (this, null);
					
					return;
				}

                var bytes = e.Result; // get the downloaded data
                string localFolder = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);
                string localPath = System.IO.Path.Combine(localFolder, fileName);
                System.IO.File.WriteAllBytes(localPath, bytes); // write to local storage

                ShareLocalFile(localPath);
            };

            webClient.DownloadDataAsync(uri);
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
                var items = new NSObject[] { new NSString(message ?? string.Empty), NSUrl.FromString(url) };
                var activityController = new UIActivityViewController(items, null);
                if (activityController.PopoverPresentationController != null)
                {
                    activityController.PopoverPresentationController.SourceView =
                      UIApplication.SharedApplication.KeyWindow.RootViewController.ChildViewControllers != null
                        ? UIApplication.SharedApplication.KeyWindow.RootViewController.ChildViewControllers[0].View
                        : UIApplication.SharedApplication.KeyWindow.RootViewController.View;
                }
                var vc = UIApplication.SharedApplication.KeyWindow.RootViewController;

                await vc.PresentViewControllerAsync(activityController, true);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Unable to share text" + ex.Message);
            }
        }
       
    }
}