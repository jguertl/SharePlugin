using Foundation;
using Plugin.Share.Abstractions;
using Social;
using System;
using System.Threading.Tasks;
using UIKit;

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
                var vc = GetVisibleViewController();

                if (activityController.PopoverPresentationController != null)
                {
                    activityController.PopoverPresentationController.SourceView = vc.View;
                }

                await vc.PresentViewControllerAsync(activityController, true);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Unable to share text" + ex.Message);
            }
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
                var vc = GetVisibleViewController();

                if (activityController.PopoverPresentationController != null)
                {
                    activityController.PopoverPresentationController.SourceView = vc.View;
                }

                await vc.PresentViewControllerAsync(activityController, true);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Unable to share text" + ex.Message);
            }
        }

        /// <summary>
        /// Gets the visible view controller.
        /// </summary>
        /// <returns>The visible view controller.</returns>
        UIViewController GetVisibleViewController()
        {
            var rootController = UIApplication.SharedApplication.KeyWindow.RootViewController;

            if (rootController.PresentedViewController == null)
                return rootController;

            if (rootController.PresentedViewController is UINavigationController)
            {
                return ((UINavigationController)rootController.PresentedViewController).TopViewController;
            }

            if (rootController.PresentedViewController is UITabBarController)
            {
                return ((UITabBarController)rootController.PresentedViewController).SelectedViewController;
            }

            return rootController.PresentedViewController;
        }
       
    }
}