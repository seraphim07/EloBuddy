using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Events;
using System;
using System.Linq;

namespace ClayAIO
{
    class ClayBase
    {
        protected MenuManagerBase menuManagerBase;
        protected SpellManagerBase spellManagerBase;

        protected DamageType primaryDamageType;
        
        protected void Initialize()
        {
            Game.OnTick += OnTick;
            Obj_AI_Base.OnLevelUp += OnLevelUp;
            Gapcloser.OnGapcloser += OnGapcloser;
            Interrupter.OnInterruptableSpell += OnInterruptableSpell;
        }

        private void OnLevelUp(Obj_AI_Base sender, Obj_AI_BaseLevelUpEventArgs e)
        {
            if (menuManagerBase.IsAutoSkillEnabled && sender.Name.Equals(Player.Instance.Name))
            {
                Spellbook spellbook = Player.Instance.Spellbook;
                SpellSlot[] spells = new SpellSlot[] { SpellSlot.R, menuManagerBase.FirstPrioritySkill, menuManagerBase.SecondPrioritySkill, menuManagerBase.ThirdPrioritySkill };

                for (int i = 0; i < spells.Length; i++)
                {
                    if (!spellbook.GetSpell(spells[i]).IsLearned)
                    {
                        spellbook.LevelSpell(spells[i]);
                    }
                }

                for (int i = 0; i < spells.Length; i++)
                {
                    spellbook.LevelSpell(spells[i]);
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

                OnTickCombo();
            }

            if (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.Harass))
            {
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

        protected virtual void OnGapcloser(AIHeroClient sender, Gapcloser.GapcloserEventArgs e) { }
        protected virtual void OnInterruptableSpell(Obj_AI_Base sender, Interrupter.InterruptableSpellEventArgs e) { }

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
            if (spellManagerBase.Heal != null &&
                spellManagerBase.Heal.IsReady() &&
                Player.Instance.HealthPercent <= menuManagerBase.UseHealHp &&
                EntityManager.Heroes.Enemies.Count(x => x.Distance(Player.Instance.ServerPosition) <= 1000) > 0)
            {
                spellManagerBase.Heal.Cast();
            }
        }
    }
}
