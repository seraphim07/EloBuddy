using EloBuddy;
using EloBuddy.SDK.Menu;
using EloBuddy.SDK.Menu.Values;
using System.Collections.Generic;
using System.Linq;

namespace ClayAIO.ClayScripts.Annie
{
    class MenuManager : MenuManagerBase
    {
        public Menu Initiator, PermaActive;

        public MenuManager()
        {
            SkinDictionary = new Dictionary<string, int>();
            SkinDictionary.Add("Classic", 0);
            SkinDictionary.Add("Unknown", 1);

            GenerateMain();
            GenerateGapcloser();
            GenerateInterrupt();
            GenerateInitiator();
            GeneratePermaActive();
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
            Main = MainMenu.AddMenu("ClayAnnie", "clay_annie");
            Main.AddGroupLabel("Welcome to ClayAnnie!");
            Main.AddGroupLabel("Made by seraphim07");
        }

        #region Gapcloser
        private void OnGapcloserPriorityValueChange(ValueBase<int> sender, ValueBase<int>.ValueChangeArgs e)
        {
            ComboBox[] comboBoxes = new ComboBox[]
            {
                Gapcloser["gapcloser_priority_1"] as ComboBox,
                Gapcloser["gapcloser_priority_2"] as ComboBox,
                Gapcloser["gapcloser_priority_3"] as ComboBox
            }.Where(comboBox => !comboBox.Equals(sender as ComboBox)).ToArray();

            int[] possibleValues = new int[] { 0, 1, 2 };

            foreach (ComboBox c in comboBoxes.Where(comboBox => comboBox.CurrentValue.Equals((sender as ComboBox).CurrentValue)))
            {
                ComboBox otherBox = comboBoxes.First(comboBox => !comboBox.Equals(c));
                c.CurrentValue = possibleValues.First(possibleValue =>
                    !possibleValue.Equals((sender as ComboBox).CurrentValue) &&
                    !possibleValue.Equals(otherBox.CurrentValue));
            }
        }

        private void GenerateGapcloser()
        {
            Gapcloser = Main.AddSubMenu("Gapcloser", "gapcloser");
            Gapcloser.Add("gapcloser_use_q", new CheckBox("Use Q to stun enemy gap closer", true));
            Gapcloser.Add("gapcloser_use_w", new CheckBox("Use W to stun enemy gap closer", true));
            Gapcloser.Add("gapcloser_use_r", new CheckBox("Use R to stun enemy gap closer", true));
            ComboBox gapcloserPriority1 = Gapcloser.Add("gapcloser_priority_1", new ComboBox("Preferred skill to stun gap closer (1st priority)", 0, "Q", "W", "R"));
            ComboBox gapcloserPriority2 = Gapcloser.Add("gapcloser_priority_2", new ComboBox("Preferred skill to stun gap closer (2nd priority)", 1, "Q", "W", "R"));
            ComboBox gapcloserPriority3 = Gapcloser.Add("gapcloser_priority_3", new ComboBox("Preferred skill to stun gap closer (3rd priority)", 2, "Q", "W", "R"));
            gapcloserPriority1.OnValueChange += OnGapcloserPriorityValueChange;
            gapcloserPriority2.OnValueChange += OnGapcloserPriorityValueChange;
            gapcloserPriority3.OnValueChange += OnGapcloserPriorityValueChange;
        }

        public bool GapcloserUseQ
        {
            get
            {
                return (Gapcloser["gapcloser_use_q"] as CheckBox).CurrentValue;
            }
        }

        public bool GapcloserUseW
        {
            get
            {
                return (Gapcloser["gapcloser_use_w"] as CheckBox).CurrentValue;
            }
        }

        public bool GapcloserUseR
        {
            get
            {
                return (Gapcloser["gapcloser_use_r"] as CheckBox).CurrentValue;
            }
        }

