using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Enumerations;
using SharpDX;
using System;

namespace BundledBuddies.Bundles.Annie
{
    class SpellManager : SpellManagerBase
    {
        public Spell.Targeted Q;
        public Spell.Skillshot W;
        public Spell.Active E;
        public Spell.Skillshot R;

        public SpellManager() : base()
        {
            SpellDataInst QData = Player.Instance.Spellbook.GetSpell(SpellSlot.Q);
            Q = new Spell.Targeted(SpellSlot.Q, Convert.ToUInt32(QData.SData.CastRange));

            SpellDataInst WData = Player.Instance.Spellbook.GetSpell(SpellSlot.W);
            W = new Spell.Skillshot(SpellSlot.W, Convert.ToUInt32(WData.SData.CastConeDistance), SkillShotType.Cone, Convert.ToInt32(WData.SData.CastTime) * 1000, int.MaxValue)
            {
                ConeAngleDegrees = Convert.ToInt32(WData.SData.CastConeAngle)
            };
            
            E = new Spell.Active(SpellSlot.E);

            SpellDataInst RData = Player.Instance.Spellbook.GetSpell(SpellSlot.R);
            R = new Spell.Skillshot(SpellSlot.R, Convert.ToUInt32(RData.SData.CastRange), SkillShotType.Circular, Convert.ToInt32(RData.SData.CastTime) * 1000, int.MaxValue, Convert.ToInt32(RData.SData.CastRadius));
        }
    }
}
