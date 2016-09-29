using ClayAIO.ClayScripts.Annie;
using EloBuddy;
using EloBuddy.SDK.Events;

namespace ClayAIO.ClayScripts
{
    class ClayAnnie : ClayBase
    {
        private MenuManager menuManager;
        private SpellManager spellManager;

        public ClayAnnie() : base()
        {
            primaryDamageType = DamageType.Magical;

            menuManagerBase = new MenuManager();
            spellManagerBase = new SpellManager();

            menuManager = menuManagerBase as MenuManager;
            spellManager = spellManagerBase as SpellManager;

            Initialize();

            Drawing.OnDraw += spellManager.OnDraw;

            Chat.Print("ClayAnnie loaded!");
        }

        protected override void OnTickPermaActive()
        {
            base.OnTickPermaActive();
        }

        protected override void OnGapcloser(AIHeroClient sender, Gapcloser.GapcloserEventArgs e)
        {
            base.OnGapcloser(sender, e);
        }

        protected override void OnInterruptableSpell(Obj_AI_Base sender, Interrupter.InterruptableSpellEventArgs e)
        {
            base.OnInterruptableSpell(sender, e);
        }

        protected override void OnTickCombo()
        {
            base.OnTickCombo();
        }

        protected override void OnTickHarass()
        {
            base.OnTickHarass();
        }

        protected override void OnTickLaneClear()
        {
            base.OnTickLaneClear();
        }

        protected override void OnTickJungleClear()
        {
            base.OnTickJungleClear();
        }

        protected override void OnTickLastHit()
        {
            base.OnTickLastHit();
        }

        protected override void OnTickFlee()
        {
            base.OnTickFlee();
        }
    }
}
