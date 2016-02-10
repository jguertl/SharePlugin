using Android.App;
using Android.Content;
using Android.Support.CustomTabs;
using Plugin.CurrentActivity;
using Plugin.Share.Abstractions;
using System;
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
        public static async Task Init()
        {
            var test = DateTime.UtcNow;
        }
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
                if (CrossCurrentActivity.Current.Activity == null)
                {
                    var intent = new Intent(Intent.ActionView);
                    intent.SetData(Android.Net.Uri.Parse(url));

                    intent.SetFlags(ActivityFlags.ClearTop);
                    intent.SetFlags(ActivityFlags.NewTask);
                    Android.App.Application.Context.StartActivity(intent);
                }
                else
                {
                    var tabsBuilder = new CustomTabsIntent.Builder();
                    tabsBuilder.SetShowTitle(options?.ChromeShowTitle ?? false);
                    var toolbarColor = options?.ChromeToolbarColor;
                    if (toolbarColor != null)
                    {
                        tabsBuilder.SetToolbarColor(Android.Graphics.Color.Argb(toolbarColor.A,
                            toolbarColor.R,
                            toolbarColor.G,
                            toolbarColor.B));
                    }
                    
                    var intent = tabsBuilder.Build();
                    intent.LaunchUrl(CrossCurrentActivity.Current.Activity, Android.Net.Uri.Parse(url));
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
            var intent = new Intent(Intent.ActionSend);
            intent.SetType("text/plain");
            intent.PutExtra(Intent.ExtraText, text ?? string.Empty);

            intent.SetFlags(ActivityFlags.ClearTop);
            intent.SetFlags(ActivityFlags.NewTask);
            var chooserIntent = Intent.CreateChooser(intent, title ?? string.Empty);
            chooserIntent.SetFlags(ActivityFlags.ClearTop);
            chooserIntent.SetFlags(ActivityFlags.NewTask);
            Android.App.Application.Context.StartActivity(chooserIntent);
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
            var intent = new Intent(Intent.ActionSend);
            intent.SetType("text/plain");
            intent.PutExtra(Intent.ExtraText, url ?? string.Empty);
            intent.PutExtra(Intent.ExtraSubject, message ?? string.Empty);

            var chooserIntent = Intent.CreateChooser(intent, title ?? string.Empty);
            chooserIntent.SetFlags(ActivityFlags.ClearTop);
            chooserIntent.SetFlags(ActivityFlags.NewTask);
            Android.App.Application.Context.StartActivity(chooserIntent);
           
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
                System.Diagnostics.Debug.WriteLine("Unable to copy to clipboard: " + ex);
            }

            return Task.FromResult(false);
        }

        /// <summary>
        /// Gets if cliboard is supported
        /// </summary>
        public bool SupportsClipboard => true;

    }
}