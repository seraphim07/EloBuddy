using BundledBuddies.Bundles.Annie;
using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Enumerations;
using EloBuddy.SDK.Rendering;
using SharpDX;
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
            if (menuManager.InitiatorKey)
            {
                OnTickInitiator();
            }

            base.OnTickPermaActive();
        }

        private void OnTickInitiator()
        {
            Player.IssueOrder(GameObjectOrder.MoveTo, Game.CursorPos);

            bool condition = true;

            if (menuManager.InitiatorInitiateWhenStun)
            {
                condition = isStunUp;
            }

            if (condition)
            {
                Tuple<Vector3, int> bestR = GetRPos();

                if (bestR.Item2 >= menuManager.InitiatorCondition)
                {
                    spellManager.R.Cast(bestR.Item1);
                }
            }
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
                Tuple<Vector3, int> bestW = GetWPos(EntityManager.Heroes.Enemies.Where(x => spellManager.W.IsInRange(x)).ToArray());

                if (bestW.Item2 > 0)
                {
                    spellManager.W.Cast(bestW.Item1);
                }
            }

            if (menuManager.ComboUseR &&
                spellManager.R.IsReady())
            {
                Tuple<Vector3, int> bestR = GetRPos();

                if (bestR.Item2 > 0)
                {
                    spellManager.R.Cast(bestR.Item1);
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
                        Tuple<Vector3, int> bestW = GetWPos(EntityManager.MinionsAndMonsters.GetLaneMinions(EntityManager.UnitTeam.Enemy, Player.Instance.ServerPosition, spellManager.W.Range, false).ToArray());

                        if (bestW.Item2 >= menuManager.HarassWNumber)
                        {
                            spellManager.W.Cast(bestW.Item1);
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
            if (menuManager.JungleClearUseE &&
                spellManager.E.IsReady() &&
                EntityManager.MinionsAndMonsters.GetJungleMonsters(Player.Instance.ServerPosition, Player.Instance.GetAutoAttackRange(), false).ToArray().Length > 0)
            {
                spellManager.E.Cast();
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
            if (spellManager.Q.IsReady())
            {
                if (menuManager.JungleClearUseQWithoutLastHit)
                {
                    Obj_AI_Minion[] minions = EntityManager.MinionsAndMonsters
                        .GetJungleMonsters(Player.Instance.ServerPosition, spellManager.Q.Range, false)
                        .OrderBy(x => DamageLibrary.GetSpellDamage(Player.Instance, x, SpellSlot.Q)).ToArray();

                    if (minions.Length > 0)
                    {
                        spellManager.Q.Cast(minions[0]);
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

        private Tuple<Vector3, int> GetRPos()
        {
            Vector3 targetPosition = new Vector3();
            int maxNum = 0;

            foreach (AIHeroClient target in EntityManager.Heroes.Enemies)
            {
                for (float i = -spellManager.R.Radius; i <= spellManager.R.Radius; i += spellManager.R.Radius / 3.0f)
                {
                    for (float j = -spellManager.R.Radius; j <= spellManager.R.Radius; j += spellManager.R.Radius / 3.0f)
                    {
                        Vector3 testPosition = new Vector3(target.ServerPosition.X + i, target.ServerPosition.Y + j, target.ServerPosition.Z);
                        Geometry.Polygon.Circle rCircle = spellManager.RCircle(testPosition);
                        int count = EntityManager.Heroes.Enemies.Count(x => rCircle.IsInside(x));

                        if (count > maxNum && spellManager.R.IsInRange(testPosition))
                        {
                            targetPosition = testPosition;
                            maxNum = count;
                        }
                    }
                }
            }

            return new Tuple<Vector3, int>(targetPosition, maxNum);
        }
    }
}