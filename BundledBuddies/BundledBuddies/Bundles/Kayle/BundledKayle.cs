using BundledBuddies.Bundles.Kayle;
using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BundledBuddies.Bundles
{
    partial class BundledKayle : BundledBase
    {
        private MenuManager menuManager;
        private SpellManager spellManager;
        
        public BundledKayle() : base()
        {
            menuManagerBase = new MenuManager();
            spellManagerBase = new SpellManager();

            menuManager = menuManagerBase as MenuManager;
            spellManager = spellManagerBase as SpellManager;

            primaryDamageType = DamageType.Magical;

            Initialize();

            Drawing.OnDraw += OnDraw;

            Chat.Print("BundledKayle loaded!");
            
        }
        
        private void OnDraw(EventArgs e)
        {
            Circle.Draw(indianRed, spellManager.Q.Range, Player.Instance);
            Circle.Draw(mediumPurple, spellManager.W.Range, Player.Instance);
            Circle.Draw(darkRed, SpellManager.E_RANGE, Player.Instance);
            Circle.Draw(darkBlue, spellManager.R.Range, Player.Instance);
        }
        
        protected override void OnTickPermaActive()
        {
            base.OnTickPermaActive();
        }
        
        protected override void OnTickCombo()
        {
            if (menuManager.ComboUseQ &&
                spellManager.Q.IsReady())
            {
                AIHeroClient target = TargetSelector.GetTarget(spellManager.Q.Range, primaryDamageType);

                if (target != null)
                {
                    spellManager.Q.Cast(target);
                }
            }

            if (menuManager.ComboUseW &&
                spellManager.W.IsReady() &&
                Player.Instance.HealthPercent <= menuManager.ComboWHp)
            {
                spellManager.W.Cast(Player.Instance);
            }

            if (menuManager.ComboUseE &&
                spellManager.E.IsReady() &&
                EntityManager.Heroes.Enemies.Count(x => x.Distance(Player.Instance) <= SpellManager.E_RANGE) > 0)
            {
                spellManager.E.Cast();
            }

            if (menuManager.ComboUseR &&
                spellManager.R.IsReady())
            {
                List<AIHeroClient> targets = new List<AIHeroClient>();
                targets.Add(Player.Instance);

                if (menuManager.ComboUseRForAlly)
                {
                    foreach (AIHeroClient ally in EntityManager.Heroes.Allies.Where(x => spellManager.R.IsInRange(x)))
                    {
                        targets.Add(ally);
                    }
                }

                AIHeroClient target = targets.FirstOrDefault(x => x.HealthPercent <= menuManager.ComboRHp && EntityManager.Heroes.Enemies.Count(e => e.Distance(x) <= spellManager.Q.Range) > 0);
                
                if (target != null)
                {
                    spellManager.R.Cast(target);
                }
            }
            
            base.OnTickCombo();
        }

        protected override void OnTickHarass()
        {
            if (menuManager.HarassUseQ &&
                Player.Instance.ManaPercent >= menuManager.HarassQMana &&
                spellManager.Q.IsReady())
            {
                AIHeroClient target = TargetSelector.GetTarget(spellManager.Q.Range, primaryDamageType);

                if (target != null)
                {
                    spellManager.Q.Cast(target);
                }
            }

            if (menuManager.HarassUseW &&
                Player.Instance.ManaPercent >= menuManager.HarassWMana &&
                Player.Instance.HealthPercent <= menuManager.HarassWHp &&
                spellManager.W.IsReady())
            {
                spellManager.W.Cast(Player.Instance);
            }

            if (menuManager.HarassUseE &&
                spellManager.E.IsReady() &&
                Player.Instance.ManaPercent >= menuManager.HarassEMana &&
                EntityManager.Heroes.Enemies.Count(x => x.Distance(Player.Instance) <= SpellManager.E_RANGE) > 0)
            {
                spellManager.E.Cast();
            }
            
            base.OnTickHarass();
        }

        protected override void OnTickLaneClear()
        {
            if (menuManager.LaneClearUseE &&
                Player.Instance.ManaPercent >= menuManager.LaneClearEMana &&
                spellManager.E.IsReady() &&
                EntityManager.MinionsAndMonsters.GetLaneMinions(EntityManager.UnitTeam.Enemy, Player.Instance.ServerPosition, SpellManager.E_RANGE, false).ToArray().Length > 0)
            {
                spellManager.E.Cast();
            }
            
            base.OnTickLaneClear();
        }

        protected override void OnTickJungleClear()
        {
            if (menuManager.JungleClearUseQ &&
                spellManager.Q.IsReady())
            {
                Obj_AI_Minion[] minions = EntityManager.MinionsAndMonsters.GetJungleMonsters(Player.Instance.ServerPosition, spellManager.Q.Range, false).OrderBy(x => DamageLibrary.GetSpellDamage(Player.Instance, x, SpellSlot.Q)).ToArray();

                if (minions.Length > 0)
                {
                    spellManager.Q.Cast(minions[0]);
                }
            }

            if (menuManager.JungleClearUseE &&
                spellManager.E.IsReady() &&
                EntityManager.MinionsAndMonsters.GetJungleMonsters(Player.Instance.ServerPosition, SpellManager.E_RANGE, false).ToArray().Length > 0)
            {
                spellManager.E.Cast();
            }
            
            base.OnTickJungleClear();
        }

        protected override void OnTickLastHit()
        {
            base.OnTickLastHit();
        }

        protected override void OnTickFlee()
        {
            if (menuManager.FleeUseQ &&
                spellManager.Q.IsReady())
            {
                AIHeroClient target = TargetSelector.GetTarget(spellManager.Q.Range, primaryDamageType);

                if (target != null)
                {
                    spellManager.Q.Cast(target);
                }
            }

            if (menuManager.FleeUseW &&
                spellManager.W.IsReady())
            {
                spellManager.W.Cast(Player.Instance);
            }

            if (menuManager.FleeUseR &&
                spellManager.R.IsReady() &&
                Player.Instance.HealthPercent <= menuManager.FleeRHp)
            {
                spellManager.R.Cast(Player.Instance);
            }
            
            base.OnTickFlee();
        }
    }
}