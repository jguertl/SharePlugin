## Clipboard Access
Sometimes you may want to copy text to the clipboard so users can paste it into a text box elsewhere in the app.


#### Check Clipboard support
First, you should check if the device actually supports the clipboard.

```csharp
/// <summary>
/// Gets if clipboard is supported
/// </summary>
bool SupportsClipboard { get; }
```


### Copy Text to Clipboard
After you check to see if the device and OS supports the clipboard, you can copy text to it. Additionally, Android can display a user-visible label for the clip data.

```csharp
/// <summary>
/// Sets text on the clipboard
/// </summary>
/// <param name="text">Text to set</param>
/// <param name="label">Label to dislay (not required, Android only)</param>
/// <returns></returns>
Task<bool> SetClipboardText(string text, string label = null);
```

Example:

```csharp
public async Task<bool> Copy(string text)
{
    if(!CrossShare.Current.SupportsClipboard)
       return false;
      
    return CrossShare.Current.SetClipboardText("Follow @JamesMontemagno", "Twitter");
}
```

It is a good idea to check the result and pop up a dialog or toast to your user.

<= Back to [Table of Contents](README.md)