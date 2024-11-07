using MTM101BaldAPI.Registers;
using nbppfe.Enums;
using nbppfe.Extensions;
using nbppfe.Patches;
using System.Collections.Generic;
using UnityEngine;

namespace nbppfe.CustomContent.CustomItems
{
    public class ITM_Present : Item
    {
        public override bool Use(PlayerManager pm)
        {
            if (PickupPatch.lastPíckupClicked.item.itemType == CustomItemsEnum.Present.ToItemEnum())
            {
                Dictionary<Items, int> itemCounts = new Dictionary<Items, int>();

                for (int i = 0; i < ItemMetaStorage.Instance.All().Length; i++)
                {
                    if (!itemCounts.ContainsKey(ItemMetaStorage.Instance.All()[i].value.itemType))
                    {
                        items.Add(ItemMetaStorage.Instance.All()[i].value);
                        itemCounts.Add(ItemMetaStorage.Instance.All()[i].value.itemType, 1);
                    }
                }

                items.Shuffle();
                items.Remove(Items.None.ToItem());
                items.Remove(Items.BusPass.ToItem());
                items.Remove(CustomItemsEnum.Pickaxe.ToItem());
                items.Remove(Items.Map.ToItem());
                items.Remove(CustomItemsEnum.Present.ToItem());

                pm.ec.CreateItem(pm.plm.Entity.CurrentRoom, items[Random.Range(0, items.Count)], new Vector2(PickupPatch.lastPíckupClicked.transform.position.x, PickupPatch.lastPíckupClicked.transform.position.z));
                Destroy(gameObject);
            }
            return false;
        }



        public List<ItemObject> items = [];
    }
}
