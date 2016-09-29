using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Enumerations;
using SharpDX;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ClayAIO
{
    class SpellManagerBase
    {
        public const string NAME_HEAL = "SummonerHeal";
        public const string NAME_BARRIER = "SummonerBarrier";
        public const string NAME_CLEANSE = "SummonerBoost";
        public const string NAME_EXHAUST = "SummonerExhaust";
        public const string NAME_GHOST = "SummonerHaste";
        public const string NAME_IGNITE = "summonerignite";
        public const string NAME_FLASH = "summonerflash";

        public Spell.Active Heal;
        public Spell.Active Barrier;
        public Spell.Active Cleanse;
        public Spell.Targeted Exhaust;
        public Spell.Targeted Ignite;
        public Spell.Skillshot Flash;
        public Spell.Active Ghost;

        protected ColorBGRA indianRed;
        protected ColorBGRA mediumPurple;
        protected ColorBGRA darkRed;
        protected ColorBGRA darkBlue;

        public SpellManagerBase()
        {
            indianRed = new ColorBGRA(Color.IndianRed.R, Color.IndianRed.G, Color.IndianRed.B, 127);
            mediumPurple = new ColorBGRA(Color.MediumPurple.R, Color.MediumPurple.G, Color.MediumPurple.B, 127);
            darkRed = new ColorBGRA(Color.DarkRed.R, Color.DarkRed.G, Color.DarkRed.B, 127);
            darkBlue = new ColorBGRA(Color.DarkBlue.R, Color.DarkBlue.G, Color.DarkBlue.B, 127);

            SpellSlot healSlot = Player.Instance.GetSpellSlotFromName(NAME_HEAL);
            Heal = !healSlot.Equals(SpellSlot.Unknown) ? new Spell.Active(healSlot) : null;

            SpellSlot barrierSlot = Player.Instance.GetSpellSlotFromName(NAME_BARRIER);
            Barrier = !barrierSlot.Equals(SpellSlot.Unknown) ? new Spell.Active(barrierSlot) : null;

            SpellSlot cleanseSlot = Player.Instance.GetSpellSlotFromName(NAME_CLEANSE);
            Cleanse = !cleanseSlot.Equals(SpellSlot.Unknown) ? new Spell.Active(cleanseSlot) : null;

            SpellSlot exhaustSlot = Player.Instance.GetSpellSlotFromName(NAME_EXHAUST);
            if (!exhaustSlot.Equals(SpellSlot.Unknown))
            {
                SpellDataInst exhaustData = Player.Instance.Spellbook.GetSpell(exhaustSlot);
                Exhaust = new Spell.Targeted(exhaustSlot, Convert.ToUInt32(exhaustData.SData.CastRange));
            }
            else
            {
                Exhaust = null;
            }

            SpellSlot igniteSlot = Player.Instance.GetSpellSlotFromName(NAME_IGNITE);
            if (!igniteSlot.Equals(SpellSlot.Unknown))
            {
                SpellDataInst igniteData = Player.Instance.Spellbook.GetSpell(igniteSlot);
                Ignite = new Spell.Targeted(igniteSlot, Convert.ToUInt32(igniteData.SData.CastRange));
            }
            else
            {
                Ignite = null;
            }

            SpellSlot flashSlot = Player.Instance.GetSpellSlotFromName(NAME_FLASH);
            if (!flashSlot.Equals(SpellSlot.Unknown))
            {
                SpellDataInst flashData = Player.Instance.Spellbook.GetSpell(flashSlot);
                Flash = new Spell.Skillshot(flashSlot, Convert.ToUInt32(flashData.SData.CastRange), SkillShotType.Linear);
            }

            SpellSlot ghostSlot = Player.Instance.GetSpellSlotFromName(NAME_GHOST);
            if (!ghostSlot.Equals(SpellSlot.Unknown))
            {
                Ghost = new Spell.Active(ghostSlot);
            }
        }

        public bool CastSkillshotToTarget(Spell.Skillshot skillshot, Obj_AI_Base target)
        {
            if (skillshot.IsReady() && target != null && target.IsValidTarget(skillshot.Range))
            {
                PredictionResult predictionResult = skillshot.GetPrediction(target);

                if (predictionResult.HitChance >= HitChance.High)
                {
                    return skillshot.Cast(target);
                }
            }

            return false;
        }

        protected Tuple<Obj_AI_Base, int> GetBestLinearCastTarget(Spell.Skillshot skillshot, IEnumerable<Obj_AI_Base> entities)
        {
            Tuple<Obj_AI_Base, int> target = new Tuple<Obj_AI_Base, int>(null, 0);

            foreach (Obj_AI_Base entity in entities.Where(e => skillshot.IsInRange(e)))
            {
                Geometry.Polygon.Rectangle rectangle = new Geometry.Polygon.Rectangle(
                    Player.Instance.ServerPosition,
                    entity.ServerPosition,
                    skillshot.Width);

                int hitNumber = entities.Count(e => rectangle.IsInside(e));

                if (hitNumber > target.Item2)
                {
                    target = new Tuple<Obj_AI_Base, int>(entity, hitNumber);
                }
            }

            return target;
        }

        protected Spell.Skillshot.BestPosition GetBestConeCastPosition(Spell.Skillshot skillshot, IEnumerable<Obj_AI_Base> entities)
        {
            Spell.Skillshot.BestPosition bestPosition = new Spell.Skillshot.BestPosition()
            {
                CastPosition = new Vector3(0, 0, 0),
                HitNumber = 0
            };

            for (int x = -5; x <= 5; x++)
            {
                for (int y = -5; y <= 5; y++)
                {
                    if (x == 0 && y == 0) continue;

                    Vector3 castPosition = new Vector3(
                        Player.Instance.ServerPosition.X + x,
                        Player.Instance.ServerPosition.Y + y,
                        Player.Instance.ServerPosition.Z);

                    Geometry.Polygon.Sector sector = new Geometry.Polygon.Sector(
                        Player.Instance.ServerPosition,
                        castPosition,
                        (float)(skillshot.ConeAngleDegrees * Math.PI / 180f),
                        skillshot.Range);

                    int hitNumber = entities.Count(entity => sector.IsInside(entity));

                    if (hitNumber > bestPosition.HitNumber)
                    {
                        bestPosition.CastPosition = castPosition;
                        bestPosition.HitNumber = hitNumber;
                    }
                }
            }

            return bestPosition;
        }

        protected Spell.Skillshot.BestPosition GetBestCircularCastPosition(Spell.Skillshot skillshot, IEnumerable<Obj_AI_Base> entities)
        {
            Spell.Skillshot.BestPosition bestPosition = new Spell.Skillshot.BestPosition()
            {
                CastPosition = new Vector3(0f, 0f, 0f),
                HitNumber = 0
            };

            foreach (Obj_AI_Base entity in entities)
            {
                for (float x = entity.ServerPosition.X - skillshot.Radius;
                    x <= entity.ServerPosition.X + skillshot.Radius;
                    x += skillshot.Radius / 5f)
                {
                    for (float y = entity.ServerPosition.Y - skillshot.Radius;
                        y <= entity.ServerPosition.Y + skillshot.Radius;
                        y += skillshot.Radius / 5f)
                    {
                        Vector3 castPosition = new Vector3(x, y, entity.ServerPosition.Z);

                        if (!skillshot.IsInRange(castPosition)) continue;

                        Geometry.Polygon.Circle circle = new Geometry.Polygon.Circle(
                            castPosition,
                            skillshot.Radius);

                        int hitNumber = entities.Count(e => circle.IsInside(e));

                        if (hitNumber > bestPosition.HitNumber)
                        {
                            bestPosition.CastPosition = castPosition;
                            bestPosition.HitNumber = hitNumber;
                        }
                    }
                }
            }

            return bestPosition;
        }
    }
}
