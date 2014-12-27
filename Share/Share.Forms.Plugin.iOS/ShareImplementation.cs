using Share.Forms.Plugin.Abstractions;
using System;
using Xamarin.Forms;
using Share.Forms.Plugin.iOS;
using MonoTouch.UIKit;
using MonoTouch.Foundation;
using MonoTouch.Social;

[assembly: Dependency(typeof(ShareImplementation))]
namespace Share.Forms.Plugin.iOS
{
    public class ShareImplementation : IShare
    {
		public static void Init() 
		{

		}

		public void ShareStatus (string status)
		{
			ShowActionSheet(status);
		}

		public void ShareLink (string title, string status, string link)
		{
			ShowActionSheet(title, status, link);
		}

		private void ShowActionSheet(string status, string title = "", string link = "")
		{
			var actionSheet = new UIActionSheet("Share on");
			foreach (SLServiceKind service in Enum.GetValues(typeof(SLServiceKind)))
			{
				actionSheet.AddButton(service.ToString());
			}
			actionSheet.Clicked += delegate(object a, UIButtonEventArgs b) {
				SLServiceKind serviceKind = (SLServiceKind)Enum.Parse(typeof(SLServiceKind), actionSheet.ButtonTitle(b.ButtonIndex));
				ShareOnService(serviceKind,title,status,link);
			};
			actionSheet.ShowInView (UIApplication.SharedApplication.KeyWindow.RootViewController.View);
		}

		private void ShareOnService(SLServiceKind service, string title, string status, string link)
		{
			SLComposeViewController slComposer;
			if (SLComposeViewController.IsAvailable(service)) {
				slComposer = SLComposeViewController.FromService(service);
				slComposer.SetInitialText(status);
				if (title != null)
					slComposer.SetInitialText(String.Format("{0} {1}",title,status));
				else
					slComposer.SetInitialText(status);
				if (link != null)
					slComposer.AddUrl (new NSUrl (link));
				slComposer.CompletionHandler += (result) => {
					UIApplication.SharedApplication.KeyWindow.RootViewController.InvokeOnMainThread (() => {
						UIApplication.SharedApplication.KeyWindow.RootViewController.DismissViewController (true, null);
					});
				};
				UIApplication.SharedApplication.KeyWindow.RootViewController.PresentViewController (slComposer, true, null);
			}
		}
	}
}
