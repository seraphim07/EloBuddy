using BundledBuddies.Bundles.Annie;
using EloBuddy;
using EloBuddy.SDK.Rendering;
using System;

namespace BundledBuddies.Bundles
{
    class BundledAnnie : BundledBase
    {
        public BundledAnnie() : base()
        {
            MenuManager.Initialize();
            SpellManager.Initialize();

            Chat.Print("BundledAnnie loaded!");
        }

        protected override void OnDraw(EventArgs e)
        {
            Circle.Draw(indianRed, SpellManager.Q.Range, Player.Instance);
            Circle.Draw(mediumPurple, SpellManager.W.Range, Player.Instance);
            Circle.Draw(darkRed, SpellManager.R.Range, Player.Instance);
        }

        protected override void OnLevelUp(Obj_AI_Base sender, Obj_AI_BaseLevelUpEventArgs e)
        {
            if (MenuManager.IsAutoSkillEnabled && sender.Equals(Player.Instance))
            {
                Spellbook spellbook = Player.Instance.Spellbook;
                spellbook.LevelSpell(SpellSlot.R);
                spellbook.LevelSpell(MenuManager.FirstPrioritySkill);
                spellbook.LevelSpell(MenuManager.SecondPrioritySkill);

                foreach (SpellSlot s in new SpellSlot[] { SpellSlot.Q, SpellSlot.W, SpellSlot.E })
                {
                    if (!s.Equals(MenuManager.FirstPrioritySkill) && !s.Equals(MenuManager.SecondPrioritySkill))
                    {
                        spellbook.LevelSpell(s);
                    }
                }
            }
        }
    }
}