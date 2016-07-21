using EloBuddy;
using EloBuddy.SDK.Menu;
using EloBuddy.SDK.Menu.Values;

namespace BundledBuddies.Bundles.Annie
{
    static class MenuManager
    {
        public static Menu BundledAnnie, Initiator, Combo, Harass, LaneClear, JungleClear, Flee, Misc;

        public static void Initialize()
        {
            BundledAnnie = MainMenu.AddMenu("Bundled Annie", "bundled_annie");
            BundledAnnie.AddGroupLabel("Welcome to Bundled Annie!");
            BundledAnnie.AddGroupLabel("Made by seraphim07");
            BundledAnnie.AddGroupLabel("Feel free to send email to seraphim_07@hotmail.com for bug reports, ideas for improvements, etc!");

            Initiator = BundledAnnie.AddSubMenu("Initiator", "initiator");
            Initiator.Add("initiator_key", new KeyBind("Initiator", false, KeyBind.BindTypes.HoldActive, 'Z'));
            Initiator.Add("initiator_condition", new Slider("Initiate when >= enemies are in your R range", 2, 1, 5));
            Initiator.Add("initiator_initiate_when_stun", new CheckBox("Only initiate when you have stun", true));
            Initiator.Add("initiator_use_w", new CheckBox("Use W to stack stun", true));

            Combo = BundledAnnie.AddSubMenu("Combo", "combo");
            Combo.Add("combo_use_q", new CheckBox("Use Q", true));
            Combo.Add("combo_use_w", new CheckBox("Use W", true));
            Combo.Add("combo_use_e", new CheckBox("Use E", true));
            Combo.Add("combo_use_r", new CheckBox("Use R", true));

            Harass = BundledAnnie.AddSubMenu("Harass", "harass");
            Harass.Add("harass_use_w_stun", new CheckBox("Use W to stack stun", false));
            Harass.Add("harass_use_e_stun", new CheckBox("Use E to stack stun", false));
            Harass.Add("harass_use_q", new CheckBox("Use Q", true));
            Harass.Add("harass_use_w", new CheckBox("Use W", true));
            Harass.Add("harass_use_e", new CheckBox("Use E", false));

            LaneClear = BundledAnnie.AddSubMenu("Lane Clear", "lane_clear");
            LaneClear.Add("lane_clear_use_w", new CheckBox("Use W", true));
            LaneClear.Add("lane_clear_w_mana", new Slider("Use W when >= mana %", 50, 0, 100));

            JungleClear = BundledAnnie.AddSubMenu("Jungle Clear", "jungle_clear");
            JungleClear.Add("jungle_clear_use_q_without_last_hit", new CheckBox("Use Q even when it's not last hit", true));
            JungleClear.Add("jungle_clear_use_w", new CheckBox("Use W", true));
            JungleClear.Add("jungle_clear_use_e", new CheckBox("Use E", true));

            Flee = BundledAnnie.AddSubMenu("Flee", "flee");
            Flee.Add("flee_use_q_stun", new CheckBox("Use Q to stun", true));

            Misc = BundledBase.GenerateMiscMenu(BundledAnnie);
        }

        public static bool IsAutoSkillEnabled
        {
            get
            {
                return (Misc["misc_auto_skill_enable"] as CheckBox).CurrentValue;
            }
        }

        public static SpellSlot FirstPrioritySkill
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

        public static SpellSlot SecondPrioritySkill
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
    }
}
