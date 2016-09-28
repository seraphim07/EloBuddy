using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Enumerations;
using EloBuddy.SDK.Rendering;
using System;

namespace ClayAIO.ClayScripts.Tryndamere
{
    class SpellManager : SpellManagerBase
    {
        public Spell.Active Q;
        public Spell.Active W;
        public Spell.Skillshot E;
        public Spell.Active R;

        public SpellManager() : base()
        {
            Q = new Spell.Active(SpellSlot.Q);

            W = new Spell.Active(SpellSlot.W);

            SpellData EData = Player.Instance.Spellbook.GetSpell(SpellSlot.E).SData;
            E = new Spell.Skillshot(
                SpellSlot.E,
                Convert.ToUInt32(EData.CastRange),
                SkillShotType.Linear,
                Convert.ToInt32(EData.CastTime * 1000),
                Convert.ToInt32(EData.MissileSpeed),
                Convert.ToInt32(EData.LineWidth));

            R = new Spell.Active(SpellSlot.R);
        }

        public void OnDraw(EventArgs e)
        {
            Circle.Draw(indianRed, E.Range, Player.Instance);

            // new Geometry.Polygon.Rectangle(Player.Instance.ServerPosition, Game.CursorPos, E.Width).Draw(System.Drawing.Color.Yellow);

            Chat.Print(GetQHealAmount());
        }

        public int GetQHealAmount()
        {
            double baseHealAmount = 20 + Q.Level * 10;
            double baseHealBonusApAmount = Player.Instance.TotalMagicalDamage * 0.3;

            double healPerFuryAmount = 0.05 + Q.Level * 0.45;
            double healPerFuryBonusApAmount = Player.Instance.TotalMagicalDamage * 0.012;

            return Convert.ToInt32(baseHealAmount + baseHealBonusApAmount + (healPerFuryAmount + healPerFuryBonusApAmount) * Player.Instance.Mana);
        }
    }
}
