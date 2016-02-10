namespace Plugin.Share.Abstractions
{
    /// <summary>
    /// Platform specific Browser Options
    /// </summary>
    public class BrowserOptions
    {
        /// <summary>
        /// iOS: Gets or Set to use the SFSafariWebViewController on iOS 9+ (recommended)
        /// Default is true
        /// </summary>
        public bool UseSafariWebViewController { get; set; } = true;
        /// <summary>
        /// iOS: Gets or sets to use reader mode (good for markdown files)
        /// Default is false
        /// </summary>
        public bool UseSafairReaderMode { get; set; } = false;
        /// <summary>
        /// Android: Gets or sets to display title as well as url in chrome custom tabs
        /// Default is true
        /// </summary>
        public bool ChromeShowTitle { get; set; } = true;
        /// <summary>
        /// Android: Gets or sets the toolbar color of the chrome custom tabs
        /// If null (default) will be default chrome color
        /// </summary>
        public ShareColor ChromeToolbarColor { get; set; } = null;
    }
}
