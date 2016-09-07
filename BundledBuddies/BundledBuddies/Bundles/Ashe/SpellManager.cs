using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Enumerations;
using System;

namespace BundledBuddies.Bundles.Ashe
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

            SpellDataInst WData = Player.Instance.Spellbook.GetSpell(SpellSlot.W);
            W = new Spell.Skillshot(SpellSlot.W, Convert.ToUInt32(WData.SData.CastConeDistance), SkillShotType.Cone, Convert.ToInt32(WData.SData.CastTime) * 1000, Convert.ToInt32(WData.SData.MissileSpeed))
            {
                ConeAngleDegrees = Convert.ToInt32(WData.SData.CastConeAngle)
            };

            SpellDataInst EData = Player.Instance.Spellbook.GetSpell(SpellSlot.E);
            E = new Spell.Skillshot(SpellSlot.E, Convert.ToUInt32(EData.SData.CastRange), SkillShotType.Linear, Convert.ToInt32(EData.SData.CastTime) * 1000, Convert.ToInt32(EData.SData.MissileSpeed));

            SpellDataInst RData = Player.Instance.Spellbook.GetSpell(SpellSlot.R);
            R = new Spell.Skillshot(SpellSlot.R, Convert.ToUInt32(RData.SData.CastRange), SkillShotType.Linear, Convert.ToInt32(RData.SData.CastTime) * 1000, Convert.ToInt32(RData.SData.MissileSpeed), Convert.ToInt32(RData.SData.LineWidth));
        }
    }
}
