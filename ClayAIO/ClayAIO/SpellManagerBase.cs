using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Enumerations;
using SharpDX;
using System;

namespace ClayAIO
{
    class SpellManagerBase
    {
        public const string NAME_HEAL = "SummonerHeal";
        public const string NAME_BARRIER = "SummonerBarrier";
        public const string NAME_CLEANSE = "SummonerBoost";
        public const string NAME_EXHAUST = "SummonerExhaust";
        public const string NAME_GHOST = "SummonerHaste";

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

            SpellSlot igniteSlot = Player.Instance.GetSpellSlotFromName("summonerignite");
            if (!igniteSlot.Equals(SpellSlot.Unknown))
            {
                SpellDataInst igniteData = Player.Instance.Spellbook.GetSpell(igniteSlot);
                Ignite = new Spell.Targeted(igniteSlot, Convert.ToUInt32(igniteData.SData.CastRange));
            }
            else
            {
                Ignite = null;
            }

            SpellSlot flashSlot = Player.Instance.GetSpellSlotFromName("summonerflash");
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
    }
}
