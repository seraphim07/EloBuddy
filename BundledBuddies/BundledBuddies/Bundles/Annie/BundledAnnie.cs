using BundledBuddies.Bundles.Annie;
using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Enumerations;
using EloBuddy.SDK.Rendering;
using System;
using System.Linq;

namespace BundledBuddies.Bundles
{
    partial class BundledAnnie : BundledBase
    {
        private MenuManager menuManager;
        private SpellManager spellManager;

        private Obj_AI_Minion autoAttackedMinion;

        private bool isStunUp
        {
            get
            {
                return Player.Instance.HasBuff("pyromania_particle");
            }
        }

        private bool isTibber
        {
            get
            {
                return Player.Instance.HasBuff("infernalguardiantimer");
            }
        }

        public BundledAnnie() : base()
        {
            menuManagerBase = new MenuManager();
            spellManagerBase = new SpellManager();

            menuManager = menuManagerBase as MenuManager;
            spellManager = spellManagerBase as SpellManager;

            primaryDamageType = DamageType.Magical;

            Initialize();

            Drawing.OnDraw += OnDraw;
            Orbwalker.OnPreAttack += OnPreAttack;
            Orbwalker.OnAttack += OnAttack;

            Chat.Print("BundledAnnie loaded!");
            
        }
        
        private void OnDraw(EventArgs e)
        {
            Circle.Draw(indianRed, spellManager.Q.Range, Player.Instance);
            Circle.Draw(mediumPurple, spellManager.W.Range, Player.Instance);
            Circle.Draw(darkRed, spellManager.R.Range, Player.Instance);
        }

        private void OnPreAttack(AttackableUnit target, Orbwalker.PreAttackArgs args)
        {
            if (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.Combo) &&
                (spellManager.Q.IsReady() ||
                 spellManager.W.IsReady() ||
                 (spellManager.R.IsReady() && !isTibber)))
            {
                args.Process = false;
            }
        }

        private void OnAttack(AttackableUnit target, EventArgs args)
        {
            if (target.Type.Equals(GameObjectType.obj_AI_Minion))
            {
                autoAttackedMinion = target as Obj_AI_Minion;

                Core.DelayAction(() => autoAttackedMinion = null, 1000);
            }
        }

        protected override void OnTickPermaActive()
        {
            base.OnTickPermaActive();
        }
        
        protected override void OnTickCombo()
        {
            if (menuManager.ComboUseE &&
                spellManager.E.IsReady())
            {
                if (EntityManager.Heroes.Enemies.Count(x => Player.Instance.IsInAutoAttackRange(x)) > 0)
                {
                    spellManager.E.Cast();
                }
            }

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
                spellManager.W.IsReady())
            {
                AIHeroClient target = TargetSelector.GetTarget(spellManager.W.Range, primaryDamageType);
                
                if (target != null)
                {
                    spellManager.W.Cast(target);
                }
            }

            if (menuManager.ComboUseR &&
                spellManager.R.IsReady())
            {
                AIHeroClient target = TargetSelector.GetTarget(spellManager.R.Range, primaryDamageType);
                
                if (target != null)
                {
                    spellManager.R.Cast(target);
                }
            }

