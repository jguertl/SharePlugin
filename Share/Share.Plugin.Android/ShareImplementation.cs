using Android.App;
using Android.Content;
using Android.Database;
using Android.Graphics;
using Plugin.Share.Abstractions;
using System;
using System.Threading.Tasks;
using System.Net;
using System.IO;

namespace Plugin.Share
{
  /// <summary>
  /// Implementation for Feature
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
                var intent = new Intent(Intent.ActionView);
                intent.SetData(Android.Net.Uri.Parse(url));

                intent.SetFlags(ActivityFlags.ClearTop);
                intent.SetFlags(ActivityFlags.NewTask);
                Android.App.Application.Context.StartActivity(intent);

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

		public async Task ShareLocalFile (string localFilePath, string title = "")
		{
			if (string.IsNullOrEmpty (localFilePath))
				return;

			var fileUri = Android.Net.Uri.Parse (localFilePath);
				
			var intent = new Intent ();
			intent.SetFlags(ActivityFlags.ClearTop);
			intent.SetFlags(ActivityFlags.NewTask);
			intent.SetAction (Intent.ActionSend);
			intent.SetType ("*/*");
			intent.PutExtra (Intent.ExtraStream, fileUri);
			intent.AddFlags (ActivityFlags.GrantReadUriPermission);

			var chooserIntent = Intent.CreateChooser(intent, title);
			chooserIntent.SetFlags(ActivityFlags.ClearTop);
			chooserIntent.SetFlags(ActivityFlags.NewTask);
			Android.App.Application.Context.StartActivity (chooserIntent);
		}

		public async Task ShareExternalFile(string fileUri, string fileName)
		{
			var uri = new System.Uri(fileUri);

			var webClient = new WebClient();
			webClient.DownloadDataCompleted += (s, e) => {

				if(e.Error != null)
					return;
				
				var bytes = e.Result; // get the downloaded data
				var localFolder = Android.OS.Environment.ExternalStorageDirectory.AbsolutePath;
				string localPath = System.IO.Path.Combine (localFolder, fileName);
				File.WriteAllBytes (localPath, bytes); // write to local storage
				ShareLocalFile(string.Format($"file://{localFolder}/{fileName}"), "Test Android");
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
            var intent = new Intent(Intent.ActionSend);
            intent.SetType("text/plain");
            intent.PutExtra(Intent.ExtraText, url ?? string.Empty);
            intent.PutExtra(Intent.ExtraSubject, message ?? string.Empty);

            var chooserIntent = Intent.CreateChooser(intent, title ?? string.Empty);
            chooserIntent.SetFlags(ActivityFlags.ClearTop);
            chooserIntent.SetFlags(ActivityFlags.NewTask);
            Android.App.Application.Context.StartActivity(chooserIntent);

        }

    }
}