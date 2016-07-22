using BundledBuddies.Bundles.Annie;
using EloBuddy;
using EloBuddy.SDK.Rendering;
using System;

namespace BundledBuddies.Bundles
{
    class BundledAnnie : BundledBase
    {
        private MenuManager menuManager;
        private SpellManager spellManager;

        public BundledAnnie() : base()
        {
            menuManager = new MenuManager();
            spellManager = new SpellManager();

            Initialize();

            Chat.Print("BundledAnnie loaded!");
        }

        protected override void OnDraw(EventArgs e)
        {
            Circle.Draw(indianRed, spellManager.Q.Range, Player.Instance);
            Circle.Draw(mediumPurple, spellManager.W.Range, Player.Instance);
            Circle.Draw(darkRed, spellManager.R.Range, Player.Instance);
        }

        protected override void OnLevelUp(Obj_AI_Base sender, Obj_AI_BaseLevelUpEventArgs e)
        {
            if (menuManager.IsAutoSkillEnabled && sender.Equals(Player.Instance))
            {
                Spellbook spellbook = Player.Instance.Spellbook;
                SpellSlot[] spells = new SpellSlot[] { SpellSlot.R, menuManager.FirstPrioritySkill, menuManager.SecondPrioritySkill, menuManager.ThirdPrioritySkill };
                
                for (int i = 0; i < spells.Length; i++)
                {
                    while (spellbook.GetSpell(spells[i]).IsUpgradable)
                    {
                        spellbook.LevelSpell(spells[i]);
                    }
                }
            }
        }
    }
}