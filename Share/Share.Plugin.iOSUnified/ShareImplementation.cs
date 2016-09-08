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
        /// Gets or sets the ExcludedUIActivityTypes from sharing links or text
        /// </summary>
        public static List<NSString> ExcludedUIActivityTypes { get; set; }

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
                if (options == null)
                    options = new BrowserOptions();

                if ((options?.UseSafariWebViewController ?? false) && UIDevice.CurrentDevice.CheckSystemVersion(9, 0))
                {
                    var sfViewController = new SafariServices.SFSafariViewController(new NSUrl(url), options?.UseSafairReaderMode ?? false);
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
            var excluded = ExcludedUIActivityTypes == null ? null : ExcludedUIActivityTypes.ToArray();
            await Share(text, title, excluded);
        }

        /// <summary>
        /// Simply share text on compatible services
        /// </summary>
        /// <param name="text">Text to share</param>
        /// <param name="title">Title of popup on share (not included in message)</param>
        /// <param name="excludedActivityTypes">UIActivityType to exclude</param>
        /// <returns>awaitable Task</returns>
        public async Task Share(string text, string title = null, params NSString[] excludedActivityTypes)
        {
            await ShareInternal(title, text, null, excludedActivityTypes);
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
            var excluded = ExcludedUIActivityTypes == null ? null : ExcludedUIActivityTypes.ToArray();
            await ShareLink(url, message, title, excluded);
        }

        /// <summary>
        /// Share a link url with compatible services
        /// </summary>
        /// <param name="url">Link to share</param>
        /// <param name="message">Message to share</param>
        /// <param name="title">Title of the popup</param>
        /// <param name="excludedActivityTypes">UIActivityType to excluded</param>
        /// <returns>awaitable Task</returns>
        public async Task ShareLink(string url, string message = null, string title = null, params NSString[] excludedActivityTypes)
        {
            await ShareInternal(title, message, url, excludedActivityTypes);
        }

        /// <summary>
        /// Share data with compatible services
        /// </summary>
        /// <param name="title">Title to share</param>
        /// <param name="message">Message to share</param>
        /// <param name="url">Link to share</param>
        /// <param name="excludedActivityTypes">UIActivityType to excluded</param>
        /// <returns>awaitable Task</returns>
        async Task ShareInternal(string title, string message, string url, NSString[] excludedActivityTypes)
        {
            try
            {
                var items = new List<NSObject>();
                if (message != null)
                    items.Add(new ShareActivityItemSource(new NSString(message), title));
                if (url != null)
                    items.Add(new ShareActivityItemSource(NSUrl.FromString(url), title));

                var activityController = new UIActivityViewController(items.ToArray(), null);

                if (excludedActivityTypes != null && excludedActivityTypes.Length > 0)
                    activityController.ExcludedActivityTypes = excludedActivityTypes;

                var vc = GetVisibleViewController();

                if (UIDevice.CurrentDevice.CheckSystemVersion(8, 0))
                {
                    if (activityController.PopoverPresentationController != null)
                    {
                        activityController.PopoverPresentationController.SourceView = vc.View;
                    }
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

        /// <summary>
        /// Sets text on the clipboard
        /// </summary>
        /// <param name="text">Text to set</param>
        /// <param name="label">Label to display (not required, Android only)</param>
        /// <returns></returns>
        public Task<bool> SetClipboardText(string text, string label = null)
        {
            UIPasteboard.General.String = text;
            return Task.FromResult(true);
        }

        /// <summary>
        /// Gets if cliboard is supported
        /// </summary>
        public bool SupportsClipboard => true;

    }
}
