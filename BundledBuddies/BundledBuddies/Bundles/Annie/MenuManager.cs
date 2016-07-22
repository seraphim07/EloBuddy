using EloBuddy;
using EloBuddy.SDK.Menu;
using EloBuddy.SDK.Menu.Values;

namespace BundledBuddies.Bundles.Annie
{
    class MenuManager : MenuManagerBase
    {
        public static Menu Initiator;

        public MenuManager() : base()
        {
            Chat.Print("MENU");
            generateMain();
            generateInitiator();
            generateCombo();
            generateHarass();
            generateLaneClear();
            generateJungleClear();
            generateLastHit();
            generateFlee();
        }

        private void generateMain()
        {
            Main = MainMenu.AddMenu("Bundled Annie", "bundled_annie");
            Main.AddGroupLabel("Welcome to Bundled Annie!");
            Main.AddGroupLabel("Made by seraphim07");
            Main.AddGroupLabel("Feel free to send email to seraphim_07@hotmail.com for bug reports, ideas for improvements, etc!");
        }

        private void generateInitiator()
        {
            Initiator = Main.AddSubMenu("Initiator", "initiator");
            Initiator.Add("initiator_key", new KeyBind("Initiator", false, KeyBind.BindTypes.HoldActive, 'Z'));
            Initiator.Add("initiator_condition", new Slider("Initiate when >= enemies are in your R range", 2, 1, 5));
            Initiator.Add("initiator_initiate_when_stun", new CheckBox("Only initiate when you have stun", true));
            Initiator.Add("initiator_use_w", new CheckBox("Use W to stack stun", true));
        }

        private void generateCombo()
        {
            Combo = Main.AddSubMenu("Combo", "combo");
            Combo.Add("combo_use_q", new CheckBox("Use Q", true));
            Combo.Add("combo_use_w", new CheckBox("Use W", true));
            Combo.Add("combo_use_e", new CheckBox("Use E", true));
            Combo.Add("combo_use_r", new CheckBox("Use R", true));
        }

        private void generateHarass()
        {
            Harass = Main.AddSubMenu("Harass", "harass");
            Harass.Add("harass_use_w_stun", new CheckBox("Use W to stack stun", false));
            Harass.Add("harass_use_e_stun", new CheckBox("Use E to stack stun", false));
            Harass.Add("harass_use_q", new CheckBox("Use Q", true));
            Harass.Add("harass_use_w", new CheckBox("Use W", true));
            Harass.Add("harass_use_e", new CheckBox("Use E", false));
        }

        private void generateLaneClear()
        {
            LaneClear = Main.AddSubMenu("Lane Clear", "lane_clear");
            LaneClear.Add("lane_clear_use_w", new CheckBox("Use W", true));
            LaneClear.Add("lane_clear_w_mana", new Slider("Use W when >= mana %", 50, 0, 100));
        }

        private void generateJungleClear()
        {
            JungleClear = Main.AddSubMenu("Jungle Clear", "jungle_clear");
            JungleClear.Add("jungle_clear_use_q_without_last_hit", new CheckBox("Use Q even when it's not last hit", true));
            JungleClear.Add("jungle_clear_use_w", new CheckBox("Use W", true));
            JungleClear.Add("jungle_clear_use_e", new CheckBox("Use E", true));
        }

        private void generateLastHit()
        {
            LastHit = Main.AddSubMenu("Last Hit", "last_hit");
            LastHit.Add("last_hit_use_q", new CheckBox("Use Q", true));
        }

        private void generateFlee()
        {
            Flee = Main.AddSubMenu("Flee", "flee");
            Flee.Add("flee_use_q_stun", new CheckBox("Use Q to stun", true));
        }
    }
}
