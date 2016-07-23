using EloBuddy;
using EloBuddy.SDK;
using SharpDX;
using System;
using System.Linq;

namespace BundledBuddies.Bundles
{
    class BundledBase
    {
        protected ColorBGRA indianRed;
        protected ColorBGRA mediumPurple;
        protected ColorBGRA darkRed;
        protected ColorBGRA darkBlue;

        protected MenuManagerBase menuManagerBase;
        protected SpellManagerBase spellManagerBase;

        protected DamageType primaryDamageType;

        public BundledBase()
        {
            indianRed = new ColorBGRA(Color.IndianRed.R, Color.IndianRed.G, Color.IndianRed.B, 127);
            mediumPurple = new ColorBGRA(Color.MediumPurple.R, Color.MediumPurple.G, Color.MediumPurple.B, 127);
            darkRed = new ColorBGRA(Color.DarkRed.R, Color.DarkRed.G, Color.DarkRed.B, 127);
            darkBlue = new ColorBGRA(Color.DarkBlue.R, Color.DarkBlue.G, Color.DarkBlue.B, 127);
        }

        protected void Initialize()
        {
            Game.OnTick += OnTick;
            Obj_AI_Base.OnLevelUp += OnLevelUp;
        }

        private void OnLevelUp(Obj_AI_Base sender, Obj_AI_BaseLevelUpEventArgs e)
        {
            if (menuManagerBase.IsAutoSkillEnabled && sender.Equals(Player.Instance))
            {
                Spellbook spellbook = Player.Instance.Spellbook;
                SpellSlot[] spells = new SpellSlot[] { SpellSlot.R, menuManagerBase.FirstPrioritySkill, menuManagerBase.SecondPrioritySkill, menuManagerBase.ThirdPrioritySkill };

                for (int i = 0; i < spells.Length; i++)
                {
                    while (spellbook.GetSpell(spells[i]).IsUpgradable)
                    {
                        spellbook.LevelSpell(spells[i]);
                    }
                }
            }
        }

        private void OnTick(EventArgs e)
        {
            UseDefensiveItems();
            
            OnTickPermaActive();
            
            if (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.Combo))
            {
                UseOffensiveItems();

                if (menuManagerBase.UseHealCombo) UseHeal();
                if (menuManagerBase.UseBarrierCombo) UseBarrier();
                if (menuManagerBase.UseExhaust) UseExhaust();
                if (menuManagerBase.UseIgnite) UseIgnite();

                OnTickCombo();
            }
            
            if (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.Harass))
            {
                if (menuManagerBase.UseHealHarass) UseHeal();
                if (menuManagerBase.UseBarrierHarass) UseBarrier();

                OnTickHarass();
            }

