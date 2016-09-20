using Foundation;
using Plugin.Share.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UIKit;

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

        static ShareImplementation()
        {
            ExcludedUIActivityTypes = new List<NSString> { UIActivityType.PostToFacebook };
        }

        /// <summary>
        /// Gets or sets the UIActivityTypes that should not be displayed.
        /// </summary>
        public static List<NSString> ExcludedUIActivityTypes { get; set; }

        /// <summary>
        /// Open a browser to a specific url
        /// </summary>
        /// <param name="url">Url to open</param>
        /// <param name="options">Platform specific options</param>
        /// <returns>awaitable Task</returns>
        public async Task<bool> OpenBrowser(string url, BrowserOptions options = null)
        {
            try
            {
                if (options == null)
                    options = new BrowserOptions();

                if ((options?.UseSafariWebViewController ?? false) && UIDevice.CurrentDevice.CheckSystemVersion(9, 0))
                {
                    var sfViewController = new SafariServices.SFSafariViewController(new NSUrl(url), options?.UseSafariReaderMode ?? false);
                    var vc = GetVisibleViewController();

                    if (sfViewController.PopoverPresentationController != null)
                    {
                        sfViewController.PopoverPresentationController.SourceView = vc.View;
                    }

                    await vc.PresentViewControllerAsync(sfViewController, true);
                }
                else
                {
                    UIApplication.SharedApplication.OpenUrl(new NSUrl(url));
                }

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Unable to open browser: " + ex.Message);
                return false;
            }
        }

        /// <summary>
        /// Simply share text with compatible services
        /// </summary>
        /// <param name="text">Text to share</param>
        /// <param name="title">Title of the share popup on Android and Windows, email subject if sharing with mail apps</param>
        /// <returns>awaitable Task</returns>
        [Obsolete("Use Share(ShareMessage, ShareOptions)")]
        public Task<bool> Share(string text, string title = null)
        {
            var shareMessage = new ShareMessage();
            shareMessage.Title = title;
            shareMessage.Text = text;

            return Share(shareMessage);
        }

        /// <summary>
        /// Simply share text with compatible services
        /// </summary>
        /// <param name="text">Text to share</param>
        /// <param name="title">Title of the share popup on Android and Windows, email subject if sharing with mail apps</param>
        /// <param name="excludedActivityTypes">UIActivityType to exclude</param>
        /// <returns>awaitable Task</returns>
        [Obsolete("Use Share(ShareMessage, ShareOptions)")]
        public Task<bool> Share(string text, string title = null, params NSString[] excludedActivityTypes)
        {
            var shareMessage = new ShareMessage();
            shareMessage.Title = title;
            shareMessage.Text = text;

            var shareOptions = new ShareOptions();
            shareOptions.ExcludedUIActivityTypes = excludedActivityTypes?.Select(x => (string)x).ToArray();

            return Share(shareMessage, shareOptions);
        }

        /// <summary>
        /// Share a link url with compatible services
        /// </summary>
        /// <param name="url">Link to share</param>
        /// <param name="message">Message to include with the link</param>
        /// <param name="title">Title of the share popup on Android and Windows, email subject if sharing with mail apps</param>
        /// <returns>awaitable Task</returns>
        [Obsolete("Use Share(ShareMessage, ShareOptions)")]
        public Task<bool> ShareLink(string url, string message = null, string title = null)
        {
            var shareMessage = new ShareMessage();
            shareMessage.Title = title;
            shareMessage.Text = message;
            shareMessage.Url = url;

            return Share(shareMessage);
        }

        /// <summary>
        /// Share a link url with compatible services
        /// </summary>
        /// <param name="url">Link to share</param>
        /// <param name="message">Message to include with the link</param>
        /// <param name="title">Title of the share popup on Android and Windows, email subject if sharing with mail apps</param>
        /// <param name="excludedActivityTypes">UIActivityType to exclude</param>
        /// <returns>awaitable Task</returns>
        [Obsolete("Use Share(ShareMessage, ShareOptions)")]
        public Task<bool> ShareLink(string url, string message = null, string title = null, params NSString[] excludedActivityTypes)
        {
            var shareMessage = new ShareMessage();
            shareMessage.Title = title;
            shareMessage.Text = message;
            shareMessage.Url = url;

            var shareOptions = new ShareOptions();
            shareOptions.ExcludedUIActivityTypes = excludedActivityTypes?.Select(x => (string)x).ToArray();

            return Share(shareMessage, shareOptions);
        }

        /// <summary>
        /// Share a message with compatible services
        /// </summary>
        /// <param name="message">Message to share</param>
        /// <param name="options">Platform specific options</param>
        /// <returns>awaitable Task</returns>
        public async Task<bool> Share(ShareMessage message, ShareOptions options = null)
        {
            if (message == null)
                throw new ArgumentNullException(nameof(message));

            try
            {
                // create activity items
                var items = new List<NSObject>();
                if (message.Text != null)
                    items.Add(new ShareActivityItemSource((NSString)message.Text, message.Title));
                if (message.Url != null)
                    items.Add(new ShareActivityItemSource(NSUrl.FromString(message.Url), message.Title));

                // create activity controller
                var activityController = new UIActivityViewController(items.ToArray(), null);

                // set excluded activity types
                var excludedActivityTypes = options?.ExcludedUIActivityTypes?.Select(x => (NSString)x).ToArray() ?? ExcludedUIActivityTypes?.ToArray();
                if (excludedActivityTypes != null && excludedActivityTypes.Length > 0)
                    activityController.ExcludedActivityTypes = excludedActivityTypes;

                // show activity controller
                var vc = GetVisibleViewController();

                if (UIDevice.CurrentDevice.CheckSystemVersion(8, 0))
                {
                    if (activityController.PopoverPresentationController != null)
                    {
                        activityController.PopoverPresentationController.SourceView = vc.View;
                    }
                }

                await vc.PresentViewControllerAsync(activityController, true);

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Unable to share: " + ex.Message);
                return false;
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

        /// <summary>
        /// Sets text on the clipboard
        /// </summary>
        /// <param name="text">Text to set</param>
        /// <param name="label">Label to display (not required, Android only)</param>
        /// <returns></returns>
        public Task<bool> SetClipboardText(string text, string label = null)
        {
            try
            {
                UIPasteboard.General.String = text;

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
