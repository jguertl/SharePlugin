using Plugin.Share;
using Plugin.Share.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Xamarin.Forms;

namespace ShareTest
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

            var buttonShare = new Button
            {
                Text = "Share"
            };

            var switchTitle = new Switch { IsToggled = true };
            var switchText = new Switch { IsToggled = true };
            var switchUrl = new Switch { IsToggled = true };
            var switchChooserTitle = new Switch { IsToggled = true };

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
                CrossShare.Current.OpenBrowser("http://motzcod.es", new BrowserOptions() { SafariBarTintColor = new ShareColor(200, 0, 0), SafariControlTintColor = new ShareColor(255, 255, 255), ChromeToolbarColor = new ShareColor(200, 0, 0) });
            };

            buttonShare.Clicked += (sender, args) =>
            {
                var shareMessage = new ShareMessage();
                var shareOptions = new ShareOptions();

                if (switchTitle.IsToggled)
                    shareMessage.Title = "MotzCod.es";
                if (switchText.IsToggled)
                    shareMessage.Text = "Checkout my blog";
                if (switchUrl.IsToggled)
                    shareMessage.Url = "http://motzcod.es";

                if (switchChooserTitle.IsToggled)
                    shareOptions.ChooserTitle = "Share this!";

                CrossShare.Current.Share(shareMessage, shareOptions);
            };

            // The root page of your application
            MainPage = new ContentPage
            {
                Content = new StackLayout
                {
                    VerticalOptions = LayoutOptions.Center,
                    Children =
                    {
                        new Label
                        {
                            XAlign = TextAlignment.Center,
                            Text = "Welcome to Share Plugin Sample!"
                        },
                        button,
                        button1,
                        button2,
                        new Label
                        {
                            XAlign = TextAlignment.Center,
                            Text = "New sharing options:"
                        },
                        new StackLayout
                        {
                            Orientation = StackOrientation.Horizontal,
                            Children = { switchTitle, new Label { Text = "Include title" } }
                        },
                        new StackLayout
                        {
                            Orientation = StackOrientation.Horizontal,
                            Children = { switchText, new Label { Text = "Include text" } }
                        },
                        new StackLayout
                        {
                            Orientation = StackOrientation.Horizontal,
                            Children = { switchUrl, new Label { Text = "Include url" } }
                        },
                        new StackLayout
                        {
                            Orientation = StackOrientation.Horizontal,
                            Children = { switchChooserTitle, new Label { Text = "Include chooser title (Android only)" } }
                        },
                        buttonShare
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
