using ClayAIO.ClayScripts.Tryndamere;
using EloBuddy;

namespace ClayAIO.ClayScripts
{
    class ClayTryndamere : ClayBase
    {
        private MenuManager menuManager;
        private SpellManager spellManager;

        public ClayTryndamere() : base()
        {
            menuManagerBase = new MenuManager();
            spellManagerBase = new SpellManager();

            menuManager = menuManagerBase as MenuManager;
            spellManager = spellManagerBase as SpellManager;

            Initialize();

            Drawing.OnDraw += spellManager.OnDraw;
            AttackableUnit.OnDamage += OnDamage;

            Chat.Print("ClayTryndamere loaded!");
        }

        private void OnDamage(AttackableUnit sender, AttackableUnitDamageEventArgs args)
        {
            if (args.Target.IsMe)
            {
                if (menuManager.PermaActiveUseQ)
                {

                }
            }
        }
    }
}
