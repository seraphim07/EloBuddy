using BundledBuddies.Bundles.Annie;
using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Rendering;
using System;
using System.Collections;

namespace BundledBuddies.Bundles
{
    partial class BundledAnnie : BundledBase
    {
        private MenuManager menuManager;
        private SpellManager spellManager;

        public BundledAnnie() : base()
        {
            menuManagerBase = new MenuManager();
            spellManagerBase = new SpellManager();

            menuManager = menuManagerBase as MenuManager;
            spellManager = spellManagerBase as SpellManager;

            primaryDamageType = DamageType.Magical;

            Initialize();

            Drawing.OnDraw += OnDraw;

            Chat.Print("BundledAnnie loaded!");
            
        }
        
        private void OnDraw(EventArgs e)
        {
            Circle.Draw(indianRed, spellManager.Q.Range, Player.Instance);
            Circle.Draw(mediumPurple, spellManager.W.Range, Player.Instance);
            Circle.Draw(darkRed, spellManager.R.Range, Player.Instance);
        }

        protected override void OnTickPermaActive()
        {
            base.OnTickPermaActive();
        }

        protected override void OnTickCombo()
        {
            
        }

        protected override void OnTickHarass()
        {
            
        }

        protected override void OnTickLaneClear()
        {
            LastHitQ();

            if (menuManager.LaneClearUseW &&
                Player.Instance.ManaPercent >= menuManager.LaneClearWMana &&
                spellManager.W.IsReady())
            {
                spellManager.W.CastOnBestFarmPosition(menuManager.LaneClearWNumber);
            }
        }

        protected override void OnTickJungleClear()
        {
            
        }

        protected override void OnTickLastHit()
        {
            if (menuManager.LastHitUseQ)
            {
                LastHitQ();
            }

            base.OnTickLastHit();
        }

        protected override void OnTickFlee()
        {
            
        }

        private void LastHitQ()
        {
            if (spellManager.Q.IsReady())
            {
                foreach (Obj_AI_Minion minion in EntityManager.MinionsAndMonsters.GetLaneMinions(EntityManager.UnitTeam.Enemy, Player.Instance.ServerPosition, spellManager.Q.Range, false))
                {
                    if (Player.Instance.GetAutoAttackDamage(minion) < minion.Health && spellManager.Q.GetHealthPrediction(minion) <= 0.0f)
                    {
                        spellManager.Q.Cast(minion);
                    }
                }
            }
        }
    }
}