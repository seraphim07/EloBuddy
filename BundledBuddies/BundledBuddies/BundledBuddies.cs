﻿using BundledBuddies.Bundles;
using EloBuddy;
using EloBuddy.SDK.Events;
using System;

namespace BundledBuddies
{
    class BundledBuddies
    {
        static void Main(string[] args) { }

        static BundledBuddies()
        {
            Loading.OnLoadingComplete += OnLoadingComplete;
        }

        private static void OnLoadingComplete(EventArgs e)
        {
            switch (Player.Instance.Hero)
            {
                case Champion.Aatrox:
                    break;
                case Champion.Ahri:
                    break;
                case Champion.Akali:
                    break;
                case Champion.Alistar:
                    break;
                case Champion.Amumu:
                    break;
                case Champion.Anivia:
                    break;
                case Champion.Annie:
                    new BundledAnnie();
                    break;
                case Champion.Ashe:
                    new BundledAshe();
                    break;
                case Champion.Kayle:
                    new BundledKayle();
                    break;
            }
        }
    }
}