            base.OnTickCombo();
        }

        protected override void OnTickHarass()
        {
            if (isStunUp)
            {
                if (menuManager.HarassUseQ &&
                    spellManager.Q.IsReady())
                {
                    AIHeroClient qTarget = TargetSelector.GetTarget(spellManager.Q.Range, primaryDamageType);

                    if (qTarget != null)
                    {
                        spellManager.Q.Cast(qTarget);
                    }
                }
                else if (menuManager.HarassUseW &&
                    spellManager.W.IsReady())
                {
                    AIHeroClient wTarget = TargetSelector.GetTarget(spellManager.W.Range, primaryDamageType);

                    if (wTarget != null)
                    {
                        PredictionResult predictionResult = spellManager.W.GetPrediction(wTarget);

                        if (predictionResult.HitChance >= HitChance.High)
                        {
                            spellManager.W.Cast(predictionResult.CastPosition);
                        }
                    }
                }
            }
            else
            {
                if (spellManager.Q.IsReady())
                {
                    AIHeroClient qTarget = null;

                    if (menuManager.HarassUseQ)
                    {
                        qTarget = EntityManager.Heroes.Enemies.FirstOrDefault(x => spellManager.Q.IsInRange(x) && x.HasBuffOfType(BuffType.Stun));
                    }

                    if (qTarget != null)
                    {
                        spellManager.Q.Cast(qTarget);
                    }
                    else
                    {
                        LastHitQ(EntityManager.MinionsAndMonsters.GetLaneMinions(EntityManager.UnitTeam.Enemy, Player.Instance.ServerPosition, spellManager.Q.Range, false).ToArray());
                    }
                }
                else if (spellManager.W.IsReady())
                {
                    AIHeroClient wTarget = null;

                    if (menuManager.HarassUseW)
                    {
                        wTarget = EntityManager.Heroes.Enemies.FirstOrDefault(x => spellManager.W.IsInRange(x) && x.HasBuffOfType(BuffType.Stun));

                        if (wTarget != null)
                        {
                            PredictionResult predictionResult = spellManager.W.GetPrediction(wTarget);

                            if (predictionResult.HitChance < HitChance.High)
                            {
                                wTarget = null;
                            }
                        }
                    }

                    if (wTarget != null)
                    {
                        spellManager.W.Cast(wTarget);
                    }
                    else if (menuManager.HarassUseWStackStun &&
                        Player.Instance.ManaPercent >= menuManager.HarassWMana)
                    {
                        AIHeroClient target = TargetSelector.GetTarget(spellManager.W.Range, primaryDamageType);
                        
                        if (target != null)
                        {
                            spellManager.W.Cast(target);
                        }
                    }
                }
                else if (spellManager.E.IsReady() &&
                    menuManager.HarassUseEStackStun)
                {
                    spellManager.E.Cast();
                }
            }

            base.OnTickHarass();
        }

        protected override void OnTickLaneClear()
        {
            LastHitQ(EntityManager.MinionsAndMonsters.GetLaneMinions(EntityManager.UnitTeam.Enemy, Player.Instance.ServerPosition, spellManager.Q.Range, false).ToArray());

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
            if (menuManager.JungleClearUseE &&
                spellManager.E.IsReady() &&
                EntityManager.MinionsAndMonsters.GetJungleMonsters(Player.Instance.ServerPosition, Player.Instance.GetAutoAttackRange(), false).ToArray().Length > 0)
            {
                spellManager.E.Cast();
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

            if (spellManager.Q.IsReady())
            {
                if (menuManager.JungleClearUseQWithoutLastHit)
                {
                    Obj_AI_Minion target = EntityManager.MinionsAndMonsters
                        .GetJungleMonsters(Player.Instance.ServerPosition, spellManager.Q.Range, false)
                        .OrderBy(x => x.Distance(Player.Instance.ServerPosition)).ElementAtOrDefault(0);

                    if (target != null)
                    {
                        spellManager.Q.Cast(target);
                    }
                }
                else
                {
                    LastHitQ(EntityManager.MinionsAndMonsters.GetJungleMonsters(Player.Instance.ServerPosition, spellManager.Q.Range, false).ToArray());
                }
            }

            base.OnTickJungleClear();
        }

        protected override void OnTickLastHit()
        {
            if (menuManager.LastHitUseQ)
            {
                LastHitQ(EntityManager.MinionsAndMonsters.GetLaneMinions(EntityManager.UnitTeam.Enemy, Player.Instance.ServerPosition, spellManager.Q.Range, false).ToArray());
            }

            base.OnTickLastHit();
        }

        protected override void OnTickFlee()
        {
            if (spellManager.E.IsReady())
            {
                spellManager.E.Cast();
            }

            if (isStunUp &&
                spellManager.Q.IsReady())
            {
                AIHeroClient target = EntityManager.Heroes.Enemies.OrderBy(x => x.Distance(Player.Instance.ServerPosition)).ElementAtOrDefault(0);

                if (target != null &&
                    spellManager.Q.IsInRange(target))
                {
                    spellManager.Q.Cast(target);
                }
            }

            base.OnTickFlee();
        }

        private void LastHitQ(Obj_AI_Minion[] targets)
        {
            if (spellManager.Q.IsReady())
            {
                Obj_AI_Minion target = targets.FirstOrDefault(x => 
                    !x.IdEquals(autoAttackedMinion) &&
                    DamageLibrary.GetSpellDamage(Player.Instance, x, SpellSlot.Q) >= x.Health);

                if (target != null)
                {
                    spellManager.Q.Cast(target);
                }
            }
        }
    }
}