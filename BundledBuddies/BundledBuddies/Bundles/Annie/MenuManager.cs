using EloBuddy.SDK.Menu;
using EloBuddy.SDK.Menu.Values;

namespace BundledBuddies.Bundles.Annie
{
    class MenuManager : MenuManagerBase
    {
        public static Menu Initiator;

        public MenuManager()
        {
            GenerateMain();
            GenerateCombo();
            GenerateHarass();
            GenerateLaneClear();
            GenerateJungleClear();
            GenerateLastHit();
            GenerateFlee();

            Initialize();
        }

        private void GenerateMain()
        {
            Main = MainMenu.AddMenu("Bundled Annie", "bundled_annie");
            Main.AddGroupLabel("Welcome to Bundled Annie!");
            Main.AddGroupLabel("Made by seraphim07");
        }

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
            Harass.Add("harass_use_w_stack_stun", new CheckBox("Use W to stack stun", true));
            Harass.Add("harass_w_mana", new Slider("Use W when >= mana %", 50, 0, 100));
            Harass.Add("harass_use_e_stack_stun", new CheckBox("Use E to stack stun", true));
            Harass.Add("harass_use_q", new CheckBox("Use Q", true));
            Harass.Add("harass_use_w", new CheckBox("Use W", true));
        }

        public bool HarassUseWStackStun
        {
            get
            {
                return (Harass["harass_use_w_stack_stun"] as CheckBox).CurrentValue;
            }
        }

        public int HarassWMana
        {
            get
            {
                return (Harass["harass_w_mana"] as Slider).CurrentValue;
            }
        }
        
        public bool HarassUseEStackStun
        {
            get
            {
                return (Harass["harass_use_e_stack_stun"] as CheckBox).CurrentValue;
            }
        }

        public bool HarassUseQ
        {
            get
            {
                return (Harass["harass_use_q"] as CheckBox).CurrentValue;
            }
        }

        public bool HarassUseW
        {
            get
            {
                return (Harass["harass_use_w"] as CheckBox).CurrentValue;
            }
        }
        #endregion

        #region Lane Clear
        private void GenerateLaneClear()
        {
            LaneClear = Main.AddSubMenu("Lane Clear", "lane_clear");
            LaneClear.Add("lane_clear_use_w", new CheckBox("Use W", true));
            LaneClear.Add("lane_clear_w_mana", new Slider("Use W when >= mana %", 80, 0, 100));
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
        #endregion

        #region Jungle Clear
        private void GenerateJungleClear()
        {
            JungleClear = Main.AddSubMenu("Jungle Clear", "jungle_clear");
            JungleClear.Add("jungle_clear_use_q_without_last_hit", new CheckBox("Use Q even when it's not last hit", true));
            JungleClear.Add("jungle_clear_use_w", new CheckBox("Use W", true));
            JungleClear.Add("jungle_clear_use_e", new CheckBox("Use E", true));
        }

        public bool JungleClearUseQWithoutLastHit
        {
            get
            {
                return (JungleClear["jungle_clear_use_q_without_last_hit"] as CheckBox).CurrentValue;
            }
        }

        public bool JungleClearUseW
        {
            get
            {
                return (JungleClear["jungle_clear_use_w"] as CheckBox).CurrentValue;
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

        #region Last Hit
        private void GenerateLastHit()
        {
            LastHit = Main.AddSubMenu("Last Hit", "last_hit");
            LastHit.Add("last_hit_use_q", new CheckBox("Use Q", true));
        }

        public bool LastHitUseQ
        {
            get
            {
                return (LastHit["last_hit_use_q"] as CheckBox).CurrentValue;
            }
        }
        #endregion

        #region Flee
        private void GenerateFlee()
        {
            Flee = Main.AddSubMenu("Flee", "flee");
            Flee.Add("flee_use_q_stun", new CheckBox("Use Q to stun", true));
        }

        public bool FleeUseQStun
        {
            get
            {
                return (Flee["flee_use_q_stun"] as CheckBox).CurrentValue;
            }
        }
        #endregion
    }
}
