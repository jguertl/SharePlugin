using Microsoft.Phone.Tasks;
using Share.Plugin.Abstractions;
using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;

namespace Share.Plugin
{
    /// <summary>
    /// Implementation for Share
    /// </summary>
    public class ShareImplementation : IShare
    {
        public static async Task Init()
        {
            var test = DateTime.UtcNow;
        }
        public async Task OpenBrowser(string url)
        {
            try
            {
                var webBrowserTask = new WebBrowserTask();

                webBrowserTask.Uri = new Uri(url, UriKind.Absolute);

                Deployment.Current.Dispatcher.BeginInvoke(webBrowserTask.Show);

            }
            catch (Exception ex)
            {
                Console.WriteLine("Unable to open browser task: " + ex.Message);
            }
        }

        public async Task Share(string text, string title = null)
        {
            var task = new ShareStatusTask
            {
                Status = text ?? string.Empty
            };

            Deployment.Current.Dispatcher.BeginInvoke(task.Show);
        }

        public async Task ShareLink(string url, string message = null, string title = null)
        {
            try
            {
                var task = new ShareLinkTask
                {
                    Message = message ?? string.Empty,
                    LinkUri = new Uri(url),
                    Title = title ?? string.Empty
                };

                Deployment.Current.Dispatcher.BeginInvoke(task.Show);
            }
            catch(Exception ex)
            {
                Console.WriteLine("Unable to create share link task: " + ex);
                return;
            }

        }
    }
}