using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Events;
using System;
using System.Linq;

namespace ClayTutorial
{
    class Program
    {
        static ItemId[] ItemBuild = new ItemId[]
        {
            ItemId.Dorans_Blade,
            ItemId.Berserkers_Greaves,
            ItemId.Essence_Reaver,
            ItemId.Runaans_Hurricane,
            ItemId.Infinity_Edge,
            ItemId.Lord_Dominiks_Regards
        };

        static int currentItemIndex = 0;

        static void Main(string[] args)
        {
            Loading.OnLoadingComplete += OnLoadingComplete;
        }

        private static void OnLoadingComplete(EventArgs e)
        {
            Core.DelayAction(() =>
            {
                Game.OnTick += OnTick;
            }, 30000);
        }

        private static void OnTick(EventArgs args)
        {
            if (Shop.CanShop)
            {
                BuyItem();
            }

            foreach (SpellSlot spellSlot in new SpellSlot[] { SpellSlot.Q, SpellSlot.W, SpellSlot.E, SpellSlot.R })
            {
                Player.Instance.Spellbook.LevelSpell(spellSlot);
            }

            Player.IssueOrder(GameObjectOrder.AttackTo, ObjectManager.Get<Obj_SpawnPoint>().FirstOrDefault(x => x.IsEnemy));
        }

        private static void BuyItem()
        {
            if (Shop.BuyItem(ItemBuild[currentItemIndex]))
            {
                currentItemIndex++;
                BuyItem();
            }
        }
    }
}
