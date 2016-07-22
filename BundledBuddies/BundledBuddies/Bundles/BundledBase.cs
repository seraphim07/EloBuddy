using EloBuddy;
using EloBuddy.SDK;
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
        }

        protected void Initialize()
        {
            Game.OnTick += OnTick;
        }

        private void OnTick(EventArgs e)
        {
            UseItems();

            UseSpells();

            OnTickPermaActive();

            if (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.Combo))
            {
                UseOffensiveItems();

                OnTickCombo();
            }
            
            if (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.Harass))
            {
                OnTickHarass();
            }

            if (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.LaneClear))
            {
                OnTickLaneClear();
            }

            if (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.JungleClear))
            {
                OnTickJungleClear();
            }

            if (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.LastHit))
            {
                OnTickLastHit();
            }

            if (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.Flee))
            {
                OnTickFlee();
            }
        }

        protected virtual void OnTickPermaActive() { }
        protected virtual void OnTickCombo() { }
        protected virtual void OnTickHarass() { }
        protected virtual void OnTickLaneClear() { }
        protected virtual void OnTickJungleClear() { }
        protected virtual void OnTickLastHit() { }
        protected virtual void OnTickFlee() { }

        private void UseDefensiveItems()
        {

            Misc.Add("misc_use_qss", new CheckBox("Use Quick Silver Sash", true));
            Misc.Add("misc_use_ms", new CheckBox("Use Mercurial Scimitar", true));
            Misc.Add("misc_use_potion", new CheckBox("Use Potions", true));
            Misc.Add("misc_use_potion_hp", new Slider("Use potion when <= hp %", 25, 0, 100));
        }

        private void UseOffensiveItems()
        {
            Misc.Add("misc_use_bc", new CheckBox("Use Bilgewater Cutlass", true));
            Misc.Add("misc_use_botrk", new CheckBox("Use Blade of the Ruined King", true));
        }

        private void UseSpells()
        {

        }
    }
}
