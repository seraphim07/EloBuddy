using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Enumerations;
using SharpDX;
using System;

namespace BundledBuddies.Bundles.Annie
{
    class SpellManager : SpellManagerBase
    {
        public const double W_ANGLE_OFFSET = -30.0d;
        public const float R_RADIUS_MULTIPLIER = 1.75f;

        public Spell.Targeted Q;
        public Spell.Skillshot W;
        public Spell.Active E;
        public Spell.Skillshot R;

        public SpellManager() : base()
        {
            SpellDataInst QData = Player.Instance.Spellbook.GetSpell(SpellSlot.Q);
            Console.WriteLine("Q Range: " + QData.SData.CastRange);
            Q = new Spell.Targeted(SpellSlot.Q, Convert.ToUInt32(QData.SData.CastRange));

            SpellDataInst WData = Player.Instance.Spellbook.GetSpell(SpellSlot.W);
            W = new Spell.Skillshot(SpellSlot.W, Convert.ToUInt32(WData.SData.CastRange), SkillShotType.Cone, Convert.ToInt32(WData.SData.CastTime) * 1000, Convert.ToInt32(WData.SData.MissileSpeed), Convert.ToInt32(WData.SData.CastConeAngle))
            {
                AllowedCollisionCount = int.MaxValue
            };

            E = new Spell.Active(SpellSlot.E);

            SpellDataInst RData = Player.Instance.Spellbook.GetSpell(SpellSlot.R);
            R = new Spell.Skillshot(SpellSlot.R, Convert.ToUInt32(RData.SData.CastRange), SkillShotType.Circular, 250, Convert.ToInt32(RData.SData.MissileSpeed), Convert.ToInt32(RData.SData.CastRadius))
            {
                AllowedCollisionCount = int.MaxValue
            };
        }

        public Geometry.Polygon.Sector WSector(Vector3 targetPosition)
        {
            return new Geometry.Polygon.Sector(Player.Instance.ServerPosition, targetPosition, (float)((W.ConeAngleDegrees + W_ANGLE_OFFSET) * Math.PI / 180.0d), W.Range);
        }

        public Geometry.Polygon.Circle RCircle(Vector3 targetPosition)
        {
            return new Geometry.Polygon.Circle(targetPosition, R.Radius * R_RADIUS_MULTIPLIER);
        }
    }
}
