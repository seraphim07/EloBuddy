using BundledBuddies.Bundles.Ashe;
using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BundledBuddies.Bundles
{
    partial class BundledAshe : BundledBase
    {
        private MenuManager menuManager;
        private SpellManager spellManager;

        private List<AIHeroClient> comboTargets;

        public BundledAshe() : base()
        {
            menuManagerBase = new MenuManager();
            spellManagerBase = new SpellManager();

            menuManager = menuManagerBase as MenuManager;
            spellManager = spellManagerBase as SpellManager;

            comboTargets = new List<AIHeroClient>();

            primaryDamageType = DamageType.Physical;

            Initialize();

            Drawing.OnDraw += OnDraw;
            Orbwalker.OnPostAttack += OnPostAttack;

            Chat.Print("BundledAshe loaded!");

        }

        private void OnDraw(EventArgs e)
        {
            Circle.Draw(indianRed, spellManager.W.Range, Player.Instance);
        }

        private void OnPostAttack(AttackableUnit target, EventArgs args)
        {
            if (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.Combo) &&
                menuManager.ComboUseW &&
                spellManager.W.IsReady())
            {
                AIHeroClient selectedTarget = TargetSelector.GetTarget(spellManager.W.Range, primaryDamageType);

                if (target != null)
                {
                    spellManager.W.Cast(selectedTarget);
                    Orbwalker.ResetAutoAttack();
                }
            }
        }
        
        protected override void OnTickPermaActive()
        {
            if (menuManager.FireUltKey)
            {
                OnTickFireUlt();
            }

            if (!Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.Combo))
            {
                comboTargets.Clear();
            }

            base.OnTickPermaActive();
        }

        private void OnTickFireUlt()
        {
            Player.IssueOrder(GameObjectOrder.MoveTo, Game.CursorPos);

            if (spellManager.R.IsReady())
            {
                AIHeroClient target = TargetSelector.GetTarget(spellManager.R.Range, primaryDamageType);
                
                if (target != null)
                {
                    spellManager.R.Cast(target);
                }
            }
        }

        protected override void OnTickCombo()
        {
            if (menuManager.ComboUseE)
            {
                if (spellManager.E.IsReady())
                {
                    foreach (AIHeroClient target in comboTargets)
                    {
                        if (!target.IsVisible && spellManager.E.IsInRange(target))
                        {
                            spellManager.E.Cast(target.ServerPosition);
                        }
                    }
                }

                comboTargets.Clear();

                foreach (AIHeroClient target in EntityManager.Heroes.Enemies.Where(x => spellManager.E.IsInRange(x)))
                {
                    comboTargets.Add(target);
                }
            }
            
            if (menuManager.ComboUseQ &&
                spellManager.Q.IsReady() &&
                EntityManager.Heroes.Enemies.Count(x => Player.Instance.IsInAutoAttackRange(x)) > 0)
            {
                spellManager.Q.Cast();
            }

            if (menuManager.ComboUseR &&
                spellManager.R.IsReady())
            {
                AIHeroClient target = TargetSelector.GetTarget(spellManager.W.Range, primaryDamageType);

                if (target != null)
                {
                    spellManager.R.Cast(target);
                }
            }

            base.OnTickCombo();
        }

        protected override void OnTickHarass()
        {
            if (menuManager.HarassUseW &&
                Player.Instance.ManaPercent >= menuManager.HarassWMana &&
                spellManager.W.IsReady())
            {
                AIHeroClient target = TargetSelector.GetTarget(spellManager.W.Range, primaryDamageType);

                if (target != null)
                {
                    spellManager.W.Cast(target);
                }
            }
            
            base.OnTickHarass();
        }

        protected override void OnTickLaneClear()
        {
            if (menuManager.LaneClearUseQ &&
                Player.Instance.ManaPercent >= menuManager.LaneClearQMana &&
                spellManager.Q.IsReady() &&
                EntityManager.MinionsAndMonsters.GetLaneMinions(EntityManager.UnitTeam.Enemy, Player.Instance.ServerPosition, Player.Instance.GetAutoAttackRange(), false).ToArray().Length > 0)
            {
                spellManager.Q.Cast();
            }

            if (menuManager.LaneClearUseW &&
                Player.Instance.ManaPercent >= menuManager.LaneClearWMana &&
                spellManager.W.IsReady())
            {
                Obj_AI_Minion target = EntityManager.MinionsAndMonsters.GetLaneMinions(EntityManager.UnitTeam.Enemy, Player.Instance.ServerPosition, spellManager.W.Range, false).OrderBy(x => x.Distance(Player.Instance.ServerPosition)).ElementAtOrDefault(0);

                if (target != null)
                {
                    spellManager.W.Cast(target);
                }
            }

            base.OnTickLaneClear();
        }

        protected override void OnTickJungleClear()
        {
            if (menuManager.JungleClearUseQ &&
                spellManager.Q.IsReady() &&
                EntityManager.MinionsAndMonsters.GetJungleMonsters(Player.Instance.ServerPosition, Player.Instance.GetAutoAttackRange(), false).ToArray().Length > 0)
            {
                spellManager.Q.Cast();
            }
            
            if (menuManager.JungleClearUseW &&
                spellManager.W.IsReady())
            {
                Obj_AI_Minion target = EntityManager.MinionsAndMonsters.GetJungleMonsters(Player.Instance.ServerPosition, spellManager.W.Range, false).OrderBy(x => x.Distance(Player.Instance.ServerPosition)).ElementAtOrDefault(0);
                
                if (target != null)
                {
                    spellManager.W.Cast(target);
                }
            }
            
            base.OnTickJungleClear();
        }

        protected override void OnTickLastHit()
        {
            base.OnTickLastHit();
        }

        protected override void OnTickFlee()
        {
            if (menuManager.FleeUseW &&
                spellManager.W.IsReady())
            {
                AIHeroClient target = TargetSelector.GetTarget(spellManager.W.Range, primaryDamageType);
                
                if (target != null)
                {
                    spellManager.W.Cast(target);
                }
            }
            
            base.OnTickFlee();
        }
    }
}