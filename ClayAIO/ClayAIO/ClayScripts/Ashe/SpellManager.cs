using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Enumerations;
using EloBuddy.SDK.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ClayAIO.ClayScripts.Ashe
{
    class SpellManager : SpellManagerBase
    {
        public Spell.Active Q;
        public Spell.Skillshot W;
        public Spell.Skillshot E;
        public Spell.Skillshot R;
        
        public SpellManager() : base()
        {
            Q = new Spell.Active(SpellSlot.Q);

            SpellData WData = Player.Instance.Spellbook.GetSpell(SpellSlot.W).SData;
            W = new Spell.Skillshot(
                SpellSlot.W,
                Convert.ToUInt32(WData.CastConeDistance),
                SkillShotType.Cone,
                Convert.ToInt32(WData.CastTime) * 1000,
                Convert.ToInt32(WData.MissileSpeed))
            {
                ConeAngleDegrees = Convert.ToInt32(WData.CastConeAngle)
            };
            
            SpellData EData = Player.Instance.Spellbook.GetSpell(SpellSlot.E).SData;
            E = new Spell.Skillshot(
                SpellSlot.E,
                Convert.ToUInt32(EData.CastRange),
                SkillShotType.Linear,
                Convert.ToInt32(EData.CastTime) * 1000,
                Convert.ToInt32(EData.MissileSpeed),
                Convert.ToInt32(EData.LineWidth));

            SpellData RData = Player.Instance.Spellbook.GetSpell(SpellSlot.R).SData;
            R = new Spell.Skillshot(
                SpellSlot.R,
                Convert.ToUInt32(RData.CastRange),
                SkillShotType.Linear,
                Convert.ToInt32(RData.CastTime) * 1000,
                Convert.ToInt32(RData.MissileSpeed),
                Convert.ToInt32(RData.LineWidth));
        }

        public void OnDraw(EventArgs e)
        {
            Circle.Draw(indianRed, W.Range, Player.Instance);
        }

        public void CastQ(IEnumerable<Obj_AI_Base> targets)
        {
            if (Player.Instance.Spellbook.CanUseSpell(SpellSlot.Q) == SpellState.Ready &&
                targets.Count(x => Player.Instance.IsInAutoAttackRange(x)) > 0)
            {
                Player.Instance.Spellbook.CastSpell(SpellSlot.Q);
            }
        }
        
        public void CastWToHero()
        {
            if (W.IsReady())
            {
                AIHeroClient target = TargetSelector.GetTarget(W.Range, DamageType.Physical);

                if (target != null)
                {
                    CastW(target);
                }

                /*Spell.Skillshot.BestPosition pos = W.GetBestConeCastPosition(EntityManager.Heroes.Enemies);
                
                if (pos.HitNumber > 0)
                {
                    W.Cast(pos.CastPosition);
                }*/
            }
        }

        public void CastWToJungle()
        {
            if (W.IsReady())
            {
                List<Obj_AI_Minion> targets = EntityManager.MinionsAndMonsters.GetJungleMonsters(Player.Instance.ServerPosition, W.Range).ToList();

                if (targets.Count > 0)
                {
                    W.Cast(targets[0]);
                }

                /*Spell.Skillshot.BestPosition pos = W.GetBestConeCastPosition(EntityManager.MinionsAndMonsters.GetJungleMonsters(Player.Instance.ServerPosition, W.Range));

                if (pos.HitNumber > 0)
                {
                    W.Cast(pos.CastPosition);
                }*/
            }
        }

        public void CastW(AIHeroClient target)
        {
            if (W.IsReady() && target != null && target.IsValidTarget(W.Range))
            {
                PredictionResult pred = W.GetPrediction(target);
                
                if (pred.HitChance >= HitChance.High)
                {
                    W.Cast(target);
                }
            }
        }
        
        public void CastRToHero()
        {
            if (R.IsReady())
            {
                AIHeroClient target = TargetSelector.GetTarget(W.Range, DamageType.Physical);

                if (target != null)
                {
                    CastR(target);
                }
            }
        }
        
        public void CastRToTarget()
        {
            if (R.IsReady() && TargetSelector.SelectedTarget != null)
            {
                CastR(TargetSelector.SelectedTarget);
            }
        }

        public void CastR(AIHeroClient target)
        {
            if (R.IsReady() && target != null && target.IsValidTarget(R.Range))
            {
                PredictionResult pred = R.GetPrediction(target);

                if (pred.HitChance >= HitChance.High)
                {
                    R.Cast(target);
                }
            }
        }
    }
}
