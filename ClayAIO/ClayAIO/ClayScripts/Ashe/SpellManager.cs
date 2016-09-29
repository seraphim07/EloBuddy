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
                Convert.ToInt32(WData.CastTime * 1000f),
                Convert.ToInt32(WData.MissileSpeed),
                Convert.ToInt32(WData.LineWidth))
            {
                ConeAngleDegrees = 40
            };
            
            SpellData EData = Player.Instance.Spellbook.GetSpell(SpellSlot.E).SData;
            E = new Spell.Skillshot(
                SpellSlot.E,
                Convert.ToUInt32(EData.CastRange),
                SkillShotType.Linear,
                Convert.ToInt32(EData.CastTime * 1000f),
                Convert.ToInt32(EData.MissileSpeed),
                Convert.ToInt32(EData.LineWidth));

            SpellData RData = Player.Instance.Spellbook.GetSpell(SpellSlot.R).SData;
            R = new Spell.Skillshot(
                SpellSlot.R,
                Convert.ToUInt32(RData.CastRange),
                SkillShotType.Linear,
                Convert.ToInt32(RData.CastTime * 1000f),
                Convert.ToInt32(RData.MissileSpeed),
                Convert.ToInt32(RData.LineWidth));
        }

        public void OnDraw(EventArgs e)
        {
            Circle.Draw(indianRed, W.Range, Player.Instance);

            // new Geometry.Polygon.Sector(Player.Instance.ServerPosition, Game.CursorPos, (float)(W.ConeAngleDegrees * Math.PI / 180), W.Range).Draw(System.Drawing.Color.Yellow);
            // new Geometry.Polygon.Rectangle(Player.Instance.ServerPosition, Game.CursorPos, E.Width).Draw(System.Drawing.Color.Yellow);
            // new Geometry.Polygon.Rectangle(Player.Instance.ServerPosition, Game.CursorPos, R.Width).Draw(System.Drawing.Color.Yellow);
        }
        
        public void CastWToHero()
        {
            if (W.IsReady())
            {
                Spell.Skillshot.BestPosition bestPosition = GetBestConeCastPosition(W, EntityManager.Heroes.Enemies.Where(enemy => W.IsInRange(enemy)));

                if (bestPosition.HitNumber >= 2)
                {
                    W.Cast(bestPosition.CastPosition);
                }
                else
                {
                    AIHeroClient target = TargetSelector.GetTarget(W.Range, DamageType.Physical);

                    if (target != null)
                    {
                        CastSkillshotToTarget(W, target);
                    }
                }
            }
        }

        public void CastWToJungle()
        {
            if (W.IsReady())
            {
                List<Obj_AI_Minion> jungleMonsters = EntityManager.MinionsAndMonsters.GetJungleMonsters(Player.Instance.ServerPosition, W.Range).ToList();

                if (jungleMonsters.Count > 0)
                {
                    Spell.Skillshot.BestPosition bestPosition = GetBestConeCastPosition(W, jungleMonsters);

                    if (bestPosition.HitNumber >= 2)
                    {
                        W.Cast(bestPosition.CastPosition);
                    }
                    else
                    {
                        CastSkillshotToTarget(W, jungleMonsters[0]);
                    }
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
                    CastSkillshotToTarget(R, target);
                }
            }
        }
        
        public void CastRToTarget()
        {
            if (R.IsReady() && TargetSelector.SelectedTarget != null)
            {
                CastSkillshotToTarget(R, TargetSelector.SelectedTarget);
            }
        }
    }
}
