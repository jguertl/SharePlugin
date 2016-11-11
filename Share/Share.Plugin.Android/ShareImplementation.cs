using Android.App;
using Android.Content;
using Android.Support.CustomTabs;
using Plugin.CurrentActivity;
using Plugin.Share.Abstractions;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Plugin.Share
{
    /// <summary>
    /// Implementation for Feature
    /// </summary>
    public class ShareImplementation : IShare
    {
        /// <summary>
        /// For linker
        /// </summary>
        /// <returns></returns>
        [Obsolete("Calling Init() is no longer required")]
        public static async Task Init()
        {
            var test = DateTime.UtcNow;
        }

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
                if (options == null)
                    options = new BrowserOptions();

                if (CrossCurrentActivity.Current.Activity == null)
                {
                    var intent = new Intent(Intent.ActionView);
                    intent.SetData(Android.Net.Uri.Parse(url));

                    intent.SetFlags(ActivityFlags.ClearTop);
                    intent.SetFlags(ActivityFlags.NewTask);
                    Application.Context.StartActivity(intent);
                }
                else
                {
                    var tabsBuilder = new CustomTabsIntent.Builder();
                    tabsBuilder.SetShowTitle(options?.ChromeShowTitle ?? false);

                    var toolbarColor = options?.ChromeToolbarColor;
                    if (toolbarColor != null)
                        tabsBuilder.SetToolbarColor(toolbarColor.ToNativeColor());

                    var intent = tabsBuilder.Build();
                    intent.LaunchUrl(CrossCurrentActivity.Current.Activity, Android.Net.Uri.Parse(url));
                }

                return Task.FromResult(true);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Unable to open browser: " + ex.Message);
                return Task.FromResult(false);
            }
        }

        /// <summary>
        /// Simply share text with compatible services
        /// </summary>
        /// <param name="text">Text to share</param>
        /// <param name="title">Title of the share popup on Android and Windows, email subject if sharing with mail apps</param>
        /// <returns>True if the operation was successful, false otherwise</returns>
        [Obsolete("Use Share(ShareMessage, ShareOptions)")]
        public Task<bool> Share(string text, string title = null)
        {
            var shareMessage = new ShareMessage();
            shareMessage.Title = title;
            shareMessage.Text = text;

            var shareOptions = new ShareOptions();
            shareOptions.ChooserTitle = title;

            return Share(shareMessage, shareOptions);
        }

        /// <summary>
        /// Share a link url with compatible services
        /// </summary>
        /// <param name="url">Link to share</param>
        /// <param name="message">Message to include with the link</param>
        /// <param name="title">Title of the share popup on Android and Windows, email subject if sharing with mail apps</param>
        /// <returns>True if the operation was successful, false otherwise</returns>
        [Obsolete("Use Share(ShareMessage, ShareOptions)")]
        public Task<bool> ShareLink(string url, string message = null, string title = null)
        {
            var shareMessage = new ShareMessage();
            shareMessage.Title = title;
            shareMessage.Text = message;
            shareMessage.Url = url;

            var shareOptions = new ShareOptions();
            shareOptions.ChooserTitle = title;

            return Share(shareMessage, shareOptions);
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
                var items = new List<string>();
                if (message.Text != null)
                    items.Add(message.Text);
                if (message.Url != null)
                    items.Add(message.Url);

                var intent = new Intent(Intent.ActionSend);
                intent.SetType("text/plain");
                intent.PutExtra(Intent.ExtraText, string.Join(Environment.NewLine, items));
                if (message.Title != null)
                    intent.PutExtra(Intent.ExtraSubject, message.Title);

                var chooserIntent = Intent.CreateChooser(intent, options?.ChooserTitle);
                chooserIntent.SetFlags(ActivityFlags.ClearTop);
                chooserIntent.SetFlags(ActivityFlags.NewTask);
                Application.Context.StartActivity(chooserIntent);

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
                var sdk = (int)Android.OS.Build.VERSION.SdkInt;
                if (sdk < (int)Android.OS.BuildVersionCodes.Honeycomb)
                {
                    var clipboard = (Android.Text.ClipboardManager)Application.Context.GetSystemService(Context.ClipboardService);
                    clipboard.Text = text;
                }
                else
                {
                    var clipboard = (ClipboardManager)Application.Context.GetSystemService(Context.ClipboardService);
                    clipboard.PrimaryClip = ClipData.NewPlainText(label ?? string.Empty, text);
                }

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