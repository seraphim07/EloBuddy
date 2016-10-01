using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Enumerations;
using EloBuddy.SDK.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ClayAIO.ClayScripts.Annie
{
    class SpellManager : SpellManagerBase
    {
        public const string STUN_BUFF_NAME = "pyromania_particle";
        public const string R_BUFF_NAME = "infernalguardiantimer";

        public Spell.Targeted Q;
        public Spell.Skillshot W;
        public Spell.Active E;
        public Spell.Skillshot R;

        public SpellManager() : base()
        {
            SpellData QData = Player.Instance.Spellbook.GetSpell(SpellSlot.Q).SData;
            Q = new Spell.Targeted(
                SpellSlot.Q,
                Convert.ToUInt32(QData.CastRange));

            SpellData WData = Player.Instance.Spellbook.GetSpell(SpellSlot.W).SData;
            W = new Spell.Skillshot(
                SpellSlot.W,
                Convert.ToUInt32(WData.CastConeDistance),
                SkillShotType.Cone,
                Convert.ToInt32(WData.CastTime * 1000f),
                Convert.ToInt32(WData.MissileSpeed),
                Convert.ToInt32(WData.LineWidth))
            {
                ConeAngleDegrees = 45
            };
            
            E = new Spell.Active(SpellSlot.E);

            SpellData RData = Player.Instance.Spellbook.GetSpell(SpellSlot.R).SData;
            R = new Spell.Skillshot(
                SpellSlot.R,
                Convert.ToUInt32(RData.CastRange),
                SkillShotType.Circular,
                Convert.ToInt32(RData.CastTime * 1000f),
                Convert.ToInt32(RData.MissileSpeed),
                Convert.ToInt32(RData.CastRadius * 2f));
        }

        public void OnDraw(EventArgs e)
        {
            Circle.Draw(indianRed, Q.Range, Player.Instance);
            Circle.Draw(mediumPurple, W.Range, Player.Instance);
            Circle.Draw(darkRed, R.Range, Player.Instance);

            // new Geometry.Polygon.Sector(Player.Instance.ServerPosition, Game.CursorPos, (float)(W.ConeAngleDegrees * Math.PI / 180f), W.Range).Draw(System.Drawing.Color.Yellow);
            // new Geometry.Polygon.Circle(Game.CursorPos, R.Radius).Draw(System.Drawing.Color.Yellow);
        }

        public bool IsStunUp
        {
            get
            {
                return Player.Instance.HasBuff(STUN_BUFF_NAME);
            }
        }

        public bool IsTibber
        {
            get
            {
                return Player.Instance.HasBuff(R_BUFF_NAME);
            }
        }
        
        public void CastQToHero()
        {
            if (Q.IsReady())
            {
                AIHeroClient target = TargetSelector.GetTarget(Q.Range, DamageType.Magical);

                if (target != null)
                {
                    Q.Cast(target);
                }
            }
        }

        public void CastQToMinion(Obj_AI_Minion autoAttackedMinion)
        {
            if (Q.IsReady())
            {
                Obj_AI_Minion minion = EntityManager.MinionsAndMonsters.GetLaneMinions(
                    EntityManager.UnitTeam.Enemy,
                    Player.Instance.ServerPosition,
                    Q.Range).FirstOrDefault(m =>
                        m.IsEnemy &&
                        !m.IdEquals(autoAttackedMinion) &&
                        DamageLibrary.GetSpellDamage(Player.Instance, m, SpellSlot.Q) >= m.Health);
                
                if (minion != null)
                {
                    Q.Cast(minion);
                }
            }
        }

        public void CastQToJungle()
        {
            if (Q.IsReady())
            {
                List<Obj_AI_Minion> monsters = EntityManager.MinionsAndMonsters.GetJungleMonsters(
                    Player.Instance.ServerPosition,
                    Q.Range).ToList();

                if (monsters.Count > 0)
                {
                    Q.Cast(monsters[0]);
                }
            }
        }

        public void CastWToHero()
        {
            if (W.IsReady())
            {
                Spell.Skillshot.BestPosition bestPosition = GetBestConeCastPosition(W, EntityManager.Heroes.Enemies);

                if (bestPosition.HitNumber >= 2)
                {
                    W.Cast(bestPosition.CastPosition);
                }
                else
                {
                    AIHeroClient target = TargetSelector.GetTarget(W.Range, DamageType.Magical);

                    if (target != null)
                    {
                        CastSkillshotToTarget(W, target);
                    }
                }
            }
        }

        public void CastWToMinions(int minMinions)
        {
            if (W.IsReady())
            {
                List<Obj_AI_Minion> minions = EntityManager.MinionsAndMonsters.GetLaneMinions(
                    EntityManager.UnitTeam.Enemy,
                    Player.Instance.ServerPosition,
                    W.Range).Where(minion => W.IsInRange(minion)).ToList();

                if (minions.Count > 0)
                {
                    Spell.Skillshot.BestPosition bestPosition = GetBestConeCastPosition(W, minions);

                    if (bestPosition.HitNumber >= minMinions)
                    {
                        W.Cast(bestPosition.CastPosition);
                    }
                }
            }
        }

        public void CastWToJungle()
        {
            if (W.IsReady())
            {
                List<Obj_AI_Minion> monsters = EntityManager.MinionsAndMonsters.GetJungleMonsters(
                    Player.Instance.ServerPosition,
                    W.Range).ToList();

                if (monsters.Count > 0)
                {
                    Spell.Skillshot.BestPosition bestPosition = GetBestConeCastPosition(W, monsters);

                    if (bestPosition.HitNumber >= 2)
                    {
                        W.Cast(bestPosition.CastPosition);
                    }
                    else
                    {
                        CastSkillshotToTarget(W, monsters[0]);
                    }
                }
            }
        }

        public void CastRToHero()
        {
            if (R.IsReady() && !IsTibber)
            {
                Spell.Skillshot.BestPosition bestPosition = GetBestCircularCastPosition(R, EntityManager.Heroes.Enemies);

                if (bestPosition.HitNumber >= 5)
                {
                    R.Cast(bestPosition.CastPosition);
                }
                else
                {
                    AIHeroClient target = TargetSelector.GetTarget(R.Range, DamageType.Magical);

                    if (target != null)
                    {
                        CastSkillshotToTarget(R, target);
                    }
                }
            }
        }

        public void CastRToHero(int minHeroes)
        {
            if (R.IsReady() && !IsTibber)
            {
                Spell.Skillshot.BestPosition bestPosition = GetBestCircularCastPosition(R, EntityManager.Heroes.Enemies);

                if (bestPosition.HitNumber >= minHeroes)
                {
                    R.Cast(bestPosition.CastPosition);
                }
            }
        }
    }
}