        public SpellSlot GapcloserFirstPrioritySkill
        {
            get
            {
                switch ((Gapcloser["gapcloser_priority_1"] as ComboBox).CurrentValue)
                {
                    case 0:
                        return SpellSlot.Q;
                    case 1:
                        return SpellSlot.W;
                    default:
                        return SpellSlot.R;
                }
            }
        }

        public SpellSlot GapcloserSecondPrioritySkill
        {
            get
            {
                switch ((Gapcloser["gapcloser_priority_2"] as ComboBox).CurrentValue)
                {
                    case 0:
                        return SpellSlot.Q;
                    case 1:
                        return SpellSlot.W;
                    default:
                        return SpellSlot.R;
                }
            }
        }

        public SpellSlot GapcloserThirdPrioritySkill
        {
            get
            {
                switch ((Gapcloser["gapcloser_priority_3"] as ComboBox).CurrentValue)
                {
                    case 0:
                        return SpellSlot.Q;
                    case 1:
                        return SpellSlot.W;
                    default:
                        return SpellSlot.R;
                }
            }
        }
        #endregion

        #region Interrupt
        private void OnInterruptPriorityValueChange(ValueBase<int> sender, ValueBase<int>.ValueChangeArgs e)
        {
            ComboBox[] comboBoxes = new ComboBox[]
            {
                Interrupt["interrupt_priority_1"] as ComboBox,
                Interrupt["interrupt_priority_2"] as ComboBox,
                Interrupt["interrupt_priority_3"] as ComboBox
            }.Where(comboBox => !comboBox.Equals(sender as ComboBox)).ToArray();

            int[] possibleValues = new int[] { 0, 1, 2 };

            foreach (ComboBox c in comboBoxes.Where(comboBox => comboBox.CurrentValue.Equals((sender as ComboBox).CurrentValue)))
            {
                ComboBox otherBox = comboBoxes.First(comboBox => !comboBox.Equals(c));
                c.CurrentValue = possibleValues.First(possibleValue =>
                    !possibleValue.Equals((sender as ComboBox).CurrentValue) &&
                    !possibleValue.Equals(otherBox.CurrentValue));
            }
        }

        private void GenerateInterrupt()
        {
            Interrupt = Main.AddSubMenu("Interrupt", "interrupt");
            Interrupt.Add("interrupt_use_q", new CheckBox("Use Q to interrupt cast", true));
            Interrupt.Add("interrupt_use_w", new CheckBox("Use W to interrupt cast", true));
            Interrupt.Add("interrupt_use_r", new CheckBox("Use R to interrupt cast", true));
            ComboBox interruptPriority1 = Interrupt.Add("interrupt_priority_1", new ComboBox("Preferred skill to interrupt cast (1st priority)", 0, "Q", "W", "R"));
            ComboBox interruptPriority2 = Interrupt.Add("interrupt_priority_2", new ComboBox("Preferred skill to interrupt cast (2nd priority)", 1, "Q", "W", "R"));
            ComboBox interruptPriority3 = Interrupt.Add("interrupt_priority_3", new ComboBox("Preferred skill to interrupt cast (3rd priority)", 2, "Q", "W", "R"));
            interruptPriority1.OnValueChange += OnInterruptPriorityValueChange;
            interruptPriority2.OnValueChange += OnInterruptPriorityValueChange;
            interruptPriority3.OnValueChange += OnInterruptPriorityValueChange;
        }

        public bool InterruptUseQ
        {
            get
            {
                return (Interrupt["interrupt_use_q"] as CheckBox).CurrentValue;
            }
        }
        
        public bool InterruptUseW
        {
            get
            {
                return (Interrupt["interrupt_use_w"] as CheckBox).CurrentValue;
            }
        }

        public bool InterruptUseR
        {
            get
            {
                return (Interrupt["interrupt_use_r"] as CheckBox).CurrentValue;
            }
        }

        public SpellSlot InterruptFirstPrioritySkill
        {
            get
            {
                switch ((Interrupt["interrupt_priority_1"] as ComboBox).CurrentValue)
                {
                    case 0:
                        return SpellSlot.Q;
                    case 1:
                        return SpellSlot.W;
                    default:
                        return SpellSlot.R;
                }
            }
        }

