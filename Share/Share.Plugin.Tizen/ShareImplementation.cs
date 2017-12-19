using Plugin.Share.Abstractions;
using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Tizen.Applications;

namespace Plugin.Share
{
	/// <summary>
	/// Implementation for Share
	/// </summary>
	public class ShareImplementation : IShare
	{
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
				AppControl appControl = new AppControl();
				appControl.Operation = AppControlOperations.View;
				appControl.Uri = url;
				AppControl.SendLaunchRequest(appControl);

				return Task.FromResult(true);
			}
			catch (Exception ex)
			{
				Debug.WriteLine("Unable to open browser: " + ex.Message);
				return Task.FromResult(false);
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
			if (message == null)
				throw new ArgumentNullException(nameof(message));

			try
			{
				string sharedResourcePath = Application.Current.ApplicationInfo.SharedResourcePath.ToString();
				AppControl appControl = new AppControl();

				if (options != null)
				{
					switch(options.ExcludedAppControlTypes)
					{
						case ShareAppControlType.FileInEmail:
							appControl.Operation = AppControlOperations.Share;
							appControl.Uri = "mailto:";
							if (message.Url == null)
								throw new ArgumentNullException(nameof(message.Url));
							appControl.ExtraData.Add("http://tizen.org/appcontrol/data/path", sharedResourcePath + "/" + message.Url);
							break;
						case ShareAppControlType.FileInMessage:
							appControl.Operation = AppControlOperations.Share;
							appControl.Uri = "mmsto:";
							if (message.Url == null)
								throw new ArgumentNullException(nameof(message.Url));
							appControl.ExtraData.Add("http://tizen.org/appcontrol/data/path", sharedResourcePath + "/" + message.Url);
							break;
						case ShareAppControlType.TextInEmail:
							appControl.Operation = AppControlOperations.ShareText;
							appControl.Uri = "mailto:";
							if (message.Text == null)
								throw new ArgumentNullException(nameof(message.Text));
							appControl.ExtraData.Add("http://tizen.org/appcontrol/data/subject", message.Title);
							appControl.ExtraData.Add("http://tizen.org/appcontrol/data/text", message.Text);
							//appControl.ExtraData.Add("http://tizen.org/appcontrol/data/url", message.Url);
							break;
						case ShareAppControlType.TextInSMS:
							appControl.Operation = AppControlOperations.ShareText;
							appControl.Uri = "sms:";
							if (message.Text == null)
								throw new ArgumentNullException(nameof(message.Text));
							appControl.ExtraData.Add("http://tizen.org/appcontrol/data/subject", message.Title);
							appControl.ExtraData.Add("http://tizen.org/appcontrol/data/text", message.Text);
							//appControl.ExtraData.Add("http://tizen.org/appcontrol/data/url", message.Url);
							break;
						case ShareAppControlType.TextInMMS:
							appControl.Operation = AppControlOperations.ShareText;
							appControl.Uri = "mmsto:";
							if (message.Text == null)
								throw new ArgumentNullException(nameof(message.Text));
							appControl.ExtraData.Add("http://tizen.org/appcontrol/data/subject", message.Title);
							appControl.ExtraData.Add("http://tizen.org/appcontrol/data/text", message.Text);
							//appControl.ExtraData.Add("http://tizen.org/appcontrol/data/url", message.Url);
							break;
						case ShareAppControlType.Link:
							appControl.Operation = AppControlOperations.ShareText;
							if (message.Url == null)
								throw new ArgumentNullException(nameof(message.Url));
							appControl.ExtraData.Add("http://tizen.org/appcontrol/data/subject", message.Title);
							appControl.ExtraData.Add("http://tizen.org/appcontrol/data/url", message.Text);
							appControl.ExtraData.Add("http://tizen.org/appcontrol/data/text", message.Url);
							break;
						default:
							appControl.Operation = AppControlOperations.ShareText;
							appControl.ExtraData.Add("http://tizen.org/appcontrol/data/subject", message.Title);
							appControl.ExtraData.Add("http://tizen.org/appcontrol/data/text", message.Text);
							appControl.ExtraData.Add("http://tizen.org/appcontrol/data/url", message.Url);
							break;
					}
				}
				else
				{
					appControl.Operation = AppControlOperations.ShareText;
					appControl.ExtraData.Add("http://tizen.org/appcontrol/data/subject", message.Title);
					appControl.ExtraData.Add("http://tizen.org/appcontrol/data/text", message.Text);
					appControl.ExtraData.Add("http://tizen.org/appcontrol/data/url", message.Url);
				}
				
				AppControl.SendLaunchRequest(appControl);

				return Task.FromResult(true);
			}
			catch (Exception ex)
			{
				Debug.WriteLine("Unable to share: " + ex.Message);
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
			return Task.FromResult(false);
		}

		public bool CanOpenUrl(string url) => true;

		/// <summary>
		/// Gets if cliboard is supported
		/// </summary>
		public bool SupportsClipboard => false;
	}
}
