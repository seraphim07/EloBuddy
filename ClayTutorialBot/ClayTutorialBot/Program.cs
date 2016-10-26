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
            Game.OnNotify += OnNotify;
        }

        private static void OnClientSideMessage(ChatClientSideMessageEventArgs args)
        {
            Console.WriteLine(args.Message);
        }

        private static void OnNotify(GameNotifyEventArgs args)
        {
            Console.WriteLine(args.EventId);
            Console.WriteLine(args.NetworkId);
        }
    }
}
