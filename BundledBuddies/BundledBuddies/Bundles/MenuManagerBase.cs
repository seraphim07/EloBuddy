using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Menu;
using EloBuddy.SDK.Menu.Values;
using System.Linq;

namespace BundledBuddies.Bundles
{
    class MenuManagerBase
    {
        public Menu Main, Combo, Harass, LaneClear, JungleClear, LastHit, Flee, Misc;

        protected void Initialize()
        {
            GenerateMiscMenu();
        }
        
        private void OnValueChange(ValueBase<int> sender, ValueBase<int>.ValueChangeArgs e)
        {
            ComboBox[] comboBoxes = new ComboBox[]
            {
                Misc["misc_auto_skill_priority_1"] as ComboBox,
                Misc["misc_auto_skill_priority_2"] as ComboBox,
                Misc["misc_auto_skill_priority_3"] as ComboBox
            }.Where(comboBox => !comboBox.Equals(sender as ComboBox)).ToArray();

            int[] possibleValues = new int[] { 0, 1, 2 };

            foreach (ComboBox c in comboBoxes.Where(comboBox => comboBox.CurrentValue.Equals((sender as ComboBox).CurrentValue)))
            {
                ComboBox otherBox = comboBoxes.First(comboBox => !comboBox.Equals(c));
                c.CurrentValue = possibleValues.First(possibleValue => !possibleValue.Equals((sender as ComboBox).CurrentValue) && !possibleValue.Equals(otherBox.CurrentValue));
            }
        }

        private void GenerateMiscMenu()
        {
            Misc = Main.AddSubMenu("Misc", "misc");
            Misc.Add("misc_auto_skill_enable", new CheckBox("Enable auto skill levelup", true));
            ComboBox MiscAutoSkillPriority1 = Misc.Add("misc_auto_skill_priority_1", new ComboBox("Auto skill level up priority 1", 0, "Q", "W", "E"));
            ComboBox MiscAutoSkillPriority2 = Misc.Add("misc_auto_skill_priority_2", new ComboBox("Auto skill level up priority 2", 1, "Q", "W", "E"));
            ComboBox MiscAutoSkillPriority3 = Misc.Add("misc_auto_skill_priority_3", new ComboBox("Auto skill level up priority 3", 2, "Q", "W", "E"));
            MiscAutoSkillPriority1.OnValueChange += OnValueChange;
            MiscAutoSkillPriority2.OnValueChange += OnValueChange;
            MiscAutoSkillPriority3.OnValueChange += OnValueChange;
            Misc.Add("misc_use_qss", new CheckBox("Use Quick Silver Sash", true));
            Misc.Add("misc_use_ms", new CheckBox("Use Mercurial Scimitar", true));
            Misc.Add("misc_use_bc", new CheckBox("Use Bilgewater Cutlass", true));
            Misc.Add("misc_use_botrk", new CheckBox("Use Blade of the Ruined King", true));
            Misc.Add("misc_use_potion", new CheckBox("Use Potions", true));
            Misc.Add("misc_use_potion_hp", new Slider("Use potion when <= hp %", 25, 0, 100));
            
            if (Player.Instance.GetSpellSlotFromName("summonerheal") != SpellSlot.Unknown)
            {
                Misc.Add("misc_use_heal_combo", new CheckBox("Use Heal during Combo mode", true));
                Misc.Add("misc_use_heal_harass", new CheckBox("Use Heal during Harass mode", false));
                Misc.Add("misc_use_heal_flee", new CheckBox("Use Heal during Flee mode", true));
                Misc.Add("misc_use_heal_hp", new Slider("Use Heal when <= hp %", 25, 0, 100));
                Misc.Add("misc_use_heal_ally", new CheckBox("Use Heal to save ally", true));
                Misc.Add("misc_use_heal_ally_hp", new Slider("Use Heal when <= ally's hp %", 25, 0, 100));
            }

            if (Player.Instance.GetSpellSlotFromName("summonerbarrier") != SpellSlot.Unknown)
            {
                Misc.Add("misc_use_barrier_combo", new CheckBox("Use Barrier during Combo mode", true));
                Misc.Add("misc_use_barrier_harass", new CheckBox("Use Barrier during Harass mode", false));
                Misc.Add("misc_use_barrier_flee", new CheckBox("Use Barrier during Flee mode", true));
                Misc.Add("misc_use_barrier_hp", new Slider("Use Barrier when <= hp %", 25, 0, 100));
            }

            if (Player.Instance.GetSpellSlotFromName("summonercleanse") != SpellSlot.Unknown)
            {
                Misc.Add("misc_use_cleanse", new CheckBox("Use Cleanse", true));
            }

            if (Player.Instance.GetSpellSlotFromName("summonerexhaust") != SpellSlot.Unknown)
            {
                Misc.Add("misc_use_exhaust", new CheckBox("Use Exhaust", true));
                Misc.Add("misc_use_exhaust_hp", new Slider("Use Exhaust when <= enemy hp %", 50, 0, 100));
            }

            if (Player.Instance.GetSpellSlotFromName("summonerignite") != SpellSlot.Unknown)
            {
                CheckBox MiscUseIgnite = Misc.Add("misc_use_ignite", new CheckBox("Use Ignite", true));
                MiscUseIgnite.OnValueChange += delegate
                {
                    Misc["misc_use_ignite_killable"].IsVisible = MiscUseIgnite.CurrentValue;
                };
                Misc.Add("misc_use_ignite_hp", new Slider("Use Ignite when <= enemy hp %", 25, 0, 100));
                Misc.Add("misc_use_ignite_killable", new CheckBox("Use Ignite only when killable", true));
            }
        }

