using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Enumerations;
using EloBuddy.SDK.Rendering;
using System;

namespace ClayAIO.ClayScripts.Annie
{
    class SpellManager : SpellManagerBase
    {
        public Spell.Targeted Q;
        public Spell.Skillshot W;
        public Spell.Active E;
        public Spell.Skillshot R;

        public SpellManager() : base()
        {
            SpellData QData = Player.Instance.Spellbook.GetSpell(SpellSlot.Q).SData;
            Q = new Spell.Targeted(
                SpellSlot.Q,
                Convert.ToUInt32(QData.CastRange));

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
            
            E = new Spell.Active(SpellSlot.E);

            SpellData RData = Player.Instance.Spellbook.GetSpell(SpellSlot.R).SData;
            R = new Spell.Skillshot(
                SpellSlot.R,
                Convert.ToUInt32(RData.CastRange),
                SkillShotType.Circular,
                Convert.ToInt32(RData.CastTime * 1000f),
                Convert.ToInt32(RData.MissileSpeed),
                Convert.ToInt32(RData.CastRadius));
        }

        public void OnDraw(EventArgs e)
        {
            Circle.Draw(indianRed, Q.Range, Player.Instance);
            Circle.Draw(mediumPurple, W.Range, Player.Instance);
            Circle.Draw(darkRed, R.Range, Player.Instance);

            new Geometry.Polygon.Sector(Player.Instance.ServerPosition, Game.CursorPos, (float)(W.ConeAngleDegrees * Math.PI / 180f), W.Range).Draw(System.Drawing.Color.Yellow);
            new Geometry.Polygon.Circle(Game.CursorPos, R.Radius).Draw(System.Drawing.Color.Yellow);
        }

        public void CastWToHero()
        {

        }
    }
}