            if (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.LaneClear))
            {
                OnTickLaneClear();
            }

            if (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.JungleClear))
            {
                OnTickJungleClear();
            }

            if (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.LastHit))
            {
                OnTickLastHit();
            }

            if (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.Flee))
            {
                if (menuManagerBase.UseHealFlee) UseHeal();
                if (menuManagerBase.UseBarrierFlee) UseBarrier();

                OnTickFlee();
            }
        }

        protected virtual void OnTickPermaActive()
        {
            if (menuManagerBase.UseCleanse &&
                spellManagerBase.Cleanse != null &&
                spellManagerBase.Cleanse.IsReady() &&
                IsCCed)
            {
                Core.DelayAction(() => spellManagerBase.Cleanse.Cast(), 500);
            }
        }

        protected virtual void OnTickCombo() { }
        protected virtual void OnTickHarass() { }
        protected virtual void OnTickLaneClear() { }
        protected virtual void OnTickJungleClear() { }
        protected virtual void OnTickLastHit() { }
        protected virtual void OnTickFlee() { }

        private bool IsCCed
        {
            get
            {
                return Player.HasBuffOfType(BuffType.Suppression) ||
                    Player.HasBuffOfType(BuffType.Charm) ||
                    Player.HasBuffOfType(BuffType.Flee) ||
                    Player.HasBuffOfType(BuffType.Blind) ||
                    Player.HasBuffOfType(BuffType.Polymorph) ||
                    Player.HasBuffOfType(BuffType.Snare) ||
                    Player.HasBuffOfType(BuffType.Stun) ||
                    Player.HasBuffOfType(BuffType.Taunt) ||
                    Player.HasBuff("FizzMarinerDoom") ||
                    Player.HasBuff("VladimirHemoplague") ||
                    Player.HasBuff("zedulttargetmark");
            }
        }

        private void UseDefensiveItems()
        {
            if (menuManagerBase.UseQss)
            {
                ResolveCC(ItemId.Quicksilver_Sash);
            }

            if (menuManagerBase.UseMs)
            {
                ResolveCC(ItemId.Mercurial_Scimitar);
            }

            if (menuManagerBase.UsePotion &&
                !Player.HasBuff("RegenerationPotion") &&
                Player.Instance.HealthPercent <= menuManagerBase.PotionHp &&
                Player.Instance.InventoryItems.HasItem(ItemId.Health_Potion) &&
                !Player.Instance.IsInShopRange())
            {
                InventorySlot healthPotion = Player.Instance.InventoryItems.First(i => i.Id == ItemId.Health_Potion);

                if (healthPotion.CanUseItem())
                {
                    Core.DelayAction(() => healthPotion.Cast(), 500);
                }
            }
        }

        private void ResolveCC(ItemId itemId)
        {
            if (Player.Instance.InventoryItems.HasItem(itemId) && IsCCed)
            {
                InventorySlot item = Player.Instance.InventoryItems.First(i => i.Id == itemId);

                if (item.CanUseItem())
                {
                    Core.DelayAction(() => item.Cast(), 500);
                }
            }
        }

        private void UseOffensiveItems()
        {
            if (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.Combo))
            {
                if (menuManagerBase.UseBc)
                {
                    UseTargettedItem(ItemId.Bilgewater_Cutlass);
                }

                if (menuManagerBase.UseBotrk)
                {
                    UseTargettedItem(ItemId.Blade_of_the_Ruined_King);
                }
            }
        }

        private void UseTargettedItem(ItemId itemId)
        {
            if (Player.Instance.InventoryItems.HasItem(itemId))
            {
                InventorySlot item = Player.Instance.InventoryItems.First(i => i.Id == itemId);

                if (item.CanUseItem())
                {
                    SpellDataInst itemData = Player.Instance.Spellbook.GetSpell(item.SpellSlot);
                    AIHeroClient target = TargetSelector.GetTarget(itemData.SData.CastRange, primaryDamageType);

                    if (target != null)
                    {
                        item.Cast(target);
                    }
                }
            }
        }

        private void UseHeal()
        {
            if (spellManagerBase.Heal != null && spellManagerBase.Heal.IsReady())
            {
                bool useHeal = Player.Instance.HealthPercent <= menuManagerBase.UseHealHp;
                
                if (menuManagerBase.UseHealAlly)
                {
                    useHeal |= EntityManager.Heroes.Allies.FirstOrDefault(x => x.Distance(Player.Instance) < 850 && x.HealthPercent <= menuManagerBase.UseHealAllyHp) != null;
                }

                if (useHeal)
                {
                    spellManagerBase.Heal.Cast();
                }
            }
        }

        private void UseBarrier()
        {
            if (spellManagerBase.Barrier != null &&
                spellManagerBase.Barrier.IsReady() &&
                Player.Instance.HealthPercent <= menuManagerBase.UseBarrierHp)
            {
                spellManagerBase.Barrier.Cast();
            }
        }

        private void UseExhaust()
        {
            if (spellManagerBase.Exhaust != null &&
                spellManagerBase.Exhaust.IsReady())
            {
                AIHeroClient target = TargetSelector.GetTarget(EntityManager.Heroes.Enemies.Where(x => x.Distance(Player.Instance) < spellManagerBase.Exhaust.Range && x.HealthPercent <= menuManagerBase.UseExhaustHp), primaryDamageType);

                if (target != null)
                {
                    spellManagerBase.Exhaust.Cast(target);
                }
            }
        }

        private void UseIgnite()
        {
            if (spellManagerBase.Ignite != null &&
                spellManagerBase.Ignite.IsReady())
            {
                AIHeroClient target = null;

                if (menuManagerBase.UseIgniteKillable)
                {
                    target = TargetSelector.GetTarget(EntityManager.Heroes.Enemies.Where(x => x.Distance(Player.Instance) < spellManagerBase.Ignite.Range && spellManagerBase.Ignite.GetHealthPrediction(x) <= 0.0f), primaryDamageType);
                }
                else
                {
                    target = TargetSelector.GetTarget(EntityManager.Heroes.Enemies.Where(x => x.Distance(Player.Instance) < spellManagerBase.Ignite.Range && x.HealthPercent <= menuManagerBase.UseIgniteHp), primaryDamageType);
                }

                if (target != null)
                {
                    spellManagerBase.Ignite.Cast(target);
                }
            }
        }
    }
}
