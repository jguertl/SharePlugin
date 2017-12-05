using System;
using Plugin.Share.Abstractions;
using Xamarin.Forms;

namespace ShareTest
{
	using System;
	using Plugin.Share.Abstractions;
	using Xamarin.Forms;


	public static class VisualElementExtensions
	{

		public static ShareRectangle GetScreenRect(this VisualElement view)
		{
			double screenCoordinateX = view.X;
			double screenCoordinateY = view.Y;

			if (view.Parent.GetType() != typeof(App))
			{
				VisualElement parent = (VisualElement)view.Parent;

				// Loop through all parents
				while (parent != null)
				{
					// Add in the coordinates of the parent with respect to ITS parent
					screenCoordinateX += parent.X;
					screenCoordinateY += parent.Y;

					// If the parent of this parent isn't the app itself, get the parent's parent.
					if (parent.Parent.GetType() == typeof(App))
						parent = null;
					else
						parent = (VisualElement)parent.Parent;
				}
			}

			return new ShareRectangle(screenCoordinateX, screenCoordinateY, view.Width, view.Height);
		}
	}

}
