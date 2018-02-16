using CoreGraphics;
using Foundation;
using Plugin.Share.Abstractions;
using SafariServices;
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
        /// Gets or sets the UIActivityTypes that should not be displayed.
        /// </summary>
        public static List<NSString> ExcludedUIActivityTypes { get; set; } = new List<NSString> ();

        /// <summary>
        /// Open a browser to a specific url
        /// </summary>
        /// <param name="url">Url to open</param>
        /// <param name="options">Platform specific options</param>
        /// <returns>True if the operation was successful, false otherwise</returns>
        public async Task<bool> OpenBrowser(string url, BrowserOptions options = null)
        {
            try
            {
                if (options == null)
                    options = new BrowserOptions();

                if ((options?.UseSafariWebViewController ?? false) && UIDevice.CurrentDevice.CheckSystemVersion(9, 0))
                {
                    // create safari controller
                    var sfViewController = new SFSafariViewController(new NSUrl(url), options?.UseSafariReaderMode ?? false);

                    // apply custom tint colors
                    if (UIDevice.CurrentDevice.CheckSystemVersion(10, 0))
                    {
                        var barTintColor = options?.SafariBarTintColor;
                        if (barTintColor != null)
                            sfViewController.PreferredBarTintColor = barTintColor.ToUIColor();

                        var controlTintColor = options?.SafariControlTintColor;
                        if (controlTintColor != null)
                            sfViewController.PreferredControlTintColor = controlTintColor.ToUIColor();
                    }

                    // show safari controller
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
        /// Share a message with compatible services
        /// </summary>
        /// <param name="message">Message to share</param>
        /// <param name="options">Platform specific options</param>
        /// <returns>True if the operation was successful, false otherwise</returns>
        public Task<bool> Share(ShareMessage message, ShareOptions options = null)
        {
            return Share(message, options, null);
        }

        /// <summary>
        /// Share a message with compatible services
        /// </summary>
        /// <param name="message">Message to share</param>
        /// <param name="options">Platform specific options</param>
        /// <param name="excludedActivityTypes">UIActivityTypes that should not be displayed</param>
        /// <returns>True if the operation was successful, false otherwise</returns>
        private async Task<bool> Share(ShareMessage message, ShareOptions options = null, params NSString[] excludedActivityTypes)
        {
            if (message == null)
                throw new ArgumentNullException(nameof(message));

            try
            {
                // create activity items
                var items = new List<NSObject>();
                if (message.Text != null)
                    items.Add(new ShareActivityItemSource(new NSString(message.Text), message.Title));
                if (message.Url != null)
                    items.Add(new ShareActivityItemSource(NSUrl.FromString(message.Url), message.Title));

                // create activity controller
                var activityController = new UIActivityViewController(items.ToArray(), null);

                // set excluded activity types
                if (excludedActivityTypes == null)
                    // use ShareOptions.ExcludedUIActivityTypes
                    excludedActivityTypes = options?.ExcludedUIActivityTypes?.Select(x => GetUIActivityType(x)).Where(x => x != null).ToArray();

                if (excludedActivityTypes == null)
                    // use ShareImplementation.ExcludedUIActivityTypes
                    excludedActivityTypes = ExcludedUIActivityTypes?.ToArray();

                if (excludedActivityTypes != null && excludedActivityTypes.Length > 0)
                    activityController.ExcludedActivityTypes = excludedActivityTypes;

                // show activity controller
                var vc = GetVisibleViewController();

                if (UIDevice.CurrentDevice.CheckSystemVersion(8, 0))
                {
					if (activityController.PopoverPresentationController != null)
					{
						activityController.PopoverPresentationController.SourceView = vc.View;

						var rect = options?.PopoverAnchorRectangle;
						if (rect != null)
						{
							activityController.PopoverPresentationController.SourceRect = new CGRect(rect.X, rect.Y, rect.Width, rect.Height);
						}
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
			UIViewController viewController = null;
			var window = UIApplication.SharedApplication.KeyWindow;


			if (window != null && window.WindowLevel == UIWindowLevel.Normal)
				viewController = window.RootViewController;

			if (viewController == null)
			{
				window = UIApplication.SharedApplication.Windows.OrderByDescending(w => w.WindowLevel).FirstOrDefault(w => w.RootViewController != null && w.WindowLevel == UIWindowLevel.Normal);
				if (window == null)
					throw new InvalidOperationException("Could not find current view controller");
				else
					viewController = window.RootViewController;
			}

			while (viewController.PresentedViewController != null)
				viewController = viewController.PresentedViewController;


			return viewController;
		}

		/// <summary>
		/// Converts the <see cref="ShareUIActivityType"/> to its native representation.
		/// Returns null if the activity type is invalid or not supported on the current platform.
		/// </summary>
		/// <param name="type">The activity type</param>
		/// <returns>The native representation of the activity type or null</returns>
		NSString GetUIActivityType(ShareUIActivityType type)
        {
            switch (type)
            {
                case ShareUIActivityType.AssignToContact:
                    return UIActivityType.AssignToContact;
                case ShareUIActivityType.CopyToPasteboard:
                    return UIActivityType.CopyToPasteboard;
                case ShareUIActivityType.Mail:
                    return UIActivityType.Mail;
                case ShareUIActivityType.Message:
                    return UIActivityType.Message;
                case ShareUIActivityType.PostToFacebook:
                    return UIActivityType.PostToFacebook;
                case ShareUIActivityType.PostToTwitter:
                    return UIActivityType.PostToTwitter;
                case ShareUIActivityType.PostToWeibo:
                    return UIActivityType.PostToWeibo;
                case ShareUIActivityType.Print:
                    return UIActivityType.Print;
                case ShareUIActivityType.SaveToCameraRoll:
                    return UIActivityType.SaveToCameraRoll;

                case ShareUIActivityType.AddToReadingList:
                    return UIDevice.CurrentDevice.CheckSystemVersion(7, 0) ? UIActivityType.AddToReadingList : null;
                case ShareUIActivityType.AirDrop:
                    return UIDevice.CurrentDevice.CheckSystemVersion(7, 0) ? UIActivityType.AirDrop : null;
                case ShareUIActivityType.PostToFlickr:
                    return UIDevice.CurrentDevice.CheckSystemVersion(7, 0) ? UIActivityType.PostToFlickr : null;
                case ShareUIActivityType.PostToTencentWeibo:
                    return UIDevice.CurrentDevice.CheckSystemVersion(7, 0) ? UIActivityType.PostToTencentWeibo : null;
                case ShareUIActivityType.PostToVimeo:
                    return UIDevice.CurrentDevice.CheckSystemVersion(7, 0) ? UIActivityType.PostToVimeo : null;

                case ShareUIActivityType.OpenInIBooks:
                    return UIDevice.CurrentDevice.CheckSystemVersion(9, 0) ? UIActivityType.OpenInIBooks : null;

                default:
                    return null;
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
		/// Checks if the url can be opened
		/// </summary>
		/// <param name="url">Url to check</param>
		/// <returns>True if it can</returns>
		public bool CanOpenUrl(string url)
		{
			try
			{
				return UIApplication.SharedApplication.OpenUrl(NSUrl.FromString(url));
			}
			catch
			{
				return false;
			}
		}

		/// <summary>
		/// Gets if cliboard is supported
		/// </summary>
		public bool SupportsClipboard => true;
    }
}
