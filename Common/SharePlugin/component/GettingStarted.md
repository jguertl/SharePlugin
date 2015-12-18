# Getting Started with Share Plugin

### API Usage

Call **CrossShare.Current** from any project or PCL to gain access to APIs.

**Share**
```
/// <summary>
/// Simply share text on compatible services
/// </summary>
/// <param name="text">Text to share</param>
/// <param name="title">Title of popup on share (not included in message)</param>
/// <returns>awaitable Task</returns>
Task Share(string text, string title = null);
```

**Open Browser**
```
/// <summary>
/// Open a browser to a specific url
/// </summary>
/// <param name="url">Url to open</param>
/// <returns>awaitable Task</returns>
Task OpenBrowser(string url);
```

**Share Link**
```
/// <summary>
/// Share a link url with compatible services
/// </summary>
/// <param name="url">Link to share</param>
/// <param name="message">Message to share</param>
/// <param name="title">Title of the popup</param>
/// <returns>awaitable Task</returns>
Task ShareLink(string url, string message = null, string title = null);
```
