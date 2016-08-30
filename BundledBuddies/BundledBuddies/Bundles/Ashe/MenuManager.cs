using EloBuddy.SDK.Menu;
using EloBuddy.SDK.Menu.Values;

namespace BundledBuddies.Bundles.Ashe
{
    class MenuManager : MenuManagerBase
    {
        public Menu FireUlt;

        public MenuManager()
        {
            GenerateMain();
            GenerateFireUlt();
            GenerateCombo();
            GenerateHarass();
            GenerateLaneClear();
            GenerateJungleClear();
            GenerateFlee();

            Initialize();
        }

        private void GenerateMain()
        {
            Main = MainMenu.AddMenu("Bundled Ashe", "bundled_ashe");
            Main.AddGroupLabel("Welcome to Bundled Ashe!");
            Main.AddGroupLabel("Made by seraphim07");
        }

        #region Fire Ult
        private void GenerateFireUlt()
        {
            FireUlt = Main.AddSubMenu("Fire Ult", "fire_ult");
            FireUlt.Add("fire_ult_key", new KeyBind("Fire Ult", false, KeyBind.BindTypes.HoldActive, 'Z'));
        }

        public bool FireUltKey
        {
            get
            {
                return (FireUlt["fire_ult_key"] as KeyBind).CurrentValue;
            }
        }
        #endregion

        #region Combo
        private void GenerateCombo()
        {
            Combo = Main.AddSubMenu("Combo", "combo");
            Combo.Add("combo_use_q", new CheckBox("Use Q", true));
            Combo.Add("combo_use_w", new CheckBox("Use W", true));
            Combo.Add("combo_use_e", new CheckBox("Use E", true));
            Combo.Add("combo_use_r", new CheckBox("Use R", true));
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
        #endregion

        #region Harass
        private void GenerateHarass()
        {
            Harass = Main.AddSubMenu("Harass", "harass");
            Harass.Add("harass_use_w", new CheckBox("Use W", true));
            Harass.Add("harass_w_mana", new Slider("Use W when >= mana %", 50, 0, 100));
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
        #endregion

        #region Lane Clear
        private void GenerateLaneClear()
        {
            LaneClear = Main.AddSubMenu("Lane Clear", "lane_clear");
            LaneClear.Add("lane_clear_use_q", new CheckBox("Use Q", false));
            LaneClear.Add("lane_clear_q_mana", new Slider("Use Q when >= mana %", 50, 0, 100));
            LaneClear.Add("lane_clear_use_w", new CheckBox("Use W", true));
            LaneClear.Add("lane_clear_w_mana", new Slider("Use W when >= mana %", 80, 0, 100));
            LaneClear.Add("lane_clear_w_number", new Slider("Use W when >= number of minions", 5, 0, 10));
        }

        public bool LaneClearUseQ
        {
            get
            {
                return (LaneClear["lane_clear_use_q"] as CheckBox).CurrentValue;
            }
        }

        public int LaneClearQMana
        {
            get
            {
                return (LaneClear["lane_clear_q_mana"] as Slider).CurrentValue;
            }
        }

        public bool LaneClearUseW
        {
            get
            {
                return (LaneClear["lane_clear_use_w"] as CheckBox).CurrentValue;
            }
        }

        public int LaneClearWMana
        {
            get
            {
                return (LaneClear["lane_clear_w_mana"] as Slider).CurrentValue;
            }
        }

        public int LaneClearWNumber
        {
            get
            {
                return (LaneClear["lane_clear_w_number"] as Slider).CurrentValue;
            }
        }
        #endregion

        #region Jungle Clear
        private void GenerateJungleClear()
        {
            JungleClear = Main.AddSubMenu("Jungle Clear", "jungle_clear");
            JungleClear.Add("jungle_clear_use_q", new CheckBox("Use Q", false));
            JungleClear.Add("jungle_clear_use_w", new CheckBox("Use W", true));
        }

        public bool JungleClearUseQ
        {
            get
            {
                return (JungleClear["jungle_clear_use_q"] as CheckBox).CurrentValue;
            }
        }

        public bool JungleClearUseW
        {
            get
            {
                return (JungleClear["jungle_clear_use_w"] as CheckBox).CurrentValue;
            }
        }
        #endregion
        
        #region Flee
        private void GenerateFlee()
        {
            Flee = Main.AddSubMenu("Flee", "flee");
            Flee.Add("flee_use_w", new CheckBox("Use W", true));
        }

        public bool FleeUseW
        {
            get
            {
                return (Flee["flee_use_w"] as CheckBox).CurrentValue;
            }
        }
        #endregion
    }
}
