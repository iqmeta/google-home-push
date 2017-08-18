using SharpCaster.Controllers;
using SharpCaster.Models;
using SharpCaster.Models.ChromecastRequests;
using SharpCaster.Models.ChromecastStatus;
using SharpCaster.Models.MediaStatus;
using SharpCaster.Services;
using System;
using System.Threading.Tasks;

namespace PushNotificationGoogleHome
{
    class Program
    {
        static readonly ChromecastService ChromecastService = ChromecastService.Current;
        static SharpCasterDemoController _controller;

        static void Main(string[] args)
        {
            AsyncBabyAsyn();
            var input = System.Console.ReadLine();
        }
        private static async Task AsyncBabyAsyn()
        {
            ChromecastService.ChromeCastClient.ApplicationStarted += Client_ApplicationStarted;
            ChromecastService.ChromeCastClient.VolumeChanged += _client_VolumeChanged;
            ChromecastService.ChromeCastClient.MediaStatusChanged += ChromeCastClient_MediaStatusChanged;
            ChromecastService.ChromeCastClient.ConnectedChanged += ChromeCastClient_Connected;
            var chromecast = new Chromecast
            {
                DeviceUri = new Uri("http://192.168.178.158:8009"),
                FriendlyName = "Google Home"
            };
            ChromecastService.ConnectToChromecast(chromecast);
        }
        private static async void ChromeCastClient_Connected(object sender, EventArgs e)
        {
            System.Console.Write("Connected...");
            if (_controller == null)
            {
                _controller = await ChromecastService.ChromeCastClient.LaunchSharpCaster();
            }
        }
        private async static void ChromeCastClient_MediaStatusChanged(object sender, MediaStatus e)
        {
        }
        private static void _client_VolumeChanged(object sender, Volume e)
        {
        }
        private static async void Client_ApplicationStarted(object sender, ChromecastApplication e)
        {
            System.Console.WriteLine($"Launched {e.DisplayName}");
            while (_controller == null)
            {
                await Task.Delay(500);
            }
            await _controller.LoadMedia("https://iqmeta.com/chikalicka_bing.mp3", "audio/mp3", null, StreamType.BUFFERED);
        }
    }
}
