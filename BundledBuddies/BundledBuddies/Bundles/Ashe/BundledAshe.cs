using BundledBuddies.Bundles.Ashe;
using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Enumerations;
using EloBuddy.SDK.Rendering;
using SharpDX;
using System;
using System.Linq;

namespace BundledBuddies.Bundles
{
    partial class BundledAshe : BundledBase
    {
        private MenuManager menuManager;
        private SpellManager spellManager;
        
        public BundledAshe() : base()
        {
            menuManagerBase = new MenuManager();
            spellManagerBase = new SpellManager();

            menuManager = menuManagerBase as MenuManager;
            spellManager = spellManagerBase as SpellManager;

            primaryDamageType = DamageType.Physical;

            Initialize();

            Drawing.OnDraw += OnDraw;

            Chat.Print("BundledAshe loaded!");
            
        }
        
        private void OnDraw(EventArgs e)
        {
            Circle.Draw(indianRed, spellManager.Q.Range, Player.Instance);
            Circle.Draw(mediumPurple, spellManager.W.Range, Player.Instance);
            Circle.Draw(darkRed, spellManager.R.Range, Player.Instance);

            new Geometry.Polygon.Sector(Player.Instance.ServerPosition, Game.CursorPos, 55.0f * (float)Math.PI / 180.0f, spellManager.W.Range).Draw(System.Drawing.Color.Yellow);
        }
        
        protected override void OnTickPermaActive()
        {
            Geometry.Polygon.Sector test = new Geometry.Polygon.Sector(Player.Instance.ServerPosition, Game.CursorPos, 55.0f * (float)Math.PI / 180.0f, spellManager.W.Range);

            if (EntityManager.MinionsAndMonsters.GetJungleMonsters(Player.Instance.ServerPosition, spellManager.W.Range).Count(x => test.IsInside(x)) > 0)
            {
                Console.WriteLine("TEST");
            }
            else
            {
                Console.WriteLine("NULL");
            }

            base.OnTickPermaActive();
        }
        
        protected override void OnTickCombo()
        {
            base.OnTickCombo();
        }

        protected override void OnTickHarass()
        {
            base.OnTickHarass();
        }

        protected override void OnTickLaneClear()
        {
            base.OnTickLaneClear();
        }

        protected override void OnTickJungleClear()
        {
            base.OnTickJungleClear();
        }

        protected override void OnTickLastHit()
        {
            base.OnTickLastHit();
        }

        protected override void OnTickFlee()
        {
            base.OnTickFlee();
        }
    }
}