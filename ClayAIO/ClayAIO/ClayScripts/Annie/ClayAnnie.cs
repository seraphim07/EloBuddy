using ClayAIO.ClayScripts.Annie;
using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Enumerations;
using EloBuddy.SDK.Events;
using System;

namespace ClayAIO.ClayScripts
{
    class ClayAnnie : ClayBase
    {
        private MenuManager menuManager;
        private SpellManager spellManager;

        private Obj_AI_Minion autoAttackedMinion;

        public ClayAnnie() : base()
        {
            primaryDamageType = DamageType.Magical;

            menuManagerBase = new MenuManager();
            spellManagerBase = new SpellManager();

            menuManager = menuManagerBase as MenuManager;
            spellManager = spellManagerBase as SpellManager;

            Initialize();

            Drawing.OnDraw += spellManager.OnDraw;
            Orbwalker.OnPreAttack += OnPreAttack;
            Orbwalker.OnAttack += OnAttack;
            AttackableUnit.OnDamage += OnDamage;

            Chat.Print("ClayAnnie loaded!");
        }

        #region Custom Event Handlers
        private void OnDamage(AttackableUnit sender, AttackableUnitDamageEventArgs args)
        {
            if (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.Combo) &&
                spellManager.E.IsReady() &&
                menuManager.ComboUseE)
            {
                spellManager.E.Cast();
            }
        }

