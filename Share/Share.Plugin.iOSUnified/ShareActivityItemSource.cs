using System;
using System.Collections.Generic;
using System.Text;
using Foundation;
using UIKit;

namespace Plugin.Share
{
    class ShareActivityItemSource : UIActivityItemSource
    {
        private NSObject item;
        private string subject;

        public ShareActivityItemSource(NSObject item, string subject)
        {
            this.item = item;
            this.subject = subject;
        }

        public override NSObject GetItemForActivity(UIActivityViewController activityViewController, NSString activityType)
        {
            return item;
        }

        public override NSObject GetPlaceholderData(UIActivityViewController activityViewController)
        {
            return item;
        }

        public override string GetSubjectForActivity(UIActivityViewController activityViewController, NSString activityType)
        {
            return subject;
        }
    }
}
