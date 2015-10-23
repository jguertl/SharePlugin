using Share.Forms.Plugin.Abstractions;
using System;
using System.Linq;
using Xamarin.Forms;
using Share.Forms.Plugin.iOS;
#if __UNIFIED__
using Foundation;
using UIKit;
#else
using MonoTouch.Foundation;
using MonoTouch.UIKit;
#endif


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
      link = link.Trim();
      if (!string.IsNullOrWhiteSpace(link))
      {
        var tempUri = new Uri(link);
        link = tempUri.GetLeftPart(UriPartial.Authority) + System.Web.HttpUtility.UrlPathEncode(tempUri.PathAndQuery);
      }
      var shareitem = new NSObject[] { new NSString(title), new NSUrl(link) };
      var activityController = new UIActivityViewController(shareitem, null);

      activityController.SetValueForKey(new NSObject[] { new NSString(status) }.FirstOrDefault(), new NSString("subject"));

      //Would prefer this to popover from button on ipad
      if (activityController.PopoverPresentationController != null)
      {
        activityController.PopoverPresentationController.SourceView =
          UIApplication.SharedApplication.KeyWindow.RootViewController.ChildViewControllers != null
            ? UIApplication.SharedApplication.KeyWindow.RootViewController.ChildViewControllers[0].View
            : UIApplication.SharedApplication.KeyWindow.RootViewController.View;
      }

      UIApplication.SharedApplication.KeyWindow.RootViewController.ShowViewController(activityController, new NSObject());
		}
	}
}
