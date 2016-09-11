using EloBuddy;
using EloBuddy.SDK;
using System;
using System.Collections.Generic;
using System.Linq;
using EloBuddy.SDK.Events;

namespace ClayAIO.ClayScripts
{
    partial class ClayAshe : ClayBase
    {
        private MenuManager menuManager;
        private SpellManager spellManager;

        private List<AIHeroClient> comboTargets;

        public ClayAshe() : base()
        {
            menuManagerBase = new MenuManager();
            spellManagerBase = new SpellManager();

            menuManager = menuManagerBase as MenuManager;
            spellManager = spellManagerBase as SpellManager;

            comboTargets = new List<AIHeroClient>();

            Initialize();

            Drawing.OnDraw += spellManager.OnDraw;
            Orbwalker.OnPostAttack += OnPostAttack;
            
            Chat.Print("ClayAshe loaded!");
        }

        private void OnPostAttack(AttackableUnit target, EventArgs e)
        {
            if (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.Combo) &&
                menuManager.ComboUseW)
            {
                if (spellManager.CastWToHero())
                {
                    Orbwalker.ResetAutoAttack();
                }
            }

            if (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.Harass) &&
                menuManager.HarassUseW &&
                Player.Instance.ManaPercent >= menuManager.HarassWMana)
            {
                if (spellManager.CastWToHero())
                {
                    Orbwalker.ResetAutoAttack();
                }
            }

            if (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.JungleClear) &&
                menuManager.JungleClearUseW)
            {
                if (spellManager.CastWToJungle())
                {
                    Orbwalker.ResetAutoAttack();
                }
            }
        }

        protected override void OnTickPermaActive()
        {
            if (menuManager.FireUltKey)
            {
                OnTickFireUlt();
            }

            if (!Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.Combo))
            {
                comboTargets.Clear();
            }

            base.OnTickPermaActive();
        }

        private void OnTickFireUlt()
        {
            Player.IssueOrder(GameObjectOrder.MoveTo, Game.CursorPos);

            spellManager.CastRToTarget();
        }

        protected override void OnGapcloser(AIHeroClient sender, Gapcloser.GapcloserEventArgs e)
        {
            base.OnGapcloser(sender, e);
        }

        protected override void OnInterruptableSpell(Obj_AI_Base sender, Interrupter.InterruptableSpellEventArgs e)
        {
            base.OnInterruptableSpell(sender, e);
        }

        protected override void OnTickCombo()
        {
            if (menuManager.ComboUseE &&
                Player.Instance.Spellbook.CanUseSpell(SpellSlot.E) == SpellState.Ready)
            {
                foreach (AIHeroClient target in comboTargets)
                {
                    if (!target.IsVisible)
                    {
                        Player.Instance.Spellbook.CastSpell(SpellSlot.E, target.ServerPosition);
                    }
                }

                comboTargets.Clear();

                foreach (AIHeroClient target in EntityManager.Heroes.Enemies.Where(x => Player.Instance.IsInAutoAttackRange(x)))
                {
                    comboTargets.Add(target);
                }
            }

            if (menuManager.ComboUseQ)
            {
                spellManager.CastQ(EntityManager.Heroes.Enemies);
            }

            if (menuManager.ComboUseR)
            {
                spellManager.CastRToHero();
            }

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
            if (menuManager.JungleClearUseQ)
            {
                spellManager.CastQ(EntityManager.MinionsAndMonsters.GetJungleMonsters());
            }

            base.OnTickJungleClear();
        }

        protected override void OnTickLastHit()
        {
            base.OnTickLastHit();
        }

        protected override void OnTickFlee()
        {
            if (menuManager.FleeUseW)
            {
                spellManager.CastWToHero();
            }
        }
    }
}
