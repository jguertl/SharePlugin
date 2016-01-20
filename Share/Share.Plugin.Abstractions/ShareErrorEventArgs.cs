using System;

namespace Plugin.Share.Abstractions
{
	public class ShareErrorEventArgs: EventArgs
	{
		public Exception Exception { get; set;}
	}
}

