using ClayAIO.ClayScripts.Tryndamere;
using EloBuddy;
using EloBuddy.SDK.Events;

namespace ClayAIO.ClayScripts
{
    class ClayTryndamere : ClayBase
    {
        private MenuManager menuManager;
        private SpellManager spellManager;

        public ClayTryndamere() : base()
        {
            primaryDamageType = DamageType.Physical;

            menuManagerBase = new MenuManager();
            spellManagerBase = new SpellManager();

            menuManager = menuManagerBase as MenuManager;
            spellManager = spellManagerBase as SpellManager;

            Initialize();
            
            Drawing.OnDraw += spellManager.OnDraw;

            Chat.Print("ClayTryndamere loaded!");
        }
        
        protected override void OnTickPermaActive()
        {
            if (!spellManager.IsRActive)
            {
                if (menuManager.PermaActiveUseQ &&
                    Player.Instance.HealthPercent <= menuManager.PermaActiveQHp)
                {
                    spellManager.Q.Cast();

                    return;
                }

                if (menuManager.PermaActiveUseR &&
                    Player.Instance.HealthPercent <= menuManager.PermaActiveRHp)
                {
                    spellManager.R.Cast();

                    return;
                }
            }

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
            if (menuManager.ComboUseW) spellManager.W.Cast();
            if (menuManager.ComboUseE) spellManager.CastEToHero();

            base.OnTickCombo();
        }
        
        protected override void OnTickHarass()
        {
            base.OnTickHarass();
        }

        protected override void OnTickLaneClear()
        {
            if (menuManager.LaneClearUseE) spellManager.CastEToMinion();

            base.OnTickLaneClear();
        }
        
        protected override void OnTickJungleClear()
        {
            if (menuManager.JungleClearUseE) spellManager.CastEToJungle();
            base.OnTickJungleClear();
        }

        protected override void OnTickLastHit()
        {
            base.OnTickLastHit();
        }

        protected override void OnTickFlee()
        {
            if (menuManager.JungleClearUseE)
            {
                spellManager.E.Cast(Game.CursorPos);
            }

            base.OnTickFlee();
        }
    }
}
