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

            Chat.Print("ClayTryndamere loaded!");
        }
    }
}
