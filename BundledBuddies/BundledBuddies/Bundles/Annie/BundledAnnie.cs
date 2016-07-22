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
                spellbook.LevelSpell(SpellSlot.R);
                spellbook.LevelSpell(menuManager.FirstPrioritySkill);
                spellbook.LevelSpell(menuManager.SecondPrioritySkill);

                foreach (SpellSlot s in new SpellSlot[] { SpellSlot.Q, SpellSlot.W, SpellSlot.E })
                {
                    if (!s.Equals(menuManager.FirstPrioritySkill) && !s.Equals(menuManager.SecondPrioritySkill))
                    {
                        spellbook.LevelSpell(s);
                    }
                }
            }
        }
    }
}