using System;

namespace Share.Forms.Plugin.Abstractions
{
	public interface IShare
	{
		void ShareStatus(string status);
		void ShareLink(string title, string status, string link);
	}
}