        private void OnPreAttack(AttackableUnit target, Orbwalker.PreAttackArgs args)
        {
            if (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.Combo) &&
                (spellManager.Q.IsReady() ||
                spellManager.W.IsReady() ||
                (spellManager.R.IsReady() && !spellManager.IsTibber)))
            {
                args.Process = false;
            }
        }

        private void OnAttack(AttackableUnit target, EventArgs args)
        {
            if (target.Type.Equals(GameObjectType.obj_AI_Minion))
            {
                autoAttackedMinion = target as Obj_AI_Minion;

                Core.DelayAction(() => autoAttackedMinion = null, 1000);
            }
        }

        private void OnTickInitiator()
        {
            Player.IssueOrder(GameObjectOrder.MoveTo, Game.CursorPos);

            if (!spellManager.IsStunUp)
            {
                spellManager.E.Cast();
            }
            else
            {
                spellManager.CastRToHero(menuManager.InitiatorNumEnemies);
            }
        }
        #endregion

        protected override void OnTickPermaActive()
        {
            if (menuManager.InitiatorKey)
            {
                OnTickInitiator();
            }

            if (menuManager.PermaActiveUseE &&
                !spellManager.IsStunUp &&
                !Player.Instance.IsRecalling())
            {
                spellManager.E.Cast();
            }

            if (spellManager.IsTibber)
            {
                AIHeroClient target = TargetSelector.GetTarget(Player.Instance.GetAutoAttackRange(), DamageType.Magical);

                if (target != null)
                {
                    Player.IssueOrder(GameObjectOrder.AutoAttackPet, target);
                }
            }

            base.OnTickPermaActive();
        }

        protected override void OnGapcloser(AIHeroClient sender, Gapcloser.GapcloserEventArgs e)
        {
            if (sender.IsEnemy && spellManager.IsStunUp)
            {
                SpellSlot[] spells = new SpellSlot[3];
                spells[0] = menuManager.GapcloserFirstPrioritySkill;
                spells[1] = menuManager.GapcloserSecondPrioritySkill;
                spells[2] = menuManager.GapcloserThirdPrioritySkill;

                bool spellCast = false;

                for (int i = 0; i < spells.Length; i++)
                {
                    switch (spells[i])
                    {
                        case SpellSlot.Q:
                            if (spellManager.Q.IsReady() &&
                                menuManager.GapcloserUseQ &&
                                spellManager.Q.Range >= Player.Instance.Distance(e.End))
                            {
                                spellCast = spellManager.Q.Cast(sender);
                            }
                            break;
                        case SpellSlot.W:
                            if (spellManager.W.IsReady() &&
                                menuManager.GapcloserUseW &&
                                spellManager.W.Range >= Player.Instance.Distance(e.End))
                            {
                                spellCast = spellManager.CastSkillshotToTarget(spellManager.W, sender);
                            }
                            break;
                        case SpellSlot.R:
                            if (spellManager.R.IsReady() &&
                                menuManager.GapcloserUseR &&
                                spellManager.R.Range >= Player.Instance.Distance(e.End))
                            {
                                spellCast = spellManager.CastSkillshotToTarget(spellManager.R, sender);
                            }
                            break;
                    }

                    if (spellCast) break;
                }
            }

            base.OnGapcloser(sender, e);
        }

        protected override void OnInterruptableSpell(Obj_AI_Base sender, Interrupter.InterruptableSpellEventArgs e)
        {
            if (sender.IsEnemy && e.DangerLevel == DangerLevel.High && spellManager.IsStunUp)
            {
                SpellSlot[] spells = new SpellSlot[3];
                spells[0] = menuManager.InterruptFirstPrioritySkill;
                spells[1] = menuManager.InterruptSecondPrioritySkill;
                spells[2] = menuManager.InterruptThirdPrioritySkill;

                bool spellCast = false;

                for (int i = 0; i < spells.Length; i++)
                {
                    switch (spells[i])
                    {
                        case SpellSlot.Q:
                            if (spellManager.Q.IsReady() &&
                                menuManager.InterruptUseQ &&
                                spellManager.Q.IsInRange(sender))
                            {
                                spellCast = spellManager.Q.Cast(sender);
                            }
                            break;

                        case SpellSlot.W:
                            if (spellManager.W.IsReady() &&
                                menuManager.InterruptUseW &&
                                spellManager.W.IsInRange(sender))
                            {
                                spellCast = spellManager.CastSkillshotToTarget(spellManager.W, sender);
                            }
                            break;

                        case SpellSlot.R:
                            if (spellManager.R.IsReady() &&
                                menuManager.InterruptUseR &&
                                spellManager.R.IsInRange(sender))
                            {
                                spellCast = spellManager.CastSkillshotToTarget(spellManager.R, sender);
                            }
                            break;
                    }

                    if (spellCast) break;
                }
            }

            base.OnInterruptableSpell(sender, e);
        }

        protected override void OnTickCombo()
        {
            if (menuManager.ComboUseQ) spellManager.CastQToHero();
            if (menuManager.ComboUseW) spellManager.CastWToHero();
            if (menuManager.ComboUseR) spellManager.CastRToHero();

            base.OnTickCombo();
        }

        protected override void OnTickHarass()
        {
            if (menuManager.HarassUseQ &&
                Player.Instance.ManaPercent >= menuManager.HarassQMana)
            {
                spellManager.CastQToHero();
            }

            if (menuManager.HarassUseW &&
                Player.Instance.ManaPercent >= menuManager.HarassWMana)
            {
                spellManager.CastWToHero();
            }

            base.OnTickHarass();
        }

        protected override void OnTickLaneClear()
        {
            if (menuManager.LaneClearUseQ) spellManager.CastQToMinion(autoAttackedMinion);

            if (menuManager.LaneClearUseW &&
                Player.Instance.ManaPercent >= menuManager.LaneClearWMana)
            {
                spellManager.CastWToMinions(menuManager.LaneClearWMinions);
            }

            base.OnTickLaneClear();
        }

        protected override void OnTickJungleClear()
        {
            if (menuManager.JungleClearUseQ) spellManager.CastQToJungle();
            if (menuManager.JungleClearUseW) spellManager.CastWToJungle();
            if (menuManager.JungleClearUseE) spellManager.E.Cast();

            base.OnTickJungleClear();
        }

        protected override void OnTickLastHit()
        {
            if (menuManager.LastHitUseQ) spellManager.CastQToMinion(autoAttackedMinion);

            base.OnTickLastHit();
        }

        protected override void OnTickFlee()
        {
            spellManager.E.Cast();

            if (menuManager.FleeStun &&
                spellManager.IsStunUp)
            {
                spellManager.CastQToHero();
                spellManager.CastWToHero();
                spellManager.CastRToHero();
            }

            base.OnTickFlee();
        }
    }
}
