using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Enumerations;
using System;

namespace BundledBuddies.Bundles.Annie
{
    static class SpellManager
    {
        public static Spell.Targeted Q;
        public static Spell.Skillshot W;
        public static Spell.Active E;
        public static Spell.Skillshot R;

        public static void Initialize()
        {
            SpellDataInst QData = Player.Instance.Spellbook.GetSpell(SpellSlot.Q);
            Q = new Spell.Targeted(SpellSlot.Q, Convert.ToUInt32(QData.SData.CastRange));

            SpellDataInst WData = Player.Instance.Spellbook.GetSpell(SpellSlot.W);
            W = new Spell.Skillshot(SpellSlot.W, Convert.ToUInt32(WData.SData.CastRange), SkillShotType.Cone, 250, Convert.ToInt32(WData.SData.MissileSpeed), Convert.ToInt32(WData.SData.LineWidth))
            {
                AllowedCollisionCount = int.MaxValue
            };

            E = new Spell.Active(SpellSlot.E);

            SpellDataInst RData = Player.Instance.Spellbook.GetSpell(SpellSlot.R);
            R = new Spell.Skillshot(SpellSlot.R, Convert.ToUInt32(RData.SData.CastRange), SkillShotType.Circular, 250, Convert.ToInt32(RData.SData.MissileSpeed), Convert.ToInt32(RData.SData.LineWidth))
            {
                AllowedCollisionCount = int.MaxValue
            };
        }
    }
}
