using EloBuddy;
using EloBuddy.SDK.Events;
using System;

namespace ClayTutorial
{
    class Program
    {
        static void Main(string[] args)
        {
            Loading.OnLoadingComplete += OnLoadingComplete;
        }

        private static void OnLoadingComplete(EventArgs e)
        {
            Chat.OnClientSideMessage += OnClientSideMessage;
        }

        private static void OnClientSideMessage(ChatClientSideMessageEventArgs args)
        {
            Chat.Print(args.Message);
        }
    }
}