        #region Auto Skill Accessors
        public bool IsAutoSkillEnabled
        {
            get
            {
                return (Misc["misc_auto_skill_enable"] as CheckBox).CurrentValue;
            }
        }

        public SpellSlot FirstPrioritySkill
        {
            get
            {
                switch ((Misc["misc_auto_skill_priority_1"] as ComboBox).CurrentValue)
                {
                    case 0:
                        return SpellSlot.Q;
                    case 1:
                        return SpellSlot.W;
                    default:
                        return SpellSlot.E;
                }
            }
        }

        public SpellSlot SecondPrioritySkill
        {
            get
            {
                switch ((Misc["misc_auto_skill_priority_2"] as ComboBox).CurrentValue)
                {
                    case 0:
                        return SpellSlot.Q;
                    case 1:
                        return SpellSlot.W;
                    default:
                        return SpellSlot.E;
                }
            }
        }

        public SpellSlot ThirdPrioritySkill
        {
            get
            {
                switch ((Misc["misc_auto_skill_priority_3"] as ComboBox).CurrentValue)
                {
                    case 0:
                        return SpellSlot.Q;
                    case 1:
                        return SpellSlot.W;
                    default:
                        return SpellSlot.E;
                }
            }
        }
        #endregion

        #region Item Accessors
        public bool UseQss
        {
            get
            {
                return (Misc["misc_use_qss"] as CheckBox).CurrentValue;
            }
        }

        public bool UseMs
        {
            get
            {
                return (Misc["misc_use_ms"] as CheckBox).CurrentValue;
            }
        }

        public bool UsePotion
        {
            get
            {
                return (Misc["misc_use_potion"] as CheckBox).CurrentValue;
            }
        }

        public int PotionHp
        {
            get
            {
                return (Misc["misc_use_potion_hp"] as Slider).CurrentValue;
            }
        }

        public bool UseBc
        {
            get
            {
                return (Misc["misc_use_bc"] as CheckBox).CurrentValue;
            }
        }

        public bool UseBotrk
        {
            get
            {
                return (Misc["misc_use_botrk"] as CheckBox).CurrentValue;
            }
        }
        #endregion

        #region Summoner Spell Accessors
        public bool UseHealCombo
        {
            get
            {
                return Misc["misc_use_heal_combo"] != null ? (Misc["misc_use_heal_combo"] as CheckBox).CurrentValue : false;
            }
        }

        public bool UseHealHarass
        {
            get
            {
                return Misc["misc_use_heal_harass"] != null ? (Misc["misc_use_heal_harass"] as CheckBox).CurrentValue : false;
            }
        }

        public bool UseHealFlee
        {
            get
            {
                return Misc["misc_use_heal_flee"] != null ? (Misc["misc_use_heal_flee"] as CheckBox).CurrentValue : false;
            }
        }

        public int UseHealHp
        {
            get
            {
                return Misc["misc_use_heal_hp"] != null ? (Misc["misc_use_heal_hp"] as Slider).CurrentValue : 0;
            }
        }

        public bool UseHealAlly
        {
            get
            {
                return Misc["misc_use_heal_ally"] != null ? (Misc["misc_use_heal_ally"] as CheckBox).CurrentValue : false;
            }
        }

        public int UseHealAllyHp
        {
            get
            {
                return Misc["misc_use_heal_ally_hp"] != null ? (Misc["misc_use_heal_ally_hp"] as Slider).CurrentValue : 0;
            }
        }
        
        public bool UseBarrierCombo
        {
            get
            {
                return Misc["misc_use_barrier_combo"] != null ? (Misc["misc_use_barrier_combo"] as CheckBox).CurrentValue : false;
            }
        }

        public bool UseBarrierHarass
        {
            get
            {
                return Misc["misc_use_barrier_harass"] != null ? (Misc["misc_use_barrier_harass"] as CheckBox).CurrentValue : false;
            }
        }

        public bool UseBarrierFlee
        {
            get
            {
                return Misc["misc_use_barrier_flee"] != null ? (Misc["misc_use_barrier_flee"] as CheckBox).CurrentValue : false;
            }
        }

        public int UseBarrierHp
        {
            get
            {
                return Misc["misc_use_barrier_hp"] != null ? (Misc["misc_use_barrier_hp"] as Slider).CurrentValue : 0;
            }
        }
        
        public bool UseCleanse
        {
            get
            {
                return Misc["misc_use_cleanse"] != null ? (Misc["misc_use_cleanse"] as CheckBox).CurrentValue : false;
            }
        }

        public bool UseExhaust
        {
            get
            {
                return Misc["misc_use_exhaust"] != null ? (Misc["misc_use_exhaust"] as CheckBox).CurrentValue : false;
            }
        }

        public int UseExhaustHp
        {
            get
            {
                return Misc["misc_use_exhaust_hp"] != null ? (Misc["misc_use_exhaust_hp"] as Slider).CurrentValue : 0;
            }
        }

        public bool UseIgnite
        {
            get
            {
                return Misc["misc_use_ignite"] != null ? (Misc["misc_use_ignite"] as CheckBox).CurrentValue : false;
            }
        }

        public int UseIgniteHp
        {
            get
            {
                return Misc["misc_use_ignite_hp"] != null ? (Misc["misc_use_ignite_hp"] as Slider).CurrentValue : 0;
            }
        }

        public bool UseIgniteKillable
        {
            get
            {
                return Misc["misc_use_ignite_killable"] != null ? (Misc["misc_use_ignite_killable"] as CheckBox).CurrentValue : false;
            }
        }
        #endregion
    }
}
