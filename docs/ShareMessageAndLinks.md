## Sharing Messages/Text and Links
This is the main use case for this library and gives you the ability to easily bring up the share dialoges for each platform to share text and url links.

```csharp
/// <summary>
/// Share a message with compatible services
/// </summary>
/// <param name="message">Message to share</param>
/// <param name="options">Platform specific options</param>
/// <returns>True if the operation was successful, false otherwise</returns>
Task<bool> Share(ShareMessage message, ShareOptions options = null);
```

The `ShareMessage` is required and determines what can be shared. If you fill out the `Url` field then the link share sheet will appear. It will return a `bool` to determine if the share was successfully brought up or not.


```csharp
/// <summary>
/// Message object to share with compatible services
/// </summary>
public class ShareMessage
{
    /// <summary>
    /// Gets or sets the title of the message. Used as email subject if sharing with mail apps.
    /// </summary>
    public string Title { get; set; }

    /// <summary>
    /// Gets or sets the text of the message.
    /// </summary>
    public string Text { get; set; }

    /// <summary>
    /// Gets or sets the link to include with the message.
    /// </summary>
    public string Url { get; set; }
}
```

Example:

```csharp
public void ShareBlog()
{
    if(!CrossShare.IsSupported)
        return;

    CrossShare.Current.Share(new ShareMessage
    {
       Title = "Motz Cod.es",
       Text = "Checkout Motz Cod.es! for all sorts of goodies",
       Url = "http://motzcod.es"
    });
}
```

Since there is a bit more complexity on specific platforms there are additional options that can be passed in:

```csharp
/// <summary>
/// Platform specific Share Options
/// </summary>
public class ShareOptions
{
    /// <summary>
    /// Android: Gets or sets the title of the app chooser popup.
    /// If null (default) the system default title is used.
    /// </summary>
    public string ChooserTitle { get; set; } = null;

    /// <summary>
    /// iOS: Gets or sets the UIActivityTypes that should not be displayed.
    /// If null (default) the value of <see cref="Plugin.Share.ShareImplementation.ExcludedUIActivityTypes"/> is used.
    /// </summary>
    public ShareUIActivityType[] ExcludedUIActivityTypes { get; set; } = null;
}
```

Example:

```csharp
public void ShareBlog()
{
    if(!CrossShare.IsSupported)
        return;

    CrossShare.Current.Share(new ShareMessage
    {
       Title = "Motz Cod.es",
       Message = "Checkout Motz Cod.es! for all sorts of goodies",
       Url = "http://motzcod.es"
    },
    new ShareOptions
    {
        ChooserTitle = "Share Blog",
        ExcludedUIActivityTypes  = new [] { ShareUIActivityType.PostToFacebook } 
    });
}
```


<= Back to [Table of Contents](README.md)

