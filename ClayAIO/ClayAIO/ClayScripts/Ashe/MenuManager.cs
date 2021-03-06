﻿using EloBuddy.SDK.Menu;
using EloBuddy.SDK.Menu.Values;
using System.Collections.Generic;

namespace ClayAIO.ClayScripts.Ashe
{
    class MenuManager : MenuManagerBase
    {
        public Menu FireUlt;

        public MenuManager()
        {
            SkinDictionary = new Dictionary<string, int>();
            SkinDictionary.Add("Classic", 0);
            SkinDictionary.Add("Freljord Ashe (Legacy)", 1);
            SkinDictionary.Add("Sherwood Forest Ashe", 2);
            SkinDictionary.Add("Woad Ashe", 3);
            SkinDictionary.Add("Queen Ashe (Legacy)", 4);
            SkinDictionary.Add("Amethyst Ashe", 5);
            SkinDictionary.Add("Heartseeker Ashe", 6);
            SkinDictionary.Add("Marauder Ashe", 7);
            SkinDictionary.Add("PROJECT: Ashe", 8);

            GenerateMain();
            GenerateGapcloser();
            GenerateInterrupt();
            GenerateFireUlt();
            GenerateCombo();
            GenerateHarass();
            GenerateJungleClear();
            GenerateFlee();

            Initialize();
        }

        private void GenerateMain()
        {
            Main = MainMenu.AddMenu("ClayAshe", "clay_ashe");
            Main.AddGroupLabel("Welcome to ClayAshe!");
            Main.AddGroupLabel("Made by seraphim07");
        }

        #region Gap Closer
        private void GenerateGapcloser()
        {
            Gapcloser = Main.AddSubMenu("Gapcloser", "gapcloser");
            Gapcloser.Add("gapcloser_use_r", new CheckBox("Use R on enemy gap closer", true));
        }
        
        public bool GapcloserUseR
        {
            get
            {
                return (Gapcloser["gapcloser_use_r"] as CheckBox).CurrentValue;
            }
        }
        #endregion

        #region Interrupt
        private void GenerateInterrupt()
        {
            Interrupt = Main.AddSubMenu("Interrupt", "interrupt");
            Interrupt.Add("interrupt_use_r", new CheckBox("Use R to interrupt cast", true));
        }

        public bool InterruptUseR
        {
            get
            {
                return (Interrupt["interrupt_use_r"] as CheckBox).CurrentValue;
            }
        }
        #endregion

        #region Fire Ult
        private void GenerateFireUlt()
        {
            FireUlt = Main.AddSubMenu("Fire Ult", "fire_ult");
            FireUlt.Add("fire_ult_key", new KeyBind("Fire Ult to selected target", false, KeyBind.BindTypes.HoldActive, 'Z'));
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
        
        #region Jungle Clear
        private void GenerateJungleClear()
        {
            JungleClear = Main.AddSubMenu("Jungle Clear", "jungle_clear");
            JungleClear.Add("jungle_clear_use_q", new CheckBox("Use Q", true));
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
