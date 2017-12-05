using System;
using System.Collections.Generic;
using System.Text;

namespace Plugin.Share.Abstractions
{
	public class ShareRectangle
	{
		public double X { get; set; }
		public double Y { get; set; }
		public double Width { get; set; }
		public double Height { get; set; }

		public ShareRectangle(double x, double y, double width, double height)
		{
			X = x;
			Y = y;
			Width = width;
			Height = height;
		}
	}
}