        public SpellSlot InterruptSecondPrioritySkill
        {
            get
            {
                switch ((Interrupt["interrupt_priority_2"] as ComboBox).CurrentValue)
                {
                    case 0:
                        return SpellSlot.Q;
                    case 1:
                        return SpellSlot.W;
                    default:
                        return SpellSlot.R;
                }
            }
        }

        public SpellSlot InterruptThirdPrioritySkill
        {
            get
            {
                switch ((Interrupt["interrupt_priority_3"] as ComboBox).CurrentValue)
                {
                    case 0:
                        return SpellSlot.Q;
                    case 1:
                        return SpellSlot.W;
                    default:
                        return SpellSlot.R;
                }
            }
        }
        #endregion

        #region Initiator
        private void GenerateInitiator()
        {
            Initiator = Main.AddSubMenu("Initiator", "initiator");
            Initiator.Add("initiator_key", new KeyBind("Initiate with stun ult", false, KeyBind.BindTypes.HoldActive, 'Z'));
            Initiator.Add("initiator_num_enemies", new Slider("Initiate when ult can stun >= enemies", 2, 0, 5));
        }

        public bool InitiatorKey
        {
            get
            {
                return (Initiator["initiator_key"] as KeyBind).CurrentValue;
            }
        }

        public int InitiatorNumEnemies
        {
            get
            {
                return (Initiator["initiator_num_enemies"] as Slider).CurrentValue;
            }
        }
        #endregion

        #region Perma Active
        private void GeneratePermaActive()
        {
            PermaActive = Main.AddSubMenu("Perma Active", "perma_active");
            PermaActive.Add("perma_active_use_e", new CheckBox("Use E to stack stun", true));
        }

        public bool PermaActiveUseE
        {
            get
            {
                return (PermaActive["perma_active_use_e"] as CheckBox).CurrentValue;
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
            Harass.Add("harass_use_q", new CheckBox("Use Q", true));
            Harass.Add("harass_q_mana", new Slider("Use Q when >= mana %", 50, 0, 100));
            Harass.Add("harass_use_w", new CheckBox("Use W", true));
            Harass.Add("harass_w_mana", new Slider("Use W when >= mana %", 50, 0, 100));
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
        #endregion

        #region Lane Clear
        private void GenerateLaneClear()
        {
            LaneClear = Main.AddSubMenu("Lane Clear", "lane_clear");
            LaneClear.Add("lane_clear_use_q", new CheckBox("Use Q", true));
            LaneClear.Add("lane_clear_use_w", new CheckBox("Use W", true));
            LaneClear.Add("lane_clear_w_mana", new Slider("Use W when >= mana %", 75, 0, 100));
            LaneClear.Add("lane_clear_w_minions", new Slider("Use W when >= minions hit", 3, 0, 10));
        }

        public bool LaneClearUseQ
        {
            get
            {
                return (LaneClear["lane_clear_use_q"] as CheckBox).CurrentValue;
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

        public int LaneClearWMinions
        {
            get
            {
                return (LaneClear["lane_clear_w_minions"] as Slider).CurrentValue;
            }
        }
        #endregion

        #region Jungle Clear
        private void GenerateJungleClear()
        {
            JungleClear = Main.AddSubMenu("Jungle Clear", "jungle_clear");
            JungleClear.Add("jungle_clear_use_q", new CheckBox("Use Q", true));
            JungleClear.Add("jungle_clear_use_w", new CheckBox("Use W", true));
            JungleClear.Add("jungle_clear_use_e", new CheckBox("Use E", true));
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
            Flee.Add("flee_stun", new CheckBox("Use any available skill to stun chasers", true));
        }

        public bool FleeStun
        {
            get
            {
                return (Flee["flee_stun"] as CheckBox).CurrentValue;
            }
        }
        #endregion
    }
}
