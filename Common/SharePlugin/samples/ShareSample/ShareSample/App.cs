using Plugin.Share;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Xamarin.Forms;

namespace ShareSample
{
    public class App : Application
    {
        public App()
        {
            var button = new Button
            {
                Text = "Share some text"
            };

            var button1 = new Button
            {
                Text = "Share a link"
            };

            var button2 = new Button
            {
                Text = "Open browser"
            };

            button.Clicked += (sender, args) =>
            {
                CrossShare.Current.Share("Follow @JamesMontemagno on Twitter", "Share");
            };

            button1.Clicked += (sender, args) =>
            {
                CrossShare.Current.ShareLink("http://motzcod.es", "Checkout my blog", "MotzCod.es");
            };

            button2.Clicked += (sender, args) =>
            {
                CrossShare.Current.OpenBrowser("http://motzcod.es");
            };

            // The root page of your application
            MainPage = new ContentPage
            {
                Content = new StackLayout
                {
                    VerticalOptions = LayoutOptions.Center,
                    Children = {
                        new Label {
                            HorizontalTextAlignment = TextAlignment.Center,
                            Text = "Welcome to Share Plugin Sample!"
                        }, button, button1, button2

                    }
                }
            };
        }

        protected override void OnStart()
        {
            // Handle when your app starts
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }
}
