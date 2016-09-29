using ClayAIO.ClayScripts;
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
                case Champion.Annie:
                    new ClayAnnie();
                    break;
                case Champion.Ashe:
                    new ClayAshe();
                    break;
                case Champion.Tryndamere:
                    new ClayTryndamere();
                    break;
            }
        }
    }
}