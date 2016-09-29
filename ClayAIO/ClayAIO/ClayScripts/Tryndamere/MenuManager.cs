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
            GenerateCombo();
            GenerateLaneClear();
            GenerateJungleClear();
            GenerateFlee();

            Initialize();
        }

        private void GenerateMain()
        {
            Main = MainMenu.AddMenu("ClayTryndamere", "clay_tryndamere");
            Main.AddGroupLabel("Welcome to ClayTryndamere!");
            Main.AddGroupLabel("Made by seraphim07");
        }

        #region Perma Active
        private void GeneratePermaActive()
        {
            PermaActive = Main.AddSubMenu("Perma Active", "perma_active");
            PermaActive.Add("perma_active_use_q", new CheckBox("Use Q", true));
            PermaActive.Add("perma_active_q_hp", new Slider("Use Q when <= hp %", 25, 0, 100));
            PermaActive.Add("perma_active_use_r", new CheckBox("Use R", true));
            PermaActive.Add("perma_active_r_hp", new Slider("Use R when <= hp %", 10, 0, 100));
        }

        public bool PermaActiveUseQ
        {
            get
            {
                return (PermaActive["perma_active_use_q"] as CheckBox).CurrentValue;
            }
        }

        public int PermaActiveQHp
        {
            get
            {
                return (PermaActive["perma_active_q_hp"] as Slider).CurrentValue;
            }
        }

        public bool PermaActiveUseR
        {
            get
            {
                return (PermaActive["perma_active_use_r"] as CheckBox).CurrentValue;
            }
        }

        public int PermaActiveRHp
        {
            get
            {
                return (PermaActive["perma_active_r_hp"] as Slider).CurrentValue;
            }
        }
        #endregion

        #region Combo
        private void GenerateCombo()
        {
            Combo = Main.AddSubMenu("Combo", "combo");
            Combo.Add("combo_use_w", new CheckBox("Use W", true));
            Combo.Add("combo_use_e", new CheckBox("Use E", true));
        }

        public bool ComboUseW
        {
            get
            {
                return (Combo["combo_use_w"] as CheckBox).CurrentValue;
            }
        }

        public bool ComboUseE
        {
            get
            {
                return (Combo["combo_use_e"] as CheckBox).CurrentValue;
            }
        }
        #endregion
        
        #region Lane Clear
        private void GenerateLaneClear()
        {
            LaneClear = Main.AddSubMenu("Lane Clear", "lane_clear");
            LaneClear.Add("lane_clear_use_e", new CheckBox("Use E", true));
        }

        public bool LaneClearUseE
        {
            get
            {
                return (LaneClear["lane_clear_use_e"] as CheckBox).CurrentValue;
            }
        }
        #endregion

        #region Jungle Clear
        private void GenerateJungleClear()
        {
            JungleClear = Main.AddSubMenu("Jungle Clear", "jungle_clear");
            JungleClear.Add("jungle_clear_use_e", new CheckBox("Use E", true));
        }

        public bool JungleClearUseE
        {
            get
            {
                return (JungleClear["jungle_clear_use_e"] as CheckBox).CurrentValue;
            }
        }
        #endregion

        #region Flee
        private void GenerateFlee()
        {
            Flee = Main.AddSubMenu("Flee", "flee");
            Flee.Add("flee_use_e", new CheckBox("Use E", true));
        }

        public bool FleeUseE
        {
            get
            {
                return (Flee["flee_use_e"] as CheckBox).CurrentValue;
            }
        }
        #endregion
    }
}
