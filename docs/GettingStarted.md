# Getting Started

## Setup
* NuGet: [Plugin.Share](http://www.nuget.org/packages/Plugin.Share) [![NuGet](https://img.shields.io/nuget/v/Plugin.Share.svg?label=NuGet)](https://www.nuget.org/packages/Plugin.Share/)
* `PM> Install-Package Plugin.Share`
* Install into ALL of your projects, include client projects.
* namespace: `using Plugin.Share`


## Using Share APIs
It is drop dead simple to gain access to the Share APIs in any project. All you need to do is get a reference to the current instance of IShare via `CrossShare.Current`:

```csharp
public void DoPluginStuff()
{
    CrossShare.Current.DOSTUFF(); 
    //this is not the real API, keep reading :)
}
```

There may be instances where you install a plugin into a platform that it isn't supported yet. This means you will have access to the interface, but no implementation exists. You can make a simple check before calling any API to see if it is supported on the platform where the code is running. This if nifty when unit testing:

```csharp
public void DoPluginStuff()
{
    if(!CrossShare.IsSupported)
        return true;

    CrossShare.Current.DOSTUFF();
    //this is not the real API, keep reading :)
}
```


<= Back to [Table of Contents](README.md)