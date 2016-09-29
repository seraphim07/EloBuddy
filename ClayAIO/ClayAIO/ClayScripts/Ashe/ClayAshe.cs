using EloBuddy;
using EloBuddy.SDK;
using System;
using System.Collections.Generic;
using System.Linq;
using EloBuddy.SDK.Events;
using ClayAIO.ClayScripts.Ashe;
using EloBuddy.SDK.Enumerations;

namespace ClayAIO.ClayScripts
{
    class ClayAshe : ClayBase
    {
        private MenuManager menuManager;
        private SpellManager spellManager;

        private List<AIHeroClient> comboTargets;

        public ClayAshe() : base()
        {
            primaryDamageType = DamageType.Physical;

            menuManagerBase = new MenuManager();
            spellManagerBase = new SpellManager();

            menuManager = menuManagerBase as MenuManager;
            spellManager = spellManagerBase as SpellManager;

            comboTargets = new List<AIHeroClient>();

            Initialize();

            Drawing.OnDraw += spellManager.OnDraw;
            Orbwalker.OnPreAttack += OnPreAttack;
            Orbwalker.OnPostAttack += OnPostAttack;
            Obj_AI_Base.OnProcessSpellCast += OnProcessSpellCast;
            
            Chat.Print("ClayAshe loaded!");
        }

        #region Custom Event Handlers
        private void OnPreAttack(AttackableUnit target, Orbwalker.PreAttackArgs args)
        {
            if (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.Combo) &&
                menuManager.ComboUseQ)
            {
                spellManager.Q.Cast();
            }

            if (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.JungleClear) &&
                menuManager.JungleClearUseQ)
            {
                spellManager.Q.Cast();
            }
        }

        private void OnPostAttack(AttackableUnit target, EventArgs e)
        {
            if (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.Combo) &&
                menuManager.ComboUseW)
            {
                spellManager.CastWToHero();
            }

            if (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.Harass) &&
                menuManager.HarassUseW &&
                Player.Instance.ManaPercent >= menuManager.HarassWMana)
            {
                spellManager.CastWToHero();
            }

            if (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.JungleClear) &&
                menuManager.JungleClearUseW)
            {
                spellManager.CastWToJungle();
            }
        }

        private void OnProcessSpellCast(Obj_AI_Base sender, GameObjectProcessSpellCastEventArgs e)
        {
            if (sender.IsMe && e.Slot == SpellSlot.W)
            {
                Orbwalker.ResetAutoAttack();
            }
        }
        
        private void OnTickFireUlt()
        {
            Player.IssueOrder(GameObjectOrder.MoveTo, Game.CursorPos);

            spellManager.CastRToTarget();
        }
        #endregion

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
        
        protected override void OnGapcloser(AIHeroClient sender, Gapcloser.GapcloserEventArgs e)
        {
            if (sender.IsEnemy && Player.Instance.GetAutoAttackRange() >= Player.Instance.Distance(e.End))
            {
                if (menuManager.GapcloserUseR)
                {
                    spellManager.CastSkillshotToTarget(spellManager.R, sender);
                    /*Core.DelayAction(delegate
                    {
                        spellManager.CastR(sender);
                    }, 1000);*/
                }
            }

            base.OnGapcloser(sender, e);
        }
        
        protected override void OnInterruptableSpell(Obj_AI_Base sender, Interrupter.InterruptableSpellEventArgs e)
        {
            if (sender.IsEnemy && e.DangerLevel == DangerLevel.High && Player.Instance.IsInAutoAttackRange(sender))
            {
                if (menuManager.InterruptUseR)
                {
                    spellManager.CastSkillshotToTarget(spellManager.R, sender);
                }
            }

            base.OnInterruptableSpell(sender, e);
        }
        
        protected override void OnTickCombo()
        {
            if (menuManager.ComboUseE &&
                spellManager.E.IsReady())
            {
                foreach (AIHeroClient target in comboTargets)
                {
                    if (!target.IsVisible)
                    {
                        spellManager.E.Cast(target.ServerPosition);
                    }
                }
            }

            comboTargets.Clear();

            foreach (AIHeroClient target in EntityManager.Heroes.Enemies.Where(x => Player.Instance.IsInAutoAttackRange(x)))
            {
                comboTargets.Add(target);
            }
            
            if (menuManager.ComboUseR)
            {
                spellManager.CastRToHero();
            }

            base.OnTickCombo();
        }
        
        protected override void OnTickHarass()
        {
            if (menuManager.HarassUseW &&
                Player.Instance.ManaPercent >= menuManager.HarassWMana)
            {
                spellManager.CastWToHero();
            }
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
            if (menuManager.FleeUseW)
            {
                spellManager.CastWToHero();
            }
        }
    }
}
