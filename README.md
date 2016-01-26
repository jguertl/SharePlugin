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

#### Maintaners
* [Jakob Gürtl](https://github.com/jguertl)
* [James Montemagno](https://github.com/jamesmontemagno)

#### License
Licensed under MIT license
