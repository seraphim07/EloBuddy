using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Menu;
using EloBuddy.SDK.Menu.Values;
using SharpDX;
using System;

namespace BundledBuddies.Bundles
{
    class BundledBase
    {
        protected ColorBGRA indianRed;
        protected ColorBGRA mediumPurple;
        protected ColorBGRA darkRed;
        protected ColorBGRA darkBlue;
        
        public BundledBase()
        {
            indianRed = new ColorBGRA(Color.IndianRed.R, Color.IndianRed.G, Color.IndianRed.B, 127);
            mediumPurple = new ColorBGRA(Color.MediumPurple.R, Color.MediumPurple.G, Color.MediumPurple.B, 127);
            darkRed = new ColorBGRA(Color.DarkRed.R, Color.DarkRed.G, Color.DarkRed.B, 127);
            darkBlue = new ColorBGRA(Color.DarkBlue.R, Color.DarkBlue.G, Color.DarkBlue.B, 127);
            
            Drawing.OnDraw += OnDraw;
            Game.OnTick += OnTick;
            Orbwalker.OnPostAttack += OnPostAttack;
            Messages.OnMessage += OnMessage;
            Obj_AI_Base.OnLevelUp += OnLevelUp;
        }

        protected virtual void OnDraw(EventArgs e) { }
        protected virtual void OnTick(EventArgs e) { }
        protected virtual void OnPostAttack(AttackableUnit target, EventArgs e) { }
        protected virtual void OnMessage(Messages.WindowMessage message) { }
        protected virtual void OnLevelUp(Obj_AI_Base sender, Obj_AI_BaseLevelUpEventArgs e) { }
        

        public static Menu GenerateMiscMenu(Menu TopMenu)
        {
            Menu MiscMenu = TopMenu.AddSubMenu("Misc", "misc");
            MiscMenu.Add("misc_auto_skill_enable", new CheckBox("Enable auto skill levelup", true));
            ComboBox MiscAutoSkillPriority1 = MiscMenu.Add("misc_auto_skill_priority_1", new ComboBox("Auto skill level up priority 1", 0, "Q", "W", "E"));
            MiscAutoSkillPriority1.OnValueChange += delegate
            {
                int currentValue = MiscAutoSkillPriority1.CurrentValue;
                int nextValue = (currentValue + 1) % 3;

                if ((MiscMenu["misc_auto_skill_priority_2"] as ComboBox).CurrentValue == currentValue)
                {
                    (MiscMenu["misc_auto_skill_priority_2"] as ComboBox).CurrentValue = nextValue;
                }
            };
            ComboBox MiscAutoSkillPriority2 = MiscMenu.Add("misc_auto_skill_priority_2", new ComboBox("Auto skill level up priority 2", 1, "Q", "W", "E"));
            MiscAutoSkillPriority2.OnValueChange += delegate
            {
                int currentValue = MiscAutoSkillPriority2.CurrentValue;
                int nextValue = (currentValue + 1) % 3;

                if ((MiscMenu["misc_auto_skill_priority_1"] as ComboBox).CurrentValue == currentValue)
                {
                    (MiscMenu["misc_auto_skill_priority_1"] as ComboBox).CurrentValue = nextValue;
                }
            };
            MiscMenu.Add("misc_use_qss", new CheckBox("Use Quick Silver Sash", true));
            MiscMenu.Add("misc_use_ms", new CheckBox("Use Mercurial Scimitar", true));
            MiscMenu.Add("misc_use_bc", new CheckBox("Use Bilgewater Cutlass", true));
            MiscMenu.Add("misc_use_botrk", new CheckBox("Use Blade of the Ruined King", true));
            MiscMenu.Add("misc_use_potion", new CheckBox("Use Potions", true));
            MiscMenu.Add("misc_use_potion_hp", new Slider("Use potion when <= hp %", 25, 0, 100));

            if (Player.Instance.GetSpellSlotFromName("summonerheal") != SpellSlot.Unknown)
            {
                MiscMenu.Add("misc_use_heal", new CheckBox("Use Heal", true));
                MiscMenu.Add("misc_use_heal_hp", new Slider("Use Heal when <= hp %", 25, 0, 100));
                MiscMenu.Add("misc_use_heal_ally", new CheckBox("Use Heal to save ally", true));
                MiscMenu.Add("misc_use_heal_ally_hp", new Slider("Use Heal when <= ally's hp %", 25, 0, 100));
            }

            if (Player.Instance.GetSpellSlotFromName("summonerbarrier") != SpellSlot.Unknown)
            {
                MiscMenu.Add("misc_use_barrier", new CheckBox("Use Barrier", true));
                MiscMenu.Add("misc_use_barrier_hp", new Slider("Use Barrier when <= hp %", 25, 0, 100));
            }

            if (Player.Instance.GetSpellSlotFromName("summonercleanse") != SpellSlot.Unknown)
            {
                MiscMenu.Add("misc_use_cleanse", new CheckBox("Use Cleanse", true));
            }

            if (Player.Instance.GetSpellSlotFromName("summonerexhaust") != SpellSlot.Unknown)
            {
                MiscMenu.Add("misc_use_exhaust", new CheckBox("Use Exhaust", true));
            }

            if (Player.Instance.GetSpellSlotFromName("summonerignite") != SpellSlot.Unknown)
            {
                CheckBox MiscUseIgnite = MiscMenu.Add("misc_use_ignite", new CheckBox("Use Ignite", true));
                MiscUseIgnite.OnValueChange += delegate
                {
                    MiscMenu["misc_use_ignite_killable"].IsVisible = MiscUseIgnite.CurrentValue;
                };
                MiscMenu.Add("misc_use_ignite_killable", new CheckBox("Use Ignite only when killable", true));
            }

            return MiscMenu;
        }
    }
}
