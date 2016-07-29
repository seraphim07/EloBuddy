using EloBuddy;
using EloBuddy.SDK;
using System;

namespace BundledBuddies.Bundles.Kayle
{
    class SpellManager : SpellManagerBase
    {
        public const int E_RANGE = 400;

        public Spell.Targeted Q;
        public Spell.Targeted W;
        public Spell.Active E;
        public Spell.Targeted R;

        public SpellManager() : base()
        {
            SpellDataInst QData = Player.Instance.Spellbook.GetSpell(SpellSlot.Q);
            Q = new Spell.Targeted(SpellSlot.Q, Convert.ToUInt32(QData.SData.CastRange));
            Console.WriteLine("Q Range: " + Q.Range);

            SpellDataInst WData = Player.Instance.Spellbook.GetSpell(SpellSlot.W);
            W = new Spell.Targeted(SpellSlot.W, Convert.ToUInt32(WData.SData.CastRange));
            Console.WriteLine("W Range: " + W.Range);

            E = new Spell.Active(SpellSlot.E);

            SpellDataInst RData = Player.Instance.Spellbook.GetSpell(SpellSlot.R);
            R = new Spell.Targeted(SpellSlot.R, Convert.ToUInt32(RData.SData.CastRange));
            Console.WriteLine("R Range: " + R.Range);
        }
    }
}
