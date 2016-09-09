using EloBuddy;
using EloBuddy.SDK.Events;
using System;

namespace ClayAIO
{
    class ClayAIO
    {
        static void Main(string[] args)
        {
            Loading.OnLoadingComplete += OnLoadingComplete;
        }

        private static void OnLoadingComplete(EventArgs e)
        {
            switch (Player.Instance.Hero)
            {
                case Champion.Ashe:
                    new ClayAshe();
                    break;
            }
        }
    }
}