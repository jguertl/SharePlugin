using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Plugin.Share.Abstractions
{
    public enum ShareUIActivityType
    {
        // iOS 6+

        /// <summary>
        /// The object assigns the image to a contact.
        /// </summary>
        AssignToContact,
        /// <summary>
        /// The object posts the provided content to the pasteboard.
        /// </summary>
        CopyToPasteboard,
        /// <summary>
        /// The object posts the provided content to a new email message.
        /// </summary>
        Mail,
        /// <summary>
        /// The object posts the provided content to the Messages app.
        /// </summary>
        Message,
        /// <summary>
        /// The object posts the provided content to the user’s wall on Facebook.
        /// </summary>
        PostToFacebook,
        /// <summary>
        /// The object posts the provided content to the user’s Twitter feed.
        /// </summary>
        PostToTwitter,
        /// <summary>
        /// The object posts the provided content to the user’s Weibo feed.
        /// </summary>
        PostToWeibo,
        /// <summary>
        /// The object prints the provided content.
        /// </summary>
        Print,
        /// <summary>
        /// The object assigns the image or video to the user’s camera roll.
        /// </summary>
        SaveToCameraRoll,


        // iOS 7+

        /// <summary>
        /// The object adds the URL to Safari’s reading list.
        /// Supported on iOS 7+ only.
        /// </summary>
        AddToReadingList,
        /// <summary>
        /// The object makes the provided content available via AirDrop.
        /// Supported on iOS 7+ only.
        /// </summary>
        AirDrop,
        /// <summary>
        /// The object posts the provided image to the user’s Flickr account.
        /// Supported on iOS 7+ only.
        /// </summary>
        PostToFlickr,
        /// <summary>
        /// The object posts the provided content to the user’s Tencent Weibo feed.
        /// Supported on iOS 7+ only.
        /// </summary>
        PostToTencentWeibo,
        /// <summary>
        /// The object posts the provided video to the user’s Vimeo account.
        /// Supported on iOS 7+ only.
        /// </summary>
        PostToVimeo,


        // iOS 9+

        /// <summary>
        /// Supported on iOS 9+ only.
        /// </summary>
        OpenInIBooks,
    }
}
