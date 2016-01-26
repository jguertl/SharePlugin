## ![](http://refractored.com/images/plugin_share.png) Share Plugin for Xamarin and Windows

Simple way to share a message or link on a social network in any Xamarin or Windows Project

#### Setup

* Available on NuGet: https://www.nuget.org/packages/Plugin.Share/ [![NuGet](https://img.shields.io/nuget/v/Plugin.Share.svg?label=NuGet)](https://www.nuget.org/packages/Share.Plugin/)
* Install into your PCL project and Platform Specific projects

Build Status: [![Build status](https://ci.appveyor.com/api/projects/status/xuonj5weexcjk6g9?svg=true)](https://ci.appveyor.com/project/JamesMontemagno/shareplugin)


**Platform Support**

|Platform|Supported|Version|
| ------------------- | :-----------: | :------------------: |
|Xamarin.iOS|Yes|iOS 7+|
|Xamarin.iOS Unified|Yes|iOS 7+|
|Xamarin.Android|Yes|API 10+|
|Windows Phone Silverlight|Yes|8.0+|
|Windows Phone RT|Yes|8.1+|
|Windows Store RT|Yes|8.1+|
|Windows 10 UWP|Yes|10+|
|Xamarin.Mac|No||
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

## Clipboard
Added in 3.2.X is the ability to set text directly on the clipboard. All Operatings support this except for Windows Phone 8.1 RT

```
/// <summary>
/// Sets text on the clipboard
/// </summary>
/// <param name="text">Text to set</param>
/// <param name="label">Label to dislay (no required, Android only)</param>
/// <returns></returns>
Task<bool> SetClipboardText(string text, string label = null);

/// <summary>
/// Gets if clipboard is supported
/// </summary>
bool SupportsClipboard { get; }
```


## iOS Specific Support 
Added in iOS 9 is the ability to use the new SFSafariViewController to open the browser: https://blog.xamarin.com/keep-users-engaged-with-ios-9s-sfsafariviewcontroller/ This is really awesome and you can toggle this on by setting:

```csharp
Plugin.Share.ShareImplementation.UseSafariViewController = true;
```
in your iOS project.

If your app is on iOS 9 and it is true then it will use the new controller, else it will drop to normal OpenUrl.

Please be aware of ATS restrcitions with SafariViews. 


#### Maintaners
* [Jakob Gürtl](https://github.com/jguertl)
* [James Montemagno](https://github.com/jamesmontemagno)

#### License
Licensed under MIT license
