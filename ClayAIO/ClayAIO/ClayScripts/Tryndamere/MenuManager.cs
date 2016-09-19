using EloBuddy.SDK.Menu;
using EloBuddy.SDK.Menu.Values;
using System.Collections.Generic;

namespace ClayAIO.ClayScripts.Tryndamere
{
    class MenuManager : MenuManagerBase
    {
        public Menu PermaActive;

        public MenuManager()
        {
            SkinDictionary = new Dictionary<string, int>();
            SkinDictionary.Add("Classic", 0);
            SkinDictionary.Add("Highland Tryndamere (Legacy)", 1);
            SkinDictionary.Add("King Tryndamere", 2);
            SkinDictionary.Add("Viking Tryndamere (Legacy)", 3);
            SkinDictionary.Add("Demonblade Tryndamere", 4);
            SkinDictionary.Add("Sultan Tryndamere", 5);
            SkinDictionary.Add("Warring Kingdoms Tryndamere (Legacy)", 6);
            SkinDictionary.Add("Nightmare Tryndamere", 7);
            SkinDictionary.Add("Beast Hunter Tryndamere", 8);

            GenerateMain();
            GeneratePermaActive();

            Initialize();
        }

        private void GenerateMain()
        {
            Main = MainMenu.AddMenu("ClayTryndamere", "clay_tryndamere");
            Main.AddGroupLabel("Welcome to ClayTryndamere!");
            Main.AddGroupLabel("Made by seraphim07");
        }

        #region PermaActive
        private void GeneratePermaActive()
        {
            PermaActive = Main.AddSubMenu("Perma Active", "perma_active");
            PermaActive.Add("perma_actve_use_q", new CheckBox("Use Q when you can survive with Q's heal", true));
            PermaActive.Add("perma_active_use_r", new CheckBox("Use R when incoming attack will kill you", true));
        }

        public bool PermaActiveUseQ
        {
            get
            {
                return (PermaActive["perma_active_use_q"] as CheckBox).CurrentValue;
            }
        }

        public bool PermaActiveUseR
        {
            get
            {
                return (PermaActive["perma_active_use_r"] as CheckBox).CurrentValue;
            }
        }
        #endregion
    }
}
