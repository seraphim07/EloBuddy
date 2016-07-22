using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Menu;
using EloBuddy.SDK.Menu.Values;
using System.Collections;
using System.Linq;

namespace BundledBuddies.Bundles
{
    class MenuManagerBase
    {
        public Menu Main, Combo, Harass, LaneClear, JungleClear, LastHit, Flee, Misc;

        protected void Initialize()
        {
            generateMiscMenu();
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

        private void generateMiscMenu()
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
                Misc.Add("misc_use_heal", new CheckBox("Use Heal", true));
                Misc.Add("misc_use_heal_hp", new Slider("Use Heal when <= hp %", 25, 0, 100));
                Misc.Add("misc_use_heal_ally", new CheckBox("Use Heal to save ally", true));
                Misc.Add("misc_use_heal_ally_hp", new Slider("Use Heal when <= ally's hp %", 25, 0, 100));
            }

            if (Player.Instance.GetSpellSlotFromName("summonerbarrier") != SpellSlot.Unknown)
            {
                Misc.Add("misc_use_barrier", new CheckBox("Use Barrier", true));
                Misc.Add("misc_use_barrier_hp", new Slider("Use Barrier when <= hp %", 25, 0, 100));
            }

            if (Player.Instance.GetSpellSlotFromName("summonercleanse") != SpellSlot.Unknown)
            {
                Misc.Add("misc_use_cleanse", new CheckBox("Use Cleanse", true));
            }

            if (Player.Instance.GetSpellSlotFromName("summonerexhaust") != SpellSlot.Unknown)
            {
                Misc.Add("misc_use_exhaust", new CheckBox("Use Exhaust", true));
            }

            if (Player.Instance.GetSpellSlotFromName("summonerignite") != SpellSlot.Unknown)
            {
                CheckBox MiscUseIgnite = Misc.Add("misc_use_ignite", new CheckBox("Use Ignite", true));
                MiscUseIgnite.OnValueChange += delegate
                {
                    Misc["misc_use_ignite_killable"].IsVisible = MiscUseIgnite.CurrentValue;
                };
                Misc.Add("misc_use_ignite_killable", new CheckBox("Use Ignite only when killable", true));
            }
        }
        
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
    }
}
