using BundledBuddies.Bundles.Ashe;
using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Enumerations;
using EloBuddy.SDK.Rendering;
using SharpDX;
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
                Tuple<Vector3, int> bestW = GetWPos(EntityManager.Heroes.Enemies.Where(x => spellManager.W.IsInRange(x)).ToArray());

                if (bestW.Item2 > 0)
                {
                    spellManager.W.Cast(bestW.Item1);
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
                AIHeroClient[] targets = EntityManager.Heroes.Enemies.Where(x => spellManager.R.IsInRange(x)).OrderBy(x => x.Distance(Player.Instance)).ToArray();

                for (int i = 0; i < targets.Length; i++)
                {
                    PredictionResult result = spellManager.R.GetPrediction(targets[i]);

                    if (result.HitChance >= HitChance.High)
                    {
                        spellManager.R.Cast(result.CastPosition);

                        return;
                    }
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
                AIHeroClient[] targets = EntityManager.Heroes.Enemies.Where(x => spellManager.W.IsInRange(x)).OrderBy(x => x.Distance(Player.Instance)).ToArray();

                for (int i = 0; i < targets.Length; i++)
                {
                    PredictionResult result = spellManager.R.GetPrediction(targets[i]);

                    if (result.HitChance >= HitChance.High)
                    {
                        spellManager.R.Cast(result.CastPosition);

                        break;
                    }
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
                Tuple<Vector3, int> bestW = GetWPos(EntityManager.Heroes.Enemies.Where(x => spellManager.W.IsInRange(x)).ToArray());

                if (bestW.Item2 > 0)
                {
                    spellManager.W.Cast(bestW.Item1);
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
                Tuple<Vector3, int> bestW = GetWPos(EntityManager.MinionsAndMonsters.GetLaneMinions(EntityManager.UnitTeam.Enemy, Player.Instance.ServerPosition, spellManager.W.Range, false).ToArray());

                if (bestW.Item2 >= menuManager.LaneClearWNumber)
                {
                    spellManager.W.Cast(bestW.Item1);
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
                Tuple<Vector3, int> bestW = GetWPos(EntityManager.MinionsAndMonsters.GetJungleMonsters(Player.Instance.ServerPosition, spellManager.W.Range, false).ToArray());

                if (bestW.Item2 > 0)
                {
                    spellManager.W.Cast(bestW.Item1);
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
                Tuple<Vector3, int> bestW = GetWPos(EntityManager.Heroes.Enemies.Where(x => spellManager.W.IsInRange(x)).ToArray());

                if (bestW.Item2 > 0)
                {
                    spellManager.W.Cast(bestW.Item1);
                }
            }
            
            base.OnTickFlee();
        }
        
        private Tuple<Vector3, int> GetWPos(Obj_AI_Base[] targets)
        {
            Vector3 targetPosition = new Vector3();
            int maxNum = 0;

            for (int i = -300; i <= 300; i += 100)
            {
                for (int j = -300; j <= 300; j += 100)
                {
                    if (i.Equals(0) && j.Equals(0)) continue;

                    Vector3 testPosition = new Vector3(Player.Instance.ServerPosition.X + i, Player.Instance.ServerPosition.Y + j, Player.Instance.ServerPosition.Z);
                    Geometry.Polygon.Sector wSector = spellManager.WSector(testPosition);
                    int count = targets.Count(x => wSector.IsInside(x));

                    if (count > maxNum)
                    {
                        targetPosition = testPosition;
                        maxNum = count;
                    }
                }
            }

            return new Tuple<Vector3, int>(targetPosition, maxNum);
        }
    }
}