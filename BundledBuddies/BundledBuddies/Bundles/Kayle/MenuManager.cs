using EloBuddy.SDK.Menu;
using EloBuddy.SDK.Menu.Values;

namespace BundledBuddies.Bundles.Kayle
{
    class MenuManager : MenuManagerBase
    {
        public MenuManager()
        {
            GenerateMain();
            GenerateCombo();
            GenerateHarass();
            GenerateLaneClear();
            GenerateJungleClear();
            GenerateFlee();

            Initialize();
        }

        private void GenerateMain()
        {
            Main = MainMenu.AddMenu("Bundled Kayle", "bundled_kayle");
            Main.AddGroupLabel("Welcome to Bundled Kayle!");
            Main.AddGroupLabel("Made by seraphim07");
        }
        
        #region Combo
        private void GenerateCombo()
        {
            Combo = Main.AddSubMenu("Combo", "combo");
            Combo.Add("combo_use_q", new CheckBox("Use Q", true));
            Combo.Add("combo_use_w", new CheckBox("Use W", true));
            Combo.Add("combo_w_hp", new Slider("Use W when <= hp %", 90, 0, 100));
            Combo.Add("combo_use_e", new CheckBox("Use E", true));
            Combo.Add("combo_use_r", new CheckBox("Use R", true));
            Combo.Add("combo_use_r_for_ally", new CheckBox("Use R for ally", true));
            Combo.Add("combo_r_hp", new Slider("Use R when <= hp %", 10, 0, 100));
        }

        public bool ComboUseQ
        {
            get
            {
                return (Combo["combo_use_q"] as CheckBox).CurrentValue;
            }
        }

        public bool ComboUseW
        {
            get
            {
                return (Combo["combo_use_w"] as CheckBox).CurrentValue;
            }
        }

        public int ComboWHp
        {
            get
            {
                return (Combo["combo_w_hp"] as Slider).CurrentValue;
            }
        }

        public bool ComboUseE
        {
            get
            {
                return (Combo["combo_use_e"] as CheckBox).CurrentValue;
            }
        }

        public bool ComboUseR
        {
            get
            {
                return (Combo["combo_use_r"] as CheckBox).CurrentValue;
            }
        }
        
        public bool ComboUseRForAlly
        {
            get
            {
                return (Combo["combo_use_r_for_ally"] as CheckBox).CurrentValue;
            }
        }

        public int ComboRHp
        {
            get
            {
                return (Combo["combo_r_hp"] as Slider).CurrentValue;
            }
        }
        #endregion

        #region Harass
        private void GenerateHarass()
        {
            Harass = Main.AddSubMenu("Harass", "harass");
            Harass.Add("harass_use_q", new CheckBox("Use Q", true));
            Harass.Add("harass_q_mana", new Slider("Use Q when >= mana %", 50, 0, 100));
            Harass.Add("harass_use_w", new CheckBox("Use W", true));
            Harass.Add("harass_w_mana", new Slider("Use W when >= mana %", 50, 0, 100));
            Harass.Add("harass_w_hp", new Slider("Use W when <= hp %", 50, 0, 100));
            Harass.Add("harass_use_e", new CheckBox("Use E", true));
            Harass.Add("harass_e_mana", new Slider("Use E when >= mana %", 50, 0, 100));
        }
        
        public bool HarassUseQ
        {
            get
            {
                return (Harass["harass_use_q"] as CheckBox).CurrentValue;
            }
        }

        public int HarassQMana
        {
            get
            {
                return (Harass["harass_q_mana"] as Slider).CurrentValue;
            }
        }

        public bool HarassUseW
        {
            get
            {
                return (Harass["harass_use_w"] as CheckBox).CurrentValue;
            }
        }

        public int HarassWMana
        {
            get
            {
                return (Harass["harass_w_mana"] as Slider).CurrentValue;
            }
        }

        public int HarassWHp
        {
            get
            {
                return (Harass["harass_w_hp"] as Slider).CurrentValue;
            }
        }

        public bool HarassUseE
        {
            get
            {
                return (Harass["harass_use_e"] as CheckBox).CurrentValue;
            }
        }

        public int HarassEMana
        {
            get
            {
                return (Harass["harass_e_mana"] as Slider).CurrentValue;
            }
        }
        #endregion

        #region Lane Clear
        private void GenerateLaneClear()
        {
            LaneClear = Main.AddSubMenu("Lane Clear", "lane_clear");
            LaneClear.Add("lane_clear_use_e", new CheckBox("Use E", true));
            LaneClear.Add("lane_clear_e_mana", new Slider("Use E when >= mana %", 50, 0, 100));
        }

        public bool LaneClearUseE
        {
            get
            {
                return (LaneClear["lane_clear_use_e"] as CheckBox).CurrentValue;
            }
        }

        public int LaneClearEMana
        {
            get
            {
                return (LaneClear["lane_clear_e_mana"] as Slider).CurrentValue;
            }
        }
        #endregion

        #region Jungle Clear
        private void GenerateJungleClear()
        {
            JungleClear = Main.AddSubMenu("Jungle Clear", "jungle_clear");
            JungleClear.Add("jungle_clear_use_q", new CheckBox("Use Q", true));
            JungleClear.Add("jungle_clear_use_e", new CheckBox("Use E", true));
        }

        public bool JungleClearUseQ
        {
            get
            {
                return (JungleClear["jungle_clear_use_q"] as CheckBox).CurrentValue;
            }
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
            Flee.Add("flee_use_q", new CheckBox("Use Q", true));
            Flee.Add("flee_use_w", new CheckBox("Use W", true));
            Flee.Add("flee_use_r", new CheckBox("Use R", true));
            Flee.Add("flee_r_hp", new Slider("Use R when <= hp %", 10, 0, 100));
        }

        public bool FleeUseQ
        {
            get
            {
                return (Flee["flee_use_q"] as CheckBox).CurrentValue;
            }
        }

        public bool FleeUseW
        {
            get
            {
                return (Flee["flee_use_w"] as CheckBox).CurrentValue;
            }
        }

        public bool FleeUseR
        {
            get
            {
                return (Flee["flee_use_r"] as CheckBox).CurrentValue;
            }
        }

        public int FleeRHp
        {
            get
            {
                return (Flee["flee_r_hp"] as Slider).CurrentValue;
            }
        }
        #endregion
    }
}
