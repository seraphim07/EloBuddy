using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Enumerations;
using EloBuddy.SDK.Rendering;
using EloBuddy.SDK.Spells;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ClayAIO.ClayScripts
{
    class SpellManager : SpellManagerBase
    {
        private float minHitChancePercent = 60.0f;

        private SpellInfo WInfo;
        private SpellInfo RInfo;
        
        public SpellManager() : base()
        {
            List<SpellInfo> spellInfoList = SpellDatabase.GetSpellInfoList(Player.Instance);

            WInfo = spellInfoList.First(x => x.Slot == SpellSlot.W);
            RInfo = spellInfoList.First(x => x.Slot == SpellSlot.R);
        }

        public void OnDraw(EventArgs e)
        {
            Circle.Draw(indianRed, WInfo.Range + WInfo.Radius * 2, Player.Instance);
        }

        public void CastQ(IEnumerable<Obj_AI_Base> targets)
        {
            if (targets.Count(x => Player.Instance.IsInAutoAttackRange(x)) > 0)
            {
                Player.Instance.Spellbook.CastSpell(SpellSlot.Q);
            }
        }
        
        public bool CastWToHero()
        {
            AIHeroClient target = TargetSelector.GetTarget(WInfo.Range + WInfo.Radius * 2, DamageType.Physical);

            if (target != null)
            {
                Player.Instance.Spellbook.CastSpell(SpellSlot.W, target.ServerPosition);

                return true;
            }

            return false;
        }

        public bool CastWToJungle()
        {
            Obj_AI_Minion target = EntityManager.MinionsAndMonsters.GetJungleMonsters(Player.Instance.ServerPosition, WInfo.Range + WInfo.Radius * 2).ElementAtOrDefault(0);

            if (target != null)
            {
                Player.Instance.Spellbook.CastSpell(SpellSlot.W, target.ServerPosition);

                return true;
            }

            return false;
        }
        
        public void CastRToHero()
        {
            CastMissileLineToHero(SpellSlot.R, RInfo);
        }
        
        public void CastRToTarget()
        {
            if (TargetSelector.SelectedTarget != null)
            {
                CastMissileLineToHero(SpellSlot.R, RInfo, TargetSelector.SelectedTarget);
            }
        }

        private void CastMissileLineToHero(SpellSlot spellSlot, SpellInfo spellInfo)
        {
            AIHeroClient target = TargetSelector.GetTarget(WInfo.Range + WInfo.Radius * 2, DamageType.Physical);

            if (target != null)
            {
                CastMissileLineToHero(spellSlot, spellInfo, target);
            }
        }

        private void CastMissileLineToHero(SpellSlot spellSlot, SpellInfo spellInfo, AIHeroClient target)
        {
            Prediction.Manager.PredictionInput predictionInput = new Prediction.Manager.PredictionInput()
            {
                Target = target,
                Range = spellInfo.Range,
                Delay = spellInfo.Delay + spellInfo.MissileFixedTravelTime,
                Speed = spellInfo.MissileSpeed,
                Radius = RInfo.Radius,
                From = Player.Instance.ServerPosition,
                Type = SkillShotType.Linear
            };

            foreach (CollisionType collision in spellInfo.Collisions)
            {
                predictionInput.CollisionTypes.Add(collision);
            }

            Prediction.Manager.PredictionOutput predictionOutput = Prediction.Position.GetPrediction(predictionInput);
            
            if (predictionOutput.HitChancePercent >= minHitChancePercent)
            {
                Player.Instance.Spellbook.CastSpell(spellSlot, predictionOutput.CastPosition);
            }
        }
    }
}
