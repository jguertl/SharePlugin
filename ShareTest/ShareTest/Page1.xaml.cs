using Plugin.Share;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;

namespace ShareTest
{
    public partial class Page1 : ContentPage
    {
        public Page1()
        {
            InitializeComponent();
            ButtonShare.Clicked += delegate
            {
                CrossShare.Current.Share("Follow @JamesMontemagno on Twitter", "Share");
            };

            ButtonBrowser.Clicked += delegate
            {
                CrossShare.Current.OpenBrowser("http://motzcod.es", new Plugin.Share.Abstractions.BrowserOptions
                {
                    UseSafariWebViewController = true,
                    
                });
            };
        }
    }
}
