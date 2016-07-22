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
            Drawing.OnDraw += OnDraw;
            Game.OnTick += OnTick;
            Orbwalker.OnPostAttack += OnPostAttack;
            Messages.OnMessage += OnMessage;
            Obj_AI_Base.OnLevelUp += OnLevelUp;
        }

        private void OnTick(EventArgs e)
        {
            OnTickPermaActive();

            if (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.Combo))
            {
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

        protected virtual void OnDraw(EventArgs e) { }
        protected virtual void OnPostAttack(AttackableUnit target, EventArgs e) { }
        protected virtual void OnMessage(Messages.WindowMessage message) { }
        protected virtual void OnLevelUp(Obj_AI_Base sender, Obj_AI_BaseLevelUpEventArgs e) { }
    }
}
